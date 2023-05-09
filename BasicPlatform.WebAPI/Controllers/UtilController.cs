using BasicPlatform.AppService.Applications;

namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 工具控制器
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class UtilController : ControllerBase
{
    private readonly ILogger<UtilController> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerFactory"></param>
    public UtilController(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<UtilController>();
    }

    /// <summary>
    /// 读取菜单资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IList<MenuTreeInfo>> GetMenuResourcesAsync([FromServices] IApiPermissionService service)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var result = service.GetMenuResources(assembly, GlobalConstant.DefaultAppId);
        return await Task.FromResult(result);
    }

    /// <summary>
    /// 读取应用数据权限资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IList<ApplicationDataPermissionInfo>> GetApplicationDataPermissionResourcesAsync(
        [FromServices] IApplicationQueryService applicationQueryService
    )
    {
        var result = new List<ApplicationDataPermissionInfo>();
        var assembly = Assembly.Load("BasicPlatform.AppService");
        var defaultList = DataPermissionHelper.GetGroupList(assembly, GlobalConstant.DefaultAppId);
        result.Add(new ApplicationDataPermissionInfo
        {
            ApplicationId = GlobalConstant.DefaultAppId,
            ApplicationName = "系统应用",
            DataPermissionGroups = defaultList
        });

        // 读取应用信息
        var apps = await applicationQueryService.GetListAsync();

        foreach (var app in apps.Where(p => !string.IsNullOrEmpty(p.PermissionResourceRoute)))
        {
            var resourceUrl = $"{app.ApiUrl}{app.PermissionResourceRoute}";
            try
            {
                var res = await resourceUrl.GetAsync()
                    .ReceiveJson<ApiResult<List<DataPermissionGroup>>>();

                if (res.Data != null && res.Success && res.Data.Count > 0)
                {
                    result.Add(new ApplicationDataPermissionInfo
                    {
                        ApplicationId = app.ClientId,
                        ApplicationName = app.Name,
                        DataPermissionGroups = res.Data
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "加载应用数据权限失败，应用ID:{ClientId},数据权限地址:{Url}", app.ClientId, resourceUrl);
            }
        }

        return result;
    }

    /// <summary>
    /// 读取应用菜单资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IList<ApplicationResourceInfo>> GetApplicationMenuResourcesAsync(
        [FromServices] IApiPermissionService service,
        [FromServices] IApplicationQueryService applicationQueryService
    )
    {
        var result = new List<ApplicationResourceInfo>();

        #region 系统资源

        var assembly = Assembly.GetExecutingAssembly();
        var defaultList = service.GetMenuResources(assembly, GlobalConstant.DefaultAppId);
        result.Add(new ApplicationResourceInfo
        {
            ApplicationId = GlobalConstant.DefaultAppId,
            ApplicationName = "系统应用",
            Resources = defaultList
        });

        #endregion

        // 其他业务应用
        var apps = await applicationQueryService.GetListAsync();

        foreach (var app in apps.Where(p => !string.IsNullOrEmpty(p.MenuResourceRoute)))
        {
            var resourceUrl = $"{app.ApiUrl}{app.MenuResourceRoute}";
            try
            {
                var res = await resourceUrl.GetAsync()
                    .ReceiveJson<ApiResult<List<MenuTreeInfo>>>();

                if (res.Data != null && res.Success && res.Data.Count > 0)
                {
                    result.Add(new ApplicationResourceInfo
                    {
                        ApplicationId = app.ClientId,
                        ApplicationName = app.Name,
                        Resources = res.Data
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "加载应用资源失败，应用ID:{ClientId},资源地址:{Url}", app.ClientId, resourceUrl);
            }
        }

        return result;
    }

    /// <summary>
    /// 读取应用列表
    /// </summary>
    /// <param name="applicationQueryService"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<dynamic>> GetAppListAsync(
        [FromServices] IApplicationQueryService applicationQueryService)
    {
        var apps = await applicationQueryService.GetListAsync();
        return apps.Where(p => !string.IsNullOrEmpty(p.FrontendUrl)).Select(p => new
        {
            Name = p.ClientId,
            Entry = p.FrontendUrl,
            Credentials = true
        }).ToList<dynamic>();
    }

    /// <summary>
    /// 读取应用配置
    /// </summary>
    /// <param name="applicationQueryService"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<dynamic> GetAppConfigAsync(
        [FromServices] IApplicationQueryService applicationQueryService)
    {
        var appList = await applicationQueryService.GetListAsync();
        return new
        {
            Apps = appList.Where(p => !string.IsNullOrEmpty(p.FrontendUrl)).Select(p => new
            {
                Name = p.ClientId,
                Entry = p.FrontendUrl,
                Credentials = true
            }).ToList<dynamic>(),
            Routes = appList.Where(p => !string.IsNullOrEmpty(p.FrontendUrl)).Select(p => new
            {
                Path = $"/app/{p.ClientId}/*",
                MicroApp = p.ClientId,
                MicroAppProps = new
                {
                    AutoCaptureError = true,
                    ClassName = "micro-app",
                }
            }).ToList<dynamic>(),
        };
    }

    /// <summary>
    /// 同步数据库结构
    /// </summary>
    /// <param name="freeSql"></param>
    /// <param name="accessor"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public IActionResult SyncStructure(
        [FromServices] IFreeSql freeSql,
        [FromServices] ISecurityContextAccessor accessor)
    {
        if (!accessor.IsRoot)
        {
            throw FriendlyException.Of("只有超级管理员才能执行此操作");
        }

        freeSql.SyncStructure("BasicPlatform.Domain");
        return Ok("ok");
    }

    /// <summary>
    /// 读取程序集信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetAssemblyInfo()
    {
        // 读取所有程序集
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        // 过滤掉动态程序集
        assemblies = assemblies.Where(x => !x.IsDynamic).ToArray();
        // 只读取直接引用的程序集
        assemblies = assemblies.Where(x => x.Location.Contains(AppDomain.CurrentDomain.BaseDirectory)).ToArray();
        // 按名称排序
        assemblies = assemblies.OrderBy(x => x.FullName).ToArray();
        // 过滤掉重复的
        assemblies = assemblies.Distinct().ToArray();
        // 循环读取程序集信息
        var list = new List<object>();
        foreach (var assembly in assemblies)
        {
            var version = assembly.GetName().Version;
            var fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            var info = new
            {
                assembly.FullName,
                version,
                fileVersion.FileVersion,
                fileVersion.ProductVersion,
                fileVersion.ProductName,
                fileVersion.CompanyName,
                fileVersion.FileDescription,
                fileVersion.Comments,
                fileVersion.OriginalFilename,
                fileVersion.Language,
            };
            list.Add(info);
        }

        return Ok(list);
    }


    /// <summary>
    /// 释放内存
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult MemoryFree()
    {
        try
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "释放内存失败");
        }

        return Ok("ok");
    }
}