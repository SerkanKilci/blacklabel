namespace Blacklabel.Application.Matching;

/// <summary>
/// Conservative safety-net check: does this ingredients text obviously mention one of the 14 EU
/// allergen categories, in a language our target markets (US/Canada/Turkey/Europe) actually use?
/// Used ONLY to decide whether to trust an empty allergen-tag list from the source (OFF's
/// allergens_tags, or OCR-extracted allergens) -- never to assert which allergen is present. A hit
/// downgrades DataQuality so the incomplete-data warning still shows; it never adds a row to
/// ProductAllergens. A false negative here is no worse than the existing baseline (we already
/// trust the source's own tagging); a false positive only costs one extra "unverified" warning on
/// an otherwise-fine product, so this list leans inclusive rather than exhaustive -- and avoids
/// bare fragments (e.g. "ble", "ei") that are common substrings of unrelated words.
/// </summary>
public static class AllergenKeywordScanner
{
    private static readonly string[] Keywords =
    {
        // milk
        "milk", "süt", "sut", "lait", "leche", "milch",
        // gluten / wheat
        "gluten", "wheat", "buğday", "bugday", "blé", "trigo", "weizen",
        // eggs
        "egg", "yumurta", "oeuf", "huevo", "eier",
        // peanuts
        "peanut", "fıstık", "fistik", "cacahu", "erdnuss",
        // soybeans
        "soy", "soya", "soja",
        // tree nuts
        "hazelnut", "almond", "walnut", "fındık", "findik", "badem", "ceviz", "noix", "nuez",
        // fish
        "fish", "balık", "balik", "poisson", "pescado",
        // sesame seeds
        "sesame", "susam", "sésame", "sesamo",
        // mustard
        "mustard", "hardal", "moutarde",
        // celery
        "celery", "kereviz", "céleri", "celeri",
        // sulphur dioxide and sulphites
        "sulphite", "sulfite", "sülfit", "sulfit",
        // lupin
        "lupin",
        // molluscs
        "mollusc", "mollusk", "yumuşakça", "yumusakca", "mollusque",
    };

    public static bool MentionsPossibleAllergen(string? ingredientsText)
    {
        if (string.IsNullOrWhiteSpace(ingredientsText))
        {
            return false;
        }

        var normalized = NormalizeForMatching(ingredientsText);
        return Keywords.Any(keyword => normalized.Contains(NormalizeForMatching(keyword)));
    }

    // Same Turkish dotted/dotless "I" fix as AdditiveSynonymMatcher.
    private static string NormalizeForMatching(string text)
        => text.Replace('İ', 'i').Replace('I', 'i').ToLowerInvariant();
}
