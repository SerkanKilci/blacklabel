using Blacklabel.Application.Dtos;
using Blacklabel.Application.Mapping;
using Blacklabel.Domain.Entities;
using Xunit;

namespace Blacklabel.Tests.Preferences;

public class UserPreferenceMapperTests
{
    [Fact]
    public void ApplyToEntity_Then_ToResponse_Roundtrips_All_Fields()
    {
        var entity = new UserPreference { UserId = Guid.NewGuid() };
        var request = new UpdateUserPreferenceRequest(
            AvoidedAdditiveCodes: new[] { "E211", "E621" },
            AllergenCodes: new[] { "milk", "gluten" },
            DietFlags: new DietFlagsDto(Vegan: true, Vegetarian: true, GlutenFree: false, LactoseFree: true, NoPalmOil: false, LowSugar: true, LowSalt: false));

        UserPreferenceMapper.ApplyToEntity(entity, request);
        var response = UserPreferenceMapper.ToResponse(entity);

        Assert.Equal(request.AvoidedAdditiveCodes, response.AvoidedAdditiveCodes);
        Assert.Equal(request.AllergenCodes, response.AllergenCodes);
        Assert.Equal(request.DietFlags, response.DietFlags);
    }

    [Fact]
    public void ToResponse_Returns_Empty_Defaults_For_Blank_Entity()
    {
        var entity = new UserPreference { UserId = Guid.NewGuid() };

        var response = UserPreferenceMapper.ToResponse(entity);

        Assert.Empty(response.AvoidedAdditiveCodes);
        Assert.Empty(response.AllergenCodes);
        Assert.Equal(DietFlagsDto.Empty, response.DietFlags);
    }
}
