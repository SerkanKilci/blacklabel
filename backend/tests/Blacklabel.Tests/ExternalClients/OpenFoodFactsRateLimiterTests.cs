using Blacklabel.Infrastructure.ExternalClients;
using Xunit;

namespace Blacklabel.Tests.ExternalClients;

public class OpenFoodFactsRateLimiterTests
{
    [Fact]
    public async Task WaitForPermitAsync_Grants_Up_To_The_Configured_Permit_Limit_Immediately()
    {
        using var limiter = new OpenFoodFactsRateLimiter();

        for (var i = 0; i < 12; i++)
        {
            var acquired = await limiter.WaitForPermitAsync(CancellationToken.None, TimeSpan.FromMilliseconds(50));
            Assert.True(acquired, $"expected permit {i + 1} of 12 to be granted immediately");
        }
    }

    [Fact]
    public async Task WaitForPermitAsync_Returns_False_Rather_Than_Hanging_Once_The_Budget_Is_Exhausted()
    {
        using var limiter = new OpenFoodFactsRateLimiter();

        for (var i = 0; i < 12; i++)
        {
            await limiter.WaitForPermitAsync(CancellationToken.None, TimeSpan.FromMilliseconds(50));
        }

        // The 1-minute window won't replenish within this short wait, so this must come back
        // false quickly (and queue rather than throw) instead of blocking the caller for a
        // live, user-facing HTTP request.
        var acquired = await limiter.WaitForPermitAsync(CancellationToken.None, TimeSpan.FromMilliseconds(50));

        Assert.False(acquired);
    }
}
