using Blacklabel.Application.Etl;
using Xunit;

namespace Blacklabel.Tests.Etl;

public class TargetMarketFilterTests
{
    [Theory]
    [InlineData("8690504010104")] // Turkey
    [InlineData("8681234567890")] // Turkey
    [InlineData("0012345678905")] // US/Canada block (000-139)
    [InlineData("0125552346001")] // US/Canada block (000-139)
    [InlineData("7541234567890")] // Canada dedicated block (754-755)
    public void IsTargetMarketProduct_Matches_Gs1_Prefix_Ranges(string barcode)
    {
        Assert.True(TargetMarketFilter.IsTargetMarketProduct(barcode, Array.Empty<string>()));
    }

    [Fact]
    public void IsTargetMarketProduct_Rejects_Prefix_Outside_Any_Target_Range_Without_Country_Tag()
    {
        // 400-440 is Germany's GS1 block: falls outside the numeric ranges we match directly,
        // so this must be caught via countries_tags instead (see the Europe test below).
        Assert.False(TargetMarketFilter.IsTargetMarketProduct("4001234567892", Array.Empty<string>()));
    }

    [Theory]
    [InlineData("en:turkey")]
    [InlineData("en:TURKEY")]
    [InlineData("tr:türkiye")]
    [InlineData("en:united-states")]
    [InlineData("en:canada")]
    public void IsTargetMarketProduct_Matches_Turkey_And_NorthAmerica_Country_Tags(string countryTag)
    {
        Assert.True(TargetMarketFilter.IsTargetMarketProduct("4001234567892", new[] { countryTag }));
    }

    [Theory]
    [InlineData("en:france")]
    [InlineData("en:germany")]
    [InlineData("en:united-kingdom")]
    [InlineData("en:spain")]
    [InlineData("en:italy")]
    [InlineData("en:netherlands")]
    [InlineData("en:switzerland")]
    [InlineData("en:norway")]
    public void IsTargetMarketProduct_Matches_European_Country_Tags(string countryTag)
    {
        // German-manufacturer barcode prefix (400-440) alone wouldn't match — the country tag
        // (actual sold-in market) is what should carry this.
        Assert.True(TargetMarketFilter.IsTargetMarketProduct("4001234567892", new[] { countryTag }));
    }

    [Fact]
    public void IsTargetMarketProduct_Rejects_Unrelated_Country_And_Prefix()
    {
        // 690-699 is China's GS1 block, and "japan" isn't in any target market list.
        Assert.False(TargetMarketFilter.IsTargetMarketProduct("6901234567894", new[] { "en:japan" }));
    }

    [Fact]
    public void IsTargetMarketProduct_Rejects_Empty_Country_Tags_And_Out_Of_Range_Prefix()
    {
        Assert.False(TargetMarketFilter.IsTargetMarketProduct("6901234567894", Array.Empty<string>()));
    }
}
