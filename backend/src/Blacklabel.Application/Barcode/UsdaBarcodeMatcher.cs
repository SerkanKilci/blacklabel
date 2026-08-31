namespace Blacklabel.Application.Barcode;

/// <summary>
/// Pure matching helpers for reconciling our barcodes against USDA FoodData Central search
/// results. FDC has no dedicated "look up by barcode" endpoint -- a barcode passed as the search
/// query is a fuzzy full-text match that can happily return a product it has never actually seen
/// the barcode for. Every candidate must be independently confirmed against its own gtinUpc field
/// before its data is trusted; see IUsdaFoodDataClient.
/// </summary>
public static class UsdaBarcodeMatcher
{
    // Same US/Canada GS1 prefix ranges as Etl.TargetMarketFilter, kept separate on purpose: this
    // predicate only decides "is FoodData Central worth querying for this barcode", not the
    // fuller countries_tags signal the ETL importer's market filter combines it with.
    private static readonly (int Min, int Max)[] UsCanadaGs1PrefixRanges =
    {
        (0, 139),
        (754, 755),
    };

    public static bool IsUsOrCanadaBarcode(string normalizedBarcode)
        => normalizedBarcode.Length >= 3
           && int.TryParse(normalizedBarcode.AsSpan(0, 3), out var prefix)
           && UsCanadaGs1PrefixRanges.Any(range => prefix >= range.Min && prefix <= range.Max);

    /// <summary>
    /// GTIN-14 canonical form: digits only, left-padded to 14. This is GS1's own equivalence
    /// rule -- a UPC-A, EAN-13, and GTIN-14 for the same item differ only in leading zeros.
    /// </summary>
    public static string Canonicalize(string digitsOnlyBarcode) => digitsOnlyBarcode.PadLeft(14, '0');

    /// <summary>True only if <paramref name="candidateGtinUpc"/> is the same item as <paramref name="ourNormalizedBarcode"/> once both are canonicalized.</summary>
    public static bool IsMatch(string ourNormalizedBarcode, string? candidateGtinUpc)
    {
        if (string.IsNullOrWhiteSpace(candidateGtinUpc))
        {
            return false;
        }

        var candidateDigits = new string(candidateGtinUpc.Where(char.IsDigit).ToArray());
        return candidateDigits.Length > 0 && Canonicalize(ourNormalizedBarcode) == Canonicalize(candidateDigits);
    }
}
