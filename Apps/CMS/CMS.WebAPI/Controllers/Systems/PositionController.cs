namespace CMS.WebAPI.Controllers.Systems;

/// <summary>
/// 职位控制器
/// </summary>
public class PositionController : CustomControllerBase
{
    private readonly IPositionService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public PositionController(IPositionService service)
    {
        _service = service;
    }

    /// <summary>
    /// 获取下拉列表
    /// </summary>
    /// <param name="organizationId">组织ID</param>
    /// <returns></returns>
    [HttpGet]
    [SkipApiPermissionVerification]
    public Task<List<SelectViewModel>> GetSelectListAsync([FromQuery] string? organizationId = null)
    {
        return _service.GetSelectListAsync(organizationId);
    }
}