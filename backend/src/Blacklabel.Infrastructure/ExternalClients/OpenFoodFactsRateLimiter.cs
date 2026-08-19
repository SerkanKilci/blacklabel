using System.Threading.RateLimiting;

namespace Blacklabel.Infrastructure.ExternalClients;

/// <summary>
/// Throttles our own outbound calls to Open Food Facts to stay under their documented limit for
/// product read queries — 15 requests/minute per IP address
/// (https://openfoodfacts.github.io/openfoodfacts-server/api/). That's per our server's IP, not
/// per user: every cache-miss lookup from every user shares this one budget, and OFF reserves the
/// right to IP-ban a client that exceeds it, which would break lookups for every user of this app
/// at once, not just the request that tripped it. Capped below their limit, not at it, to leave
/// headroom for clock skew at the window boundary. Registered as a singleton — this only works as
/// real backpressure if every request server-wide shares one limiter instance.
/// </summary>
public sealed class OpenFoodFactsRateLimiter : IDisposable
{
    private readonly RateLimiter _limiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
    {
        Window = TimeSpan.FromMinutes(1),
        PermitLimit = 12,
        QueueLimit = 20,
        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
        AutoReplenishment = true
    });

    private static readonly TimeSpan DefaultWaitTimeout = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Waits for a permit, queueing briefly if the budget is currently exhausted. Returns false
    /// (rather than waiting indefinitely) if a permit isn't available within <paramref
    /// name="waitTimeout"/> (5 seconds by default), so a live user-facing request doesn't hang
    /// just because Open Food Facts traffic is bursty. Tests pass a much shorter timeout to
    /// exercise this without actually waiting seconds per case.
    /// </summary>
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
