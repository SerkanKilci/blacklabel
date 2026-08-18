using System.Text.Json;
using Blacklabel.Application.Dtos;
using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Mapping;

public static class HouseholdProfileMapper
{
    public static HouseholdProfileResponse ToResponse(HouseholdProfile entity) => new(
        entity.Id,
        entity.Name,
        DeserializeStringList(entity.AvoidedAdditiveCodes),
        DeserializeStringList(entity.AllergenCodes),
        DeserializeDietFlags(entity.DietFlags));

    /// <summary>Just the preference fields, for feeding <see cref="Preferences.PersonalWarningCalculator"/>.</summary>
    public static UserPreferenceResponse ToPreferenceResponse(HouseholdProfile entity) => new(
        DeserializeStringList(entity.AvoidedAdditiveCodes),
        DeserializeStringList(entity.AllergenCodes),
        DeserializeDietFlags(entity.DietFlags));

    public static void ApplyToEntity(HouseholdProfile entity, UpdateHouseholdProfileRequest request)
    {
        entity.Name = request.Name;
        entity.AvoidedAdditiveCodes = JsonSerializer.Serialize(request.AvoidedAdditiveCodes);
        entity.AllergenCodes = JsonSerializer.Serialize(request.AllergenCodes);
        entity.DietFlags = JsonSerializer.Serialize(request.DietFlags);
    }

    private static IReadOnlyList<string> DeserializeStringList(string? json)
        => string.IsNullOrWhiteSpace(json) ? Array.Empty<string>() : JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();

    private static DietFlagsDto DeserializeDietFlags(string? json)
        => string.IsNullOrWhiteSpace(json) ? DietFlagsDto.Empty : JsonSerializer.Deserialize<DietFlagsDto>(json) ?? DietFlagsDto.Empty;
}
