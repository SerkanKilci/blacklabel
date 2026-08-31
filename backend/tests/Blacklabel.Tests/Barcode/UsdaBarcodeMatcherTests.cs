using Blacklabel.Application.Barcode;
using Xunit;

namespace Blacklabel.Tests.Barcode;

public class UsdaBarcodeMatcherTests
{
    [Theory]
    [InlineData("0016000275287", "00016000275287", true)]  // our 13-digit EAN vs FDC's zero-padded 14-digit GTIN
    [InlineData("0016000275287", "016000275287", true)]     // FDC's gtinUpc missing the leading zero (12 digits)
    [InlineData("0016000275287", "0016000275288", false)]   // one digit off -- must not match
    [InlineData("0016000275287", null, false)]
    [InlineData("0016000275287", "", false)]
    [InlineData("0016000275287", "not-a-barcode", false)]
    public void IsMatch_Only_True_For_The_Same_Item_After_Gtin14_Canonicalization(string ourBarcode, string? candidateGtinUpc, bool expected)
    {
        Assert.Equal(expected, UsdaBarcodeMatcher.IsMatch(ourBarcode, candidateGtinUpc));
    }

    [Theory]
    [InlineData("0016000275287", true)]  // US prefix (0xx)
    [InlineData("0754123456789", true)]  // Canada dedicated block (754)
    [InlineData("8690504010104", false)] // Turkey prefix -- not a market FDC covers
    [InlineData("4006381333931", false)] // Germany prefix
    public void IsUsOrCanadaBarcode_Matches_Only_The_Gs1_Ranges_Fdc_Covers(string barcode, bool expected)
    {
        Assert.Equal(expected, UsdaBarcodeMatcher.IsUsOrCanadaBarcode(barcode));
    }
}
