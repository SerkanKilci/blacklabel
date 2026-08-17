using System.Text.Json;
using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Matching;

public static class AdditiveSynonymMatcher
{
    public static IReadOnlySet<string> FindCodesByName(string ingredientsText, IReadOnlyList<Additive> allAdditives)
    {
        var matches = new HashSet<string>();
        if (string.IsNullOrWhiteSpace(ingredientsText))
        {
            return matches;
        }

        var normalizedText = NormalizeForMatching(ingredientsText);

        foreach (var additive in allAdditives)
        {
            var synonyms = DeserializeSynonyms(additive.Synonyms);
            if (synonyms.Any(synonym => normalizedText.Contains(NormalizeForMatching(synonym))))
            {
                matches.Add(additive.Code);
            }
        }

        return matches;
    }

    // Ingredient labels mix Turkish and English (e.g. "İÇİNDEKİLER" alongside "citric acid"). Turkish
    // culture would fold plain ASCII "I" to "ı" (breaking English matches), while invariant culture
    // folds Turkish "İ" to "i" + a combining dot (breaking Turkish matches). Both dotted and dotless
    // capital I are normalized to plain "i" up front so substring matching works either way.
    private static string NormalizeForMatching(string text)
        => text.Replace('İ', 'i').Replace('I', 'i').ToLowerInvariant();

    private static IReadOnlyList<string> DeserializeSynonyms(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return Array.Empty<string>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }
        catch (JsonException)
        {
            return Array.Empty<string>();
        }
    }
}
