using BasicPlatform.AppService.Applications;
using BasicPlatform.AppService.Resources.Models;
using BasicPlatform.AppService.Resources.Requests;

namespace BasicPlatform.WebAPI.Controllers.Developer;

/// <summary>
/// 资源管理
/// </summary>
[FrontEndRouting("资源管理",
    ModuleCode = "developer",
    ModuleName = "开发者中心",
    ModuleIcon = "ControlOutlined",
    ModuleRoutePath = "/developer",
    ModuleSort = 0,
    RoutePath = "/developer/resource",
    Sort = 1,
    Description = "资源包含菜单树以及功能，由系统生成，用于控制系统菜单的展示及功能权限。"
)]
public class ResourceController : CustomControllerBase
{
    private readonly IMediator _mediator;
    private readonly IApiPermissionService _apiPermissionService;
    private readonly ILogger<ResourceController> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="apiPermissionService"></param>
    /// <param name="loggerFactory"></param>
    public ResourceController(IMediator mediator, IApiPermissionService apiPermissionService,
        ILoggerFactory loggerFactory)
    {
        _mediator = mediator;
        _apiPermissionService = apiPermissionService;
        _logger = loggerFactory.CreateLogger<ResourceController>();
    }

    /// <summary>
    /// 同步
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(DisplayName = "同步资源", Description = "该操作会增量添加资源数据，存在且已分配的数据不会被删除，不存在且分配的的数据将会被删除")]
    public async Task<int> SyncAsync(
        [FromServices] IApplicationQueryService applicationQueryService,
        CancellationToken cancellationToken
    )
    {
        var count = 0;

        #region 系统应用资源

        var assembly = Assembly.GetExecutingAssembly();
        var resources = _apiPermissionService.GetResourceCodeInfos(assembly, GlobalConstant.DefaultAppId);
        var request = new SyncResourceRequest
        {
            ApplicationId = GlobalConstant.DefaultAppId,
            Resources = resources.Select(p => new ResourceModel
            {
                ApplicationId = p.ApplicationId,
                Key = p.Key,
                Code = p.Code
            }).ToList()
        };
        count += await _mediator.SendAsync(request, cancellationToken);

        #endregion

        // 其他应用资源
        var apps = await applicationQueryService.GetListAsync();
        foreach (var app in apps.Where(p => !string.IsNullOrEmpty(p.MenuResourceRoute)))
        {
            var resourceUrl = $"{app.ApiUrl}{app.MenuResourceRoute}";
            try
            {
                var res = await resourceUrl.GetAsync(cancellationToken: cancellationToken)
                    .ReceiveJson<ApiResult<List<MenuTreeInfo>>>();

                if (res.Data != null && res.Success && res.Data.Count > 0)
                {
                    var appResources = new List<ResourceModel>();

                    foreach (var info in res.Data.Where(module => module.Children != null)
                                 .SelectMany(module => module.Children!.Where(info => info.Functions != null)))
                    {
                        appResources.Add(new ResourceModel
                        {
                            ApplicationId = app.ClientId,
                            Key = info.Code,
                            Code = info.Code
                        });
                        appResources.AddRange(info.Functions!.Select(p => new ResourceModel
                        {
                            ApplicationId = app.ClientId,
                            Key = p.Key,
                            Code = p.Value
                        }));
                    }

                    var req = new SyncResourceRequest
                    {
                        ApplicationId = app.ClientId,
                        Resources = appResources
                    };
                    count += await _mediator.SendAsync(req, cancellationToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "加载应用资源失败，应用ID:{ClientId},资源地址:{Url}", app.ClientId, resourceUrl);
            }
        }

        return count;
    }

    /// <summary>
    /// 读取子应用资源
    /// </summary>
    /// <param name="resourceUrl"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<List<MenuTreeInfo>> GetSubAppResourcesAsync(string resourceUrl)
    {
        try
        {
            var res = await resourceUrl.GetAsync()
                .ReceiveJson<ApiResult<List<MenuTreeInfo>>>();
            return res.Data ?? new List<MenuTreeInfo>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "读取资源错误，资源地址:{Url}", resourceUrl);
            throw FriendlyException.Of("读取资源错误");
        }
    }
}