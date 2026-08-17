const VALID_LENGTHS = [8, 13, 14];

/**
 * Client-side mirror of the backend's BarcodeNormalizer (strip non-digits, pad a 12-digit
 * UPC-A to EAN-13, accept 8/13/14-digit results). Used to give instant feedback on the manual
 * barcode entry field instead of round-tripping to the API to learn the input was invalid.
 */
export function normalizeBarcode(raw: string): string | null {
  const digitsOnly = raw.replace(/\D/g, '');
  const padded = digitsOnly.length === 12 ? `0${digitsOnly}` : digitsOnly;
  return VALID_LENGTHS.includes(padded.length) ? padded : null;
}
