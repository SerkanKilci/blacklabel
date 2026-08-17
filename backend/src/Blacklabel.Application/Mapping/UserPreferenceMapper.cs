using System.Text.Json;
using Blacklabel.Application.Dtos;
using Blacklabel.Domain.Entities;

namespace Blacklabel.Application.Mapping;

public static class UserPreferenceMapper
{
    public static UserPreferenceResponse ToResponse(UserPreference entity) => new(
        DeserializeStringList(entity.AvoidedAdditiveCodes),
        DeserializeStringList(entity.AllergenCodes),
        DeserializeDietFlags(entity.DietFlags));

    public static void ApplyToEntity(UserPreference entity, UpdateUserPreferenceRequest request)
    {
        entity.AvoidedAdditiveCodes = JsonSerializer.Serialize(request.AvoidedAdditiveCodes);
        entity.AllergenCodes = JsonSerializer.Serialize(request.AllergenCodes);
        entity.DietFlags = JsonSerializer.Serialize(request.DietFlags);
    }

    public static IReadOnlyList<string> DeserializeStringList(string? json)
        => string.IsNullOrWhiteSpace(json) ? Array.Empty<string>() : JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();

    public static DietFlagsDto DeserializeDietFlags(string? json)
        => string.IsNullOrWhiteSpace(json) ? DietFlagsDto.Empty : JsonSerializer.Deserialize<DietFlagsDto>(json) ?? DietFlagsDto.Empty;
}
