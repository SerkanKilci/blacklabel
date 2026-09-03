using Blacklabel.Application.Matching;
using Xunit;

namespace Blacklabel.Tests.Matching;

public class AllergenKeywordScannerTests
{
    [Theory]
    [InlineData("Wheat flour, sugar, milk powder, salt")]
    [InlineData("Un, şeker, süt tozu, tuz")]
    [InlineData("Farine de blé, sucre, lait")]
    [InlineData("Harina de trigo, azúcar, leche")]
    [InlineData("Weizenmehl, Zucker, Eier")]
    [InlineData("Contains: peanuts and tree nuts (almond, walnut)")]
    public void MentionsPossibleAllergen_True_For_Text_Naming_A_Known_Allergen(string text)
    {
        Assert.True(AllergenKeywordScanner.MentionsPossibleAllergen(text));
    }

    [Theory]
    [InlineData("Water, sugar, citric acid, natural flavoring")]
    [InlineData("Su, şeker, sitrik asit")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void MentionsPossibleAllergen_False_For_Text_With_No_Allergen_Mention(string? text)
    {
        Assert.False(AllergenKeywordScanner.MentionsPossibleAllergen(text));
    }

    [Fact]
    public void MentionsPossibleAllergen_Does_Not_False_Positive_On_Common_Words_Containing_Short_Fragments()
    {
        // "table", "vegetable" etc. contain "ble" -- must not trip a bare wheat-fragment match.
        Assert.False(AllergenKeywordScanner.MentionsPossibleAllergen("Vegetable oil, table salt, stabilizer"));
    }
}
