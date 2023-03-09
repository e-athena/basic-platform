using System.Text;
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

    [Test]
    public void Test()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var ipList = new List<string>
        {
            "58.44.143.160",
            "183.9.242.9",
            "119.130.230.57",
            "58.211.24.210",
            "113.109.243.219",
            "113.65.229.252",
            "127.0.0.1",
            "192.168.21.56",
            "9.219.106.180"
        };
        var dict = new Dictionary<string, string>();
        foreach (var ip in ipList)
        {
            var address = NewLife.IP.Ip.GetAddress(ip);
            if (address != null)
            {
                dict.Add(ip, address);
            }
        }

        Assert.That(dict.Count, Is.EqualTo(ipList.Count));
    }

    [Test]
    public void Test1()
    {
        var a = bool.TryParse("TRUE", out var b);
        Assert.Multiple(() =>
        {
            Assert.That(a, Is.True);
            Assert.That(b, Is.True);
        });
        var c = bool.TryParse("FALSE", out var d);
        Assert.Multiple(() =>
        {
            Assert.That(c, Is.True);
            Assert.That(d, Is.False);
        });

        var e = bool.TryParse("true", out var f);
        Assert.Multiple(() =>
        {
            Assert.That(e, Is.True);
            Assert.That(f, Is.True);
        });

        var g = bool.TryParse("false", out var h);
        Assert.Multiple(() =>
        {
            Assert.That(g, Is.True);
            Assert.That(h, Is.False);
        });
    }
}