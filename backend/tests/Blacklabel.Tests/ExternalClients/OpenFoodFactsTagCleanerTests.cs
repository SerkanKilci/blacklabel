using Blacklabel.Application.ExternalModels;
using Xunit;

namespace Blacklabel.Tests.ExternalClients;

public class OpenFoodFactsTagCleanerTests
{
    [Theory]
    [InlineData("en:e330", "E330")]
    [InlineData("en:e100", "E100")]
    [InlineData("fr:e100", "E100")]
    [InlineData("e330", "E330")]
    public void CleanAdditiveTag_Strips_Language_Prefix_And_Uppercases(string tag, string expected)
    {
        Assert.Equal(expected, OpenFoodFactsTagCleaner.CleanAdditiveTag(tag));
    }

    [Theory]
    [InlineData("en:gluten", "gluten")]
    [InlineData("en:sesame-seeds", "sesame-seeds")]
    [InlineData("EN:MILK", "milk")]
    [InlineData("gluten", "gluten")]
    public void CleanAllergenTag_Strips_Language_Prefix_And_Lowercases(string tag, string expected)
    {
        Assert.Equal(expected, OpenFoodFactsTagCleaner.CleanAllergenTag(tag));
    }
}
