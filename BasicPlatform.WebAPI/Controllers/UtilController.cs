using Athena.Infrastructure.EventTracking;
using Athena.Infrastructure.EventTracking.Enums;
using Athena.Infrastructure.EventTracking.Messaging.Models;
using Athena.Infrastructure.EventTracking.Messaging.Requests;
using Athena.Infrastructure.EventTracking.Messaging.Responses;
using Athena.Infrastructure.EventTracking.Models;
using BasicPlatform.AppService.FreeSql.Users;
using BasicPlatform.AppService.Tenants.Requests;
using BasicPlatform.AppService.Users;
using BasicPlatform.Domain.Models.Users.Events;
using BasicPlatform.ProcessManager;
using BasicPlatform.WebAPI.Services;

namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 工具控制器
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class UtilController : ControllerBase
{
    private readonly ILogger<UtilController> _logger;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerFactory"></param>
    /// <param name="securityContextAccessor"></param>
    public UtilController(ILoggerFactory loggerFactory, ISecurityContextAccessor securityContextAccessor)
    {
        _securityContextAccessor = securityContextAccessor;
        _logger = loggerFactory.CreateLogger<UtilController>();
    }

    /// <summary>
    /// 读取租户信息
    /// </summary>
    /// <param name="service"></param>
    /// <param name="tenantCode"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    [HttpGet]
    [BasicAuthFilter]
    public Task<TenantInfo> GetTenantInfoAsync(
        [FromServices] ITenantQueryService service,
        [FromQuery] string tenantCode,
        [FromQuery] string appId
    )
    {
        return service.GetAsync(tenantCode, appId);
    }

    /// <summary>
    /// 读取菜单资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IList<MenuTreeInfo>> GetMenuResourcesAsync([FromServices] IApiPermissionService service)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var result = service.GetFrontEndRoutingResources(assembly, GlobalConstant.DefaultAppId);
        return await Task.FromResult(result);
    }

    /// <summary>
    /// 读取应用数据权限资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IList<ApplicationDataPermissionInfo>> GetApplicationDataPermissionResourcesAsync(
        [FromServices] ISubApplicationService subApplicationService
    )
    {
        var result = new List<ApplicationDataPermissionInfo>();
        // var assembly = Assembly.Load("BasicPlatform.AppService");
        // var defaultList = DataPermissionHelper.GetGroupList(assembly, GlobalConstant.DefaultAppId);
        var defaultList = DataPermissionHelper.GetGroupList(GlobalConstant.DefaultAppId);
        result.Add(new ApplicationDataPermissionInfo
        {
            ApplicationId = GlobalConstant.DefaultAppId,
            ApplicationName = "系统应用",
            DataPermissionGroups = defaultList
        });

        // 其他业务应用
        var subApplicationResources = await subApplicationService.GetDataPermissionResourcesAsync();
        result.AddRange(subApplicationResources);
        return result;
    }

    /// <summary>
    /// 读取应用菜单资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<IList<ApplicationResourceInfo>> GetApplicationMenuResourcesAsync(
        [FromServices] IUserQueryService userQueryService,
        [FromServices] IApiPermissionService service,
        [FromServices] ISubApplicationService subApplicationService,
        [FromServices] Microsoft.AspNetCore.Mvc.Infrastructure.IActionDescriptorCollectionProvider provider
    )
    {
        var result = new List<ApplicationResourceInfo>();

        #region 系统资源

        var assembly = Assembly.GetExecutingAssembly();
        IList<MenuTreeInfo> defaultList;
        IList<ResourceModel>? resources = null;
        if (_securityContextAccessor.IsRoot)
        {
            defaultList = service.GetFrontEndRoutingResources(assembly, GlobalConstant.DefaultAppId);
        }
        else
        {
            resources = await userQueryService.GetUserResourceAsync(null, null);
            var keys = resources
                .Where(p => p.ApplicationId == GlobalConstant.DefaultAppId || string.IsNullOrEmpty(p.ApplicationId))
                .Select(p => p.Key)
                .ToList();
            defaultList = service.GetPermissionFrontEndRoutingResources(assembly, keys, GlobalConstant.DefaultAppId);
        }

        result.Add(new ApplicationResourceInfo
        {
            ApplicationId = GlobalConstant.DefaultAppId,
            ApplicationName = "系统应用",
            Resources = defaultList
        });

        #endregion

        // 其他业务应用
        var subApplicationResources = await subApplicationService.GetMenuResourcesAsync(resources);
        result.AddRange(subApplicationResources);
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

        // 有配置前端地址的应用
        var list = appList
            .Where(p => !string.IsNullOrEmpty(p.FrontendUrl))
            .ToList();

        // 可正常访问的应用
        var healthyList = new List<string>();

        await Parallel.ForEachAsync(list, async (app, index) =>
        {
            try
            {
                var res = await app.FrontendUrl.GetAsync(cancellationToken: index);
                if (res.StatusCode == 200)
                {
                    healthyList.Add(app.ClientId);
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "应用前端地址不可访问，应用ID:{ClientId},前端地址:{Url}", app.ClientId, app.FrontendUrl);
            }
        });

        return new
        {
            Apps = appList
                .Where(p => healthyList.Contains(p.ClientId))
                .Select(p => new
                {
                    Name = p.ClientId,
                    Entry = p.FrontendUrl,
                    Credentials = true
                }).ToList<dynamic>(),
            Routes = appList
                .Where(p => healthyList.Contains(p.ClientId))
                .Select(p => new
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

    /// <summary>
    /// 检查授权
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [IgnoreApiResultFilter]
    [Authorize]
    public IActionResult CheckAuthAsync()
    {
        return Content("ok");
    }

    /// <summary>
    /// 根据目录读取所有目录及文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    [HttpGet]
    [IgnoreApiResultFilter]
    public Task<List<DirectoryOrFile>> GetDirectoryInfoAsync(string path)
    {
        var dir = new DirectoryInfo(path);
        return Task.FromResult(dir.GetDirectories().Select(GetDirectoryInfo).ToList());
    }

    private static readonly string[] IgnoreDirs =
    {
        ".git", "bin", "obj", "node_modules", "packages", "dist", "build", "logs", ".umi", ".umi-production", ".vscode",
        ".idea", ".vs"
    };

    // 递归读取目录及文件
    private static DirectoryOrFile GetDirectoryInfo(DirectoryInfo dir)
    {
        var result = new DirectoryOrFile
        {
            Name = dir.Name,
            FullName = dir.FullName,
            IsDirectory = true
        };

        if (IgnoreDirs.Contains(dir.Name))
        {
            return result;
        }

        foreach (var directory in dir.GetDirectories())
        {
            var dir1 = GetDirectoryInfo(directory);
            if (IgnoreDirs.Contains(dir1.Name))
            {
                continue;
            }

            result.Directories.Add(dir1);
        }

        foreach (var file in dir.GetFiles())
        {
            result.Directories.Add(new DirectoryOrFile
            {
                Name = file.Name,
                FullName = file.FullName,
                IsDirectory = false
            });
        }

        return result;
    }
}

/// <summary>
/// 
/// </summary>
public class DirectoryOrFile
{
    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsDirectory { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<DirectoryOrFile> Directories { get; set; } = new();
}