using Blacklabel.Infrastructure.Persistence.Seed;
using Xunit;

namespace Blacklabel.Tests.Infrastructure;

public class AdditiveSeedDataTests
{
    [Fact]
    public void Seed_Contains_At_Least_120_Additives()
    {
        var additives = AdditiveSeedData.Get();

        Assert.True(additives.Count >= 120, $"Expected at least 120 seeded additives, found {additives.Count}.");
    }

    [Fact]
    public void Seed_Has_No_Duplicate_Codes()
    {
        var additives = AdditiveSeedData.Get();

        var distinctCodes = additives.Select(a => a.Code).Distinct().Count();

        Assert.Equal(additives.Count, distinctCodes);
    }

    [Fact]
    public void Seed_Entries_Have_Required_Fields_Populated()
    {
        var additives = AdditiveSeedData.Get();

        Assert.All(additives, additive =>
        {
            Assert.False(string.IsNullOrWhiteSpace(additive.Code));
            Assert.False(string.IsNullOrWhiteSpace(additive.NameTr));
            Assert.False(string.IsNullOrWhiteSpace(additive.NameEn));
            Assert.False(string.IsNullOrWhiteSpace(additive.DescriptionTr));
            Assert.False(string.IsNullOrWhiteSpace(additive.DescriptionEn));
            Assert.InRange(additive.RiskLevel, 0, 3);
        });
    }
}
