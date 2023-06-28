using BasicPlatform.AppService.Roles;
using BasicPlatform.AppService.Roles.Requests;
using BasicPlatform.Domain.Models.Roles;

namespace BasicPlatform.Test;

public class RoleTest : TestBase
{
    private IRoleQueryService? _queryService;
    private IMediator? _mediator;

    [SetUp]
    public void Setup()
    {
        _queryService = GetService<IRoleQueryService>();
        _mediator = GetService<IMediator>();
    }

    [Test]
    public async Task CreateAsync_GetAsync_Test()
    {
        var id = await _mediator!.SendAsync(new CreateRoleRequest
        {
            Name = "test",
            Remarks = "test"
        });
        Assert.That(id, Is.Not.Empty);
        var role = await _queryService!.GetAsync(id);
        Assert.That(role, Is.Not.Null);

        var res = await _queryService!.GetPagingAsync(new GetRolePagingRequest());
        Assert.That(res, Is.Not.Null);
        await DbContext.Delete<Role>(id).ExecuteAffrowsAsync();
    }
}