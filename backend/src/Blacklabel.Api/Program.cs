using System.Text;
using System.Threading.RateLimiting;
using Blacklabel.Application;
using Blacklabel.Application.Auth;
using Blacklabel.Infrastructure;
using Blacklabel.Infrastructure.Persistence;
using Blacklabel.Infrastructure.Storage;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the JWT token returned by POST /api/v1/auth/device."
    });
});

builder.Services.AddHttpClient();

// Per-IP request throttling. Two layers: a generous global limit as blunt DDoS/flood defense
// across every endpoint, and a much stricter named policy for POST /auth/device specifically —
// that endpoint is unauthenticated by necessity (it's how a device *gets* a token) and creates a
// new AppUser row per unrecognized device ID, so without its own limit a client could mint
// unlimited free accounts (each with its own fresh 10-scan/day allowance) just by sending a new
// random device ID per request, defeating the daily-limit abuse control in ProductLookupService
// entirely. Partitioned by remote IP; if this API ends up behind a reverse proxy/load balancer,
// UseForwardedHeaders() must be configured too, or every request will appear to share the proxy's
// IP and get rate-limited together.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(1),
                PermitLimit = 100,
                QueueLimit = 0
            }));

    options.AddPolicy("auth", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(1),
                PermitLimit = 10,
                QueueLimit = 0
            }));
});

if (builder.Environment.IsDevelopment())
{
    // Only needed because the Expo web build runs on a different origin (e.g.
    // localhost:8090) than this API (localhost:5236) during local browser testing —
    // native builds never go through a browser's CORS layer. Not registered outside
    // Development.
    builder.Services.AddCors(options => options.AddPolicy("DevWeb", policy =>
        policy.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod()));
}

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions();
builder.Services.AddSingleton(jwtOptions);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSerilogRequestLogging();

// Without this, an unhandled exception anywhere downstream (a bug we haven't found yet) reaches
// the client as a bare, bodyless 500 -- Kestrel's default with no handler configured. This turns
// that into a consistent application/problem+json body and, more importantly, makes sure the
// exception itself gets logged with full detail even if it's swallowed before Serilog's request
// logger would otherwise see it. Never includes the exception message/stack trace in the response
// -- full detail goes to the log, not to API clients.
app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
{
    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
    if (exceptionFeature is not null)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exceptionFeature.Error, "Unhandled exception on {Path}", exceptionFeature.Path);
    }

    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    context.Response.ContentType = "application/problem+json";
    await context.Response.WriteAsJsonAsync(new ProblemDetails
    {
        Status = StatusCodes.Status500InternalServerError,
        Title = "An unexpected error occurred."
    });
}));

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevWeb");

    if (app.Configuration.GetValue<bool>("Database:UseInMemory"))
    {
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<BlacklabelDbContext>().Database.EnsureCreated();
    }
}

app.UseHttpsRedirection();

var imageStorageOptions = app.Services.GetRequiredService<ImageStorageOptions>();
Directory.CreateDirectory(imageStorageOptions.RootPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(imageStorageOptions.RootPath),
    RequestPath = imageStorageOptions.PublicPathPrefix,
    ContentTypeProvider = new FileExtensionContentTypeProvider()
});

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
