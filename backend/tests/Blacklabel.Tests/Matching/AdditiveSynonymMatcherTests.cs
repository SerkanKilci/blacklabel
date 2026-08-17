using Blacklabel.Application.Matching;
using Blacklabel.Domain.Entities;
using Xunit;

namespace Blacklabel.Tests.Matching;

public class AdditiveSynonymMatcherTests
{
    private static Additive CreateAdditive(string code, params string[] synonyms)
    {
        var synonymsJson = synonyms.Length == 0
            ? "[]"
            : "[" + string.Join(",", synonyms.Select(s => $"\"{s}\"")) + "]";

        return new Additive
        {
            Code = code,
            NameTr = code,
            NameEn = code,
            DescriptionTr = code,
            DescriptionEn = code,
            Synonyms = synonymsJson
        };
    }

    [Fact]
    public void FindCodesByName_Matches_Synonym_Present_In_Text()
    {
        var additives = new List<Additive> { CreateAdditive("E211", "sodyum benzoat", "sodium benzoate") };

        var result = AdditiveSynonymMatcher.FindCodesByName("Su, şeker, sodyum benzoat, aroma verici.", additives);

        Assert.Contains("E211", result);
    }

    [Fact]
    public void FindCodesByName_Is_Case_Insensitive()
    {
        var additives = new List<Additive> { CreateAdditive("E300", "ascorbic acid") };

        var result = AdditiveSynonymMatcher.FindCodesByName("Contains: ASCORBIC ACID and other ingredients.", additives);

        Assert.Contains("E300", result);
    }

    [Fact]
    public void FindCodesByName_Handles_Turkish_Uppercase_Dotted_I_Correctly()
    {
        // Turkish labels are commonly all-uppercase; "İ" must fold to "i" (not "i" + combining dot,
        // which invariant-culture ToLower would produce) to match lowercase synonym entries.
        var additives = new List<Additive> { CreateAdditive("E330", "sitrik asit", "citric acid") };

        var result = AdditiveSynonymMatcher.FindCodesByName("İÇİNDEKİLER: ŞEKER, SİTRİK ASİT, SU.", additives);

        Assert.Contains("E330", result);
    }

    [Fact]
    public void FindCodesByName_Does_Not_Match_Unrelated_Additives()
    {
        var additives = new List<Additive>
        {
            CreateAdditive("E211", "sodyum benzoat", "sodium benzoate"),
            CreateAdditive("E300", "askorbik asit", "ascorbic acid")
        };

        var result = AdditiveSynonymMatcher.FindCodesByName("Su, şeker, tuz.", additives);

        Assert.Empty(result);
    }

    [Fact]
    public void FindCodesByName_Returns_Empty_For_Additive_With_No_Synonyms()
    {
        var additives = new List<Additive> { CreateAdditive("E999") };

        var result = AdditiveSynonymMatcher.FindCodesByName("lesitin, sodyum benzoat", additives);

        Assert.Empty(result);
    }

    [Fact]
    public void FindCodesByName_Returns_Empty_For_Empty_Text()
    {
        var additives = new List<Additive> { CreateAdditive("E211", "sodyum benzoat") };

        var result = AdditiveSynonymMatcher.FindCodesByName(string.Empty, additives);

        Assert.Empty(result);
    }

    [Fact]
    public void FindCodesByName_Matches_Multiple_Additives_In_Same_Text()
    {
        var additives = new List<Additive>
        {
            CreateAdditive("E211", "sodyum benzoat"),
            CreateAdditive("E322", "lesitin", "lecithin")
        };

        var result = AdditiveSynonymMatcher.FindCodesByName("Buğday unu, lesitin, sodyum benzoat.", additives);

        Assert.Equal(2, result.Count);
        Assert.Contains("E211", result);
        Assert.Contains("E322", result);
    }
}
