using Blacklabel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blacklabel.Tests.Infrastructure;

public class BlacklabelDbContextTests
{
    [Fact]
    public void Model_Builds_Without_Errors()
    {
        var options = new DbContextOptionsBuilder<BlacklabelDbContext>()
            .UseSqlServer("Server=.;Database=BlacklabelModelCheck;TrustServerCertificate=True")
            .Options;

        using var context = new BlacklabelDbContext(options);

        Assert.NotNull(context.Model);
    }
}
