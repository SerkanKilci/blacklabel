using System.Text.Json;
using Blacklabel.Application.Interfaces;
using Blacklabel.Application.Scoring;
using Blacklabel.Infrastructure.Persistence;
using Blacklabel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OffImporter;
using OffImporter.Configuration;
using OffImporter.Dump;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables()
    .Build();

var importOptions = configuration.GetSection("OffImporter").Get<ImportOptions>() ?? new ImportOptions();
if (string.IsNullOrWhiteSpace(importOptions.DumpSource))
{
    Console.Error.WriteLine("OffImporter:DumpSource is not configured. Set it in appsettings.json or the OffImporter__DumpSource environment variable.");
    return 1;
}

var connectionString = configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.Error.WriteLine("ConnectionStrings:DefaultConnection is not configured. Set it via 'dotnet user-secrets set ConnectionStrings:DefaultConnection ...' or the ConnectionStrings__DefaultConnection environment variable.");
    return 1;
}

var thresholds = configuration.GetSection("ScoreThresholds").Get<ScoreThresholds>() ?? new ScoreThresholds();

var services = new ServiceCollection();
services.AddDbContext<BlacklabelDbContext>(options => options.UseSqlServer(connectionString));
services.AddScoped<IAdditiveRepository, AdditiveRepository>();
services.AddScoped<IAllergenRepository, AllergenRepository>();
services.AddSingleton(thresholds);
services.AddSingleton<ScoreCalculator>();
services.AddScoped<ProductUpsertService>();
services.AddHttpClient();

await using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();

var httpClient = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();
httpClient.Timeout = TimeSpan.FromMinutes(30);
httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Blacklabel-OffImporter/1.0 (contact@blacklabel.app)");

var dbContext = scope.ServiceProvider.GetRequiredService<BlacklabelDbContext>();
var upsertService = scope.ServiceProvider.GetRequiredService<ProductUpsertService>();

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

Console.WriteLine($"Blacklabel OFF Importer starting. Source: {importOptions.DumpSource}");

long processedLines = 0;
long matched = 0;
long created = 0;
long updated = 0;
long invalidBarcode = 0;
long parseErrors = 0;
var pendingSinceLastSave = 0;

try
{
    await foreach (var line in DumpLineReader.ReadLinesAsync(importOptions.DumpSource, httpClient, cts.Token))
    {
        processedLines++;

        OffDumpProduct? raw = null;
        try
        {
            raw = JsonSerializer.Deserialize<OffDumpProduct>(line);
        }
        catch (JsonException)
        {
            parseErrors++;
        }

        if (raw is not null)
        {
            var result = await upsertService.UpsertAsync(raw, cts.Token);
            switch (result)
            {
                case ProductUpsertService.Result.Created:
                    matched++;
                    created++;
                    pendingSinceLastSave++;
                    break;
                case ProductUpsertService.Result.Updated:
                    matched++;
                    updated++;
                    pendingSinceLastSave++;
                    break;
                case ProductUpsertService.Result.SkippedInvalidBarcode:
                    invalidBarcode++;
                    break;
            }
        }

        if (pendingSinceLastSave >= importOptions.BatchSize)
        {
            await dbContext.SaveChangesAsync(cts.Token);
            dbContext.ChangeTracker.Clear();
            pendingSinceLastSave = 0;
        }

        if (processedLines % importOptions.ProgressIntervalLines == 0)
        {
            Console.WriteLine(
                $"Processed {processedLines} lines | matched (TR) {matched} (created {created}, updated {updated}) | invalid barcode {invalidBarcode} | parse errors {parseErrors}");
        }
    }

    if (pendingSinceLastSave > 0)
    {
        await dbContext.SaveChangesAsync(cts.Token);
    }

    Console.WriteLine(
        $"Done. Processed {processedLines} lines | matched (TR) {matched} (created {created}, updated {updated}) | invalid barcode {invalidBarcode} | parse errors {parseErrors}");
    return 0;
}
catch (OperationCanceledException)
{
    Console.WriteLine("Import cancelled. Batches already saved before cancellation remain committed, so re-running the importer is safe.");
    return 1;
}
