using System.Threading.RateLimiting;

namespace Blacklabel.Infrastructure.ExternalClients;

/// <summary>
/// Throttles our own outbound calls to USDA FoodData Central to stay under api.data.gov's
/// documented limit for a registered key -- 1,000 requests/hour per IP
/// (https://api.data.gov, confirmed via FDC's own API key signup flow). Capped below their limit,
/// not at it, for the same clock-skew-headroom reason as <see cref="OpenFoodFactsRateLimiter"/>.
/// Registered as a singleton so the whole server shares one budget.
/// </summary>
public sealed class UsdaRateLimiter : IDisposable
{
    private readonly RateLimiter _limiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
    {
        Window = TimeSpan.FromHours(1),
        PermitLimit = 800,
        QueueLimit = 50,
        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
        AutoReplenishment = true
    });

    private static readonly TimeSpan DefaultWaitTimeout = TimeSpan.FromSeconds(5);

    public async Task<bool> WaitForPermitAsync(CancellationToken ct, TimeSpan? waitTimeout = null)
    {
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        timeoutCts.CancelAfter(waitTimeout ?? DefaultWaitTimeout);

        try
        {
            using var lease = await _limiter.AcquireAsync(1, timeoutCts.Token);
            return lease.IsAcquired;
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
        {
            return false;
        }
    }

    public void Dispose() => _limiter.Dispose();
}
