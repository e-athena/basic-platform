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
        var req = new CreateUserRequest
        {
            RealName = "于秀英",
            Email = "6b96@v22v7.net",
            UserName = "test1111",
            PhoneNumber = "17236870392",
            Password = "123"
        };
        // 创建用户
        var id = await _mediator!.SendAsync(req);
        Assert.That(id, Is.Not.Empty);
        // 读取用户
        var user = await _queryService!.GetAsync(id);
        Assert.That(user, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(user.Id, Is.EqualTo(id));
            Assert.That(user.RealName, Is.EqualTo(req.RealName));
            Assert.That(user.Email, Is.EqualTo(req.Email));
            Assert.That(user.UserName, Is.EqualTo(req.UserName));
        });
        // 读取用户分页数据
        var res = await _queryService!.GetAsync(new GetUserPagingRequest
        {
            Keyword = req.UserName
        });
        Assert.That(res, Is.Not.Null);
        // 变更状态
        await _mediator!.SendAsync(new UserStatusChangeRequest {Id = id});
        // 读取用户
        var user1 = await _queryService!.GetAsync(id);
        Assert.That(user1, Is.Not.Null);
        Assert.That(user1.Status, Is.EqualTo(Status.Disabled));

        // 删除用户
        await DbContext.Delete<User>(id).ExecuteAffrowsAsync();
    }
}