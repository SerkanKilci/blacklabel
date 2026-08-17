namespace Blacklabel.Application.Barcode;

public static class BarcodeNormalizer
{
    private static readonly int[] ValidLengths = { 8, 13, 14 };

    public static string? Normalize(string? rawBarcode)
    {
        if (string.IsNullOrWhiteSpace(rawBarcode))
        {
            return null;
        }

        var digitsOnly = new string(rawBarcode.Where(char.IsDigit).ToArray());

        if (digitsOnly.Length == 12)
        {
            digitsOnly = "0" + digitsOnly;
        }

        return ValidLengths.Contains(digitsOnly.Length) ? digitsOnly : null;
    }
}
