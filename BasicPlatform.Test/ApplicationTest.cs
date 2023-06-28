using BasicPlatform.AppService.Applications;
using BasicPlatform.AppService.Applications.Requests;

namespace BasicPlatform.Test;

public class ApplicationTest : TestBase
{
    private IApplicationQueryService? _queryService;
    private IMediator? _mediator;

    [SetUp]
    public void Setup()
    {
        _queryService = GetService<IApplicationQueryService>();
        _mediator = GetService<IMediator>();
    }

    [Test]
    public async Task CreateAsync_GetAsync_Test()
    {
        var id = await _mediator!.SendAsync(new CreateApplicationRequest
        {
            Name = "测试应用",
            ClientId = "web1",
            FrontendUrl = "http://localhost:5000",
            ApiUrl = "http://localhost:5001",
            MenuResourceRoute = "api/menus",
            PermissionResourceRoute = "api/permissions",
            Remarks = "test"
        });
        Assert.That(id, Is.Not.Empty);
        var single = await _queryService!.GetAsync(id);
        Assert.That(single, Is.Not.Null);
    }
}