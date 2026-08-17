using Blacklabel.Api.Controllers;
using Blacklabel.Infrastructure.Persistence;
using Blacklabel.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blacklabel.Tests.Api;

public class AdditivesControllerTests
{
    private static BlacklabelDbContext CreateSeededContext()
    {
        var options = new DbContextOptionsBuilder<BlacklabelDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new BlacklabelDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task GetAll_Returns_Every_Seeded_Additive()
    {
        using var context = CreateSeededContext();
        var controller = new AdditivesController(new AdditiveRepository(context))
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        var result = await controller.GetAll(CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var additives = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
        Assert.True(additives.Count() >= 120);
        Assert.False(string.IsNullOrEmpty(controller.Response.Headers.ETag));
    }

    [Fact]
    public async Task GetAll_Returns_304_When_ETag_Matches()
    {
        using var context = CreateSeededContext();

        var firstController = new AdditivesController(new AdditiveRepository(context))
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };
        await firstController.GetAll(CancellationToken.None);
        var etag = firstController.Response.Headers.ETag.ToString();

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.IfNoneMatch = etag;
        var secondController = new AdditivesController(new AdditiveRepository(context))
        {
            ControllerContext = new ControllerContext { HttpContext = httpContext }
        };

        var result = await secondController.GetAll(CancellationToken.None);

        var statusResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status304NotModified, statusResult.StatusCode);
    }

    [Fact]
    public async Task GetByCode_Returns_Additive_For_Known_Code()
    {
        using var context = CreateSeededContext();
        var controller = new AdditivesController(new AdditiveRepository(context));

        var result = await controller.GetByCode("e330", CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetByCode_Returns_404_For_Unknown_Code()
    {
        using var context = CreateSeededContext();
        var controller = new AdditivesController(new AdditiveRepository(context));

        var result = await controller.GetByCode("E9999", CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }
}
