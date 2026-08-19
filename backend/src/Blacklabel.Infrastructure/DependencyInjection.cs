using Blacklabel.Application.Auth;
using Blacklabel.Application.Interfaces;
using Blacklabel.Infrastructure.Auth;
using Blacklabel.Infrastructure.ExternalClients;
using Blacklabel.Infrastructure.Persistence;
using Blacklabel.Infrastructure.Repositories;
using Blacklabel.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace Blacklabel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var useInMemoryDatabase = configuration.GetValue<bool>("Database:UseInMemory");

        services.AddDbContext<BlacklabelDbContext>(options =>
        {
            if (useInMemoryDatabase)
            {
                // Opt-in escape hatch for local manual testing on machines without Docker/SQL
                // Server (see README) — never the default, and migrations don't apply to it.
                options.UseInMemoryDatabase("Blacklabel");
            }
            else
            {
                options.UseSqlServer(connectionString);
            }
        });

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IAdditiveRepository, AdditiveRepository>();
        services.AddScoped<IAllergenRepository, AllergenRepository>();
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<IContributionRepository, ContributionRepository>();
        services.AddScoped<IHouseholdProfileRepository, HouseholdProfileRepository>();
        services.AddScoped<IScanRepository, ScanRepository>();

        var baseUrl = configuration["OpenFoodFacts:BaseUrl"] ?? "https://world.openfoodfacts.org/api/v2/product/";
        var userAgent = configuration["OpenFoodFacts:UserAgent"] ?? "Blacklabel/1.0 (contact@blacklabel.app)";

        services.AddSingleton<OpenFoodFactsRateLimiter>();
        services.AddHttpClient<IOpenFoodFactsClient, OpenFoodFactsClient>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(8);
                client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        var visionEndpoint = configuration["VisionService:Endpoint"];
        var visionTimeoutSeconds = configuration.GetValue<int?>("VisionService:TimeoutSeconds") ?? 30;

        services.AddHttpClient<IVisionService, HttpVisionService>(client =>
        {
            if (!string.IsNullOrWhiteSpace(visionEndpoint))
            {
                client.BaseAddress = new Uri(visionEndpoint);
            }

            client.Timeout = TimeSpan.FromSeconds(visionTimeoutSeconds);
        });

        var storagePath = configuration["Storage:ContributionImagesPath"] ?? "App_Data/uploads/contributions";
        var absoluteStoragePath = Path.IsPathRooted(storagePath)
            ? storagePath
            : Path.Combine(AppContext.BaseDirectory, storagePath);
        var imageStorageOptions = new ImageStorageOptions(absoluteStoragePath, "/uploads/contributions");

        services.AddSingleton(imageStorageOptions);
        services.AddSingleton<IImageStorageService>(new LocalImageStorageService(imageStorageOptions));

        services.AddScoped<ITokenService, JwtTokenService>();

        var appleBundleId = configuration["Auth:Apple:BundleId"];
        var googleClientId = configuration["Auth:Google:ClientId"];

        services.AddSingleton<IIdentityTokenVerifier>(sp =>
            new AppleIdentityTokenVerifier(appleBundleId, sp.GetRequiredService<ILogger<AppleIdentityTokenVerifier>>()));
        services.AddSingleton<IIdentityTokenVerifier>(sp =>
            new GoogleIdentityTokenVerifier(googleClientId, sp.GetRequiredService<ILogger<GoogleIdentityTokenVerifier>>()));

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromMilliseconds(200 * retryAttempt));

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}
