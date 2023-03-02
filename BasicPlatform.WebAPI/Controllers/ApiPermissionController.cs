using Athena.Infrastructure.ApiPermission.Helpers;
using Athena.Infrastructure.Mvc.Attributes;

namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// API权限控制器
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class ApiPermissionController : ControllerBase
{
    /// <summary>
    /// 读取菜单资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<MenuTreeInfo>> GetMenuResourcesAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var result = MenuHelper.GetResources(assembly);
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