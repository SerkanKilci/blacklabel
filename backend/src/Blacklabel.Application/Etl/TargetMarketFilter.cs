namespace Blacklabel.Application.Etl;

/// <summary>
/// Pure predicate used by the Open Food Facts ETL importer (tools/OffImporter) to decide
/// whether a dump entry belongs to one of our target markets: Turkey, the United States,
/// Canada, or Europe (EU/EEA + UK + Switzerland). Two signals are combined:
///
/// - GS1 barcode prefix: which country's GS1 Member Organization issued the code to the
///   manufacturer. Reliable for Turkey/US/Canada (contiguous, well-known ranges), but a weak
///   signal for "is this sold here" at continental scale — a German-made product (GS1 400-440)
///   is routinely sold across all of Europe, so enumerating every individual European country's
///   numeric range would both be error-prone and wouldn't actually improve precision.
/// - countries_tags: Open Food Facts' own "sold in this country" tagging. This is the primary
///   signal for Europe, and a secondary OR-catch everywhere else (covers imports/re-exports that
///   a manufacturer-prefix alone would miss — e.g. a Turkish-market product carrying a German
///   manufacturer's barcode prefix).
/// </summary>
public static class TargetMarketFilter
{
    // Inclusive GS1 prefix ranges, matched against the first 3 digits of a normalized barcode.
    private static readonly (int Min, int Max)[] Gs1PrefixRanges =
    {
        (0, 139),   // United States & Canada
        (754, 755), // Canada (dedicated block)
        (868, 869), // Turkey
    };

    private static readonly HashSet<string> CountryNames = new(StringComparer.Ordinal)
    {
        // Turkey
        "turkey", "turkiye", "türkiye",
        // North America
        "united-states", "canada",
        // Europe: EU-27 + EEA (Iceland, Liechtenstein, Norway) + United Kingdom + Switzerland
        "austria", "belgium", "bulgaria", "croatia", "cyprus", "czech-republic", "czechia",
        "denmark", "estonia", "finland", "france", "germany", "greece", "hungary", "ireland",
        "italy", "latvia", "lithuania", "luxembourg", "malta", "netherlands", "poland",
        "portugal", "romania", "slovakia", "slovenia", "spain", "sweden",
        "iceland", "liechtenstein", "norway", "united-kingdom", "switzerland",
    };

    public static bool IsTargetMarketProduct(string normalizedBarcode, IReadOnlyList<string> countriesTags)
    {
        if (normalizedBarcode.Length >= 3 &&
            int.TryParse(normalizedBarcode.AsSpan(0, 3), out var prefix) &&
            Gs1PrefixRanges.Any(range => prefix >= range.Min && prefix <= range.Max))
        {
            return true;
        }

        return countriesTags.Select(NormalizeCountryTag).Any(CountryNames.Contains);
    }

    // Both dotted and dotless capital I are folded to plain "i" before lowering so
    // "en:TURKEY" / "tr:TÜRKİYE"-style tags match regardless of source casing.
    private static string NormalizeCountryTag(string tag)
    {
        var colonIndex = tag.IndexOf(':');
        var value = colonIndex >= 0 ? tag[(colonIndex + 1)..] : tag;
        return value.Replace('İ', 'i').Replace('I', 'i').ToLowerInvariant();
    }
}
