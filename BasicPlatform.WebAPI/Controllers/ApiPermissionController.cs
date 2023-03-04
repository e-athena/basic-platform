namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// API权限控制器
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class ApiPermissionController : ControllerBase
{
    private readonly IApiPermissionService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public ApiPermissionController(IApiPermissionService service)
    {
        _service = service;
    }

    /// <summary>
    /// 读取菜单资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IList<MenuTreeInfo>> GetMenuResourcesAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var result = _service.GetMenuResources(assembly);
        return await Task.FromResult(result);
    }

    /// <summary>
    /// 读取重名的资源代码
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IList<string>> GetDuplicateResourceCodes()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var result = _service.GetDuplicateResourceCodes(assembly);
        return await Task.FromResult(result);
    }

    /// <summary>
    /// 检查是否有重名的资源代码
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<bool> HasDuplicateResourceCodes()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var result = _service.HasDuplicateResourceCodes(assembly);
        return await Task.FromResult(result);
    }

    /// <summary>
    /// 读取权限资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<ApiPermissionTreeInfo>> GetResourcesAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var result = ApiPermissionHelper.GetResources(assembly);
        return await Task.FromResult(result);
    }

    /// <summary>
    /// 读取权限资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [IgnoreApiResultFilter]
    public async Task<string> GetResourcesForJavaScriptAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var result = ApiPermissionHelper.GetResourcesForJavaScript(assembly);
        return await Task.FromResult(result);
    }
}