namespace Blacklabel.Application.ExternalModels;

public static class OpenFoodFactsTagCleaner
{
    public static string CleanAdditiveTag(string tag) => StripLanguagePrefix(tag).ToUpperInvariant();

    public static string CleanAllergenTag(string tag) => StripLanguagePrefix(tag).ToLowerInvariant();

    public static string CleanCategoryTag(string tag) => StripLanguagePrefix(tag).ToLowerInvariant();

    private static string StripLanguagePrefix(string tag)
    {
        var colonIndex = tag.IndexOf(':');
        return colonIndex >= 0 ? tag[(colonIndex + 1)..] : tag;
    }
}
