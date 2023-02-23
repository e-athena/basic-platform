using BasicPlatform.AppService.Positions;
using BasicPlatform.AppService.Positions.Requests;

namespace BasicPlatform.Test;

public class PositionTest : TestBase
{
  private IPositionQueryService? _queryService;
  private IMediator? _mediator;

  [SetUp]
  public void Setup()
  {
    _queryService = GetService<IPositionQueryService>();
    _mediator = GetService<IMediator>();
  }

  [Test]
  public async Task CreateAsync_GetAsync_Test()
  {
    var req = new CreatePositionRequest
    {
      Name = "测试职位",
      Remarks = "备注",
      Status = Status.Enabled
    };
    // 创建
    var id = await _mediator!.SendAsync(req);
    Assert.That(id, Is.Not.Empty);
    // 读取
    var record = await _queryService!.GetAsync(id);
    Assert.That(record, Is.Not.Null);
    Assert.Multiple(() =>
    {
      Assert.That(record.Id, Is.EqualTo(id));
      Assert.That(record.Name, Is.EqualTo(req.Name));
      Assert.That(record.Remarks, Is.EqualTo(req.Remarks));
    });
    // 读取分页数据
    var res = await _queryService!.GetPagingAsync(new GetPositionPagingRequest
    {
      Keyword = req.Name
    });
    Assert.That(res, Is.Not.Null);
    // 变更状态
    await _mediator!.SendAsync(new PositionStatusChangeRequest { Id = id });
    // 读取
    var record1 = await _queryService!.GetAsync(id);
    Assert.That(record1, Is.Not.Null);
    Assert.That(record1.Status, Is.EqualTo(Status.Disabled));

    // 删除
    await DbContext.Delete<Organization>(id).ExecuteAffrowsAsync();
  }
}