using Blacklabel.Application.Dtos;
using Blacklabel.Application.Mapping;
using Blacklabel.Domain.Entities;
using Xunit;

namespace Blacklabel.Tests.Preferences;

public class HouseholdProfileMapperTests
{
    [Fact]
    public void ApplyToEntity_Then_ToResponse_Roundtrips_All_Fields()
    {
        var entity = new HouseholdProfile { Id = Guid.NewGuid(), UserId = Guid.NewGuid() };
        var request = new UpdateHouseholdProfileRequest(
            Name: "Ahmet",
            AvoidedAdditiveCodes: new[] { "E211", "E621" },
            AllergenCodes: new[] { "milk", "gluten" },
            DietFlags: new DietFlagsDto(Vegan: true, Vegetarian: true, GlutenFree: false, LactoseFree: true, NoPalmOil: false, LowSugar: true, LowSalt: false));

        HouseholdProfileMapper.ApplyToEntity(entity, request);
        var response = HouseholdProfileMapper.ToResponse(entity);

        Assert.Equal(request.Name, response.Name);
        Assert.Equal(request.AvoidedAdditiveCodes, response.AvoidedAdditiveCodes);
        Assert.Equal(request.AllergenCodes, response.AllergenCodes);
        Assert.Equal(request.DietFlags, response.DietFlags);
    }

    [Fact]
    public void ToResponse_Returns_Empty_Defaults_For_Blank_Entity()
    {
        var entity = new HouseholdProfile { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "Baba" };

        var response = HouseholdProfileMapper.ToResponse(entity);

        Assert.Empty(response.AvoidedAdditiveCodes);
        Assert.Empty(response.AllergenCodes);
        Assert.Equal(DietFlagsDto.Empty, response.DietFlags);
    }

    [Fact]
    public void ToPreferenceResponse_Extracts_Only_The_Preference_Fields()
    {
        var entity = new HouseholdProfile { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "Ahmet" };
        HouseholdProfileMapper.ApplyToEntity(entity, new UpdateHouseholdProfileRequest(
            "Ahmet", new[] { "E211" }, new[] { "peanuts" }, DietFlagsDto.Empty));

        var preference = HouseholdProfileMapper.ToPreferenceResponse(entity);

        Assert.Equal(new[] { "E211" }, preference.AvoidedAdditiveCodes);
        Assert.Equal(new[] { "peanuts" }, preference.AllergenCodes);
    }
}
