namespace CMS.WebAPI.Controllers.Systems;

/// <summary>
/// 角色控制器
/// </summary>
public class RoleController : CustomControllerBase
{
    private readonly IRoleService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public RoleController(IRoleService service)
    {
        _service = service;
    }

    /// <summary>
    /// 获取下拉列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [SkipApiPermissionVerification]
    public Task<List<SelectViewModel>> GetSelectListAsync([FromQuery] string organizationId)
    {
        return _service.GetSelectListAsync(organizationId);
    }
}