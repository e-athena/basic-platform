using BasicPlatform.AppService.Organizations;
using BasicPlatform.AppService.Organizations.Requests;

namespace BasicPlatform.Test;

public class OrganizationTest : TestBase
{
    private IOrganizationQueryService? _queryService;
    private IMediator? _mediator;

    [SetUp]
    public void Setup()
    {
        _queryService = GetService<IOrganizationQueryService>();
        _mediator = GetService<IMediator>();
    }

    [Test]
    public async Task CreateAsync_GetAsync_Test()
    {
        var req = new CreateOrganizationRequest()
        {
            Name = "测试组织架构",
            Remarks = "备注",
            Status = Status.Enabled
        };
        // 创建
        var id = await _mediator!.SendAsync(req);
        Assert.That(id, Is.Not.Empty);
        // 读取
        var organization = await _queryService!.GetAsync(id);
        Assert.That(organization, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(organization?.Id, Is.EqualTo(id));
            Assert.That(organization?.Name, Is.EqualTo(req.Name));
            Assert.That(organization?.Remarks, Is.EqualTo(req.Remarks));
        });
        // 读取分页数据
        var res = await _queryService!.GetPagingAsync(new GetOrganizationPagingRequest
        {
            Keyword = req.Name
        });
        Assert.That(res, Is.Not.Null);
        // 变更状态
        await _mediator!.SendAsync(new OrganizationStatusChangeRequest {Id = id});
        // 读取
        var organization1 = await _queryService!.GetAsync(id);
        Assert.That(organization1, Is.Not.Null);
        Assert.That(organization1?.Status, Is.EqualTo(Status.Disabled));

        // 删除
        await DbContext.Delete<Organization>(id).ExecuteAffrowsAsync();
    }
}