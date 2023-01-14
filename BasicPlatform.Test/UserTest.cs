using BasicPlatform.AppService.Users;
using BasicPlatform.AppService.Users.Requests;

namespace BasicPlatform.Test;

public class UserTest : TestBase
{
    private IUserQueryService? _queryService;
    private IMediator? _mediator;

    [SetUp]
    public void Setup()
    {
        _queryService = GetService<IUserQueryService>();
        _mediator = GetService<IMediator>();
    }

    [Test]
    public async Task CreateAsync_GetAsync_Test()
    {
        var id = await _mediator!.SendAsync(new CreateUserRequest
        {
            RealName = "于秀英",
            Email = "6b96@v22v7.net",
            UserName = "test1111",
            PhoneNumber = "17236870392",
            Password = "123"
        });
        Assert.That(id, Is.Not.Empty);
        var user = await _queryService!.GetAsync(id);
        Assert.That(user, Is.Not.Null);

        var res = await _queryService!.GetAsync(new GetUserPagingRequest());
        Assert.That(res, Is.Not.Null);
        await DbContext.Delete<User>(id).ExecuteAffrowsAsync();
    }
}