using Blacklabel.Application.Interfaces;
using Blacklabel.Application.Scoring;
using Blacklabel.Application.Services;
using Blacklabel.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blacklabel.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var thresholds = configuration.GetSection("ScoreThresholds").Get<ScoreThresholds>() ?? new ScoreThresholds();
        services.AddSingleton(thresholds);
        services.AddSingleton<ScoreCalculator>();

        services.AddScoped<IProductLookupService, ProductLookupService>();
        services.AddScoped<IDeviceAuthService, DeviceAuthService>();
        services.AddScoped<IAccountLinkService, AccountLinkService>();
        services.AddScoped<IContributionService, ContributionService>();
        services.AddScoped<IScanService, ScanService>();
        services.AddScoped<ISubscriptionWebhookService, SubscriptionWebhookService>();

        services.AddValidatorsFromAssemblyContaining<UpdateUserPreferenceRequestValidator>();

        return services;
    }
}
