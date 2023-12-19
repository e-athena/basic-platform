namespace CMS.WebAPI.Controllers.Systems;

/// <summary>
/// 组织架构控制器
/// </summary>
public class OrganizationController : CustomControllerBase
{
    private readonly IOrganizationService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public OrganizationController(IOrganizationService service)
    {
        _service = service;
    }

    /// <summary>
    /// 读取树形列表
    /// </summary>
    /// <param name="parentId">一级组织ID</param>
    /// <returns></returns>
    [HttpGet]
    [SkipApiPermissionVerification]
    public Task<List<TreeViewModel>> GetTreeListAsync([FromQuery] string? parentId = null)
    {
        return _service.GetTreeListAsync(parentId);
    }

    /// <summary>
    /// 获取级联列表
    /// </summary>
    /// <param name="parentId">一级组织ID</param>
    /// <returns></returns>
    [HttpGet]
    [SkipApiPermissionVerification]
    public Task<List<CascaderViewModel>> GetCascaderListAsync([FromQuery] string? parentId = null)
    {
        return _service.GetCascaderListAsync(parentId);
    }

    /// <summary>
    /// 获取下拉列表
    /// </summary>
    /// <param name="parentId">一级组织ID</param>
    /// <returns></returns>
    [HttpGet]
    [SkipApiPermissionVerification]
    public Task<List<SelectViewModel>> GetSelectListAsync([FromQuery] string? parentId = null)
    {
        return _service.GetSelectListAsync(parentId);
    }
}