using Blacklabel.Application.Barcode;
using Xunit;

namespace Blacklabel.Tests.Barcode;

public class BarcodeNormalizerTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Normalize_Returns_Null_For_Empty_Input(string? input)
    {
        Assert.Null(BarcodeNormalizer.Normalize(input));
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("1234567890123")]
    [InlineData("12345678901234")]
    public void Normalize_Accepts_Valid_Lengths_Unchanged(string barcode)
    {
        Assert.Equal(barcode, BarcodeNormalizer.Normalize(barcode));
    }

    [Fact]
    public void Normalize_Pads_12_Digit_UPC_A_To_EAN_13()
    {
        var result = BarcodeNormalizer.Normalize("123456789012");

        Assert.Equal("0123456789012", result);
        Assert.Equal(13, result!.Length);
    }

    [Fact]
    public void Normalize_Strips_NonDigit_Characters_Before_Validating()
    {
        var result = BarcodeNormalizer.Normalize("869-0504-010104");

        Assert.Equal("8690504010104", result);
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("123456789")]
    [InlineData("123456789012345")]
    public void Normalize_Rejects_Invalid_Lengths(string barcode)
    {
        Assert.Null(BarcodeNormalizer.Normalize(barcode));
    }

    [Fact]
    public void Normalize_Rejects_NonNumeric_Input()
    {
        Assert.Null(BarcodeNormalizer.Normalize("abcdefgh"));
    }
}
