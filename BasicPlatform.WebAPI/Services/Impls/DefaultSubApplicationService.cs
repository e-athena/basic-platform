namespace BasicPlatform.WebAPI.Services.Impls;

/// <summary>
/// 子应用服务接口默认实现类
/// </summary>
[Component]
public class DefaultSubApplicationService : DefaultServiceBase, ISubApplicationService
{
    private readonly ILogger<DefaultSubApplicationService> _logger;
    private readonly IApiPermissionService _apiPermissionService;
    private readonly IApplicationQueryService _applicationQueryService;
    private readonly ITenantQueryService _tenantQueryService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <param name="apiPermissionService"></param>
    /// <param name="loggerFactory"></param>
    /// <param name="applicationQueryService"></param>
    /// <param name="tenantQueryService"></param>
    public DefaultSubApplicationService(
        IOptionsMonitor<BasicAuthConfig> options,
        IApiPermissionService apiPermissionService,
        ILoggerFactory loggerFactory,
        IApplicationQueryService applicationQueryService,
        ITenantQueryService tenantQueryService
    ) : base(options)
    {
        _apiPermissionService = apiPermissionService;
        _applicationQueryService = applicationQueryService;
        _tenantQueryService = tenantQueryService;
        _logger = loggerFactory.CreateLogger<DefaultSubApplicationService>();
    }

    /// <summary>
    /// 读取事件列表
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<IDictionary<string, List<SelectViewModel>>> GetEventsAsync()
    {
        var appList = await _applicationQueryService.GetListAsync();
        if (appList.Count == 0)
        {
            return new Dictionary<string, List<SelectViewModel>>();
        }

        const string resourceUrl = "/api/external/get-events";
        var dict = new Dictionary<string, List<SelectViewModel>>();
        await Parallel.ForEachAsync(appList, async (app, cancellationToken) =>
        {
            var url = $"{app.ApiUrl}{resourceUrl}";
            try
            {
                var res = await GetRequest(url)
                    .GetAsync(cancellationToken)
                    .ReceiveJson<ApiResult<List<SelectViewModel>>>();

                if (res.Data != null && res.Success && res.Data.Count > 0)
                {
                    dict.Add(app.Name, res.Data);
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e,
                    "读取应用事件失败，应用ID:{ClientId},资源地址:{Url}",
                    app.ClientId,
                    resourceUrl
                );
            }
        });
        return dict;
    }

    /// <summary>
    /// 读取事件追踪配置列表
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<IDictionary<string, List<EventTrackingInfo>>> GetEventTrackingListAsync()
    {
        var appList = await _applicationQueryService.GetListAsync();
        if (appList.Count == 0)
        {
            return new Dictionary<string, List<EventTrackingInfo>>();
        }

        const string resourceUrl = "/api/external/get-event-tracking-list";
        var dict = new Dictionary<string, List<EventTrackingInfo>>();
        await Parallel.ForEachAsync(appList, async (app, cancellationToken) =>
        {
            var url = $"{app.ApiUrl}{resourceUrl}";
            try
            {
                var res = await GetRequest(url)
                    .GetAsync(cancellationToken)
                    .ReceiveJson<ApiResult<List<EventTrackingInfo>>>();

                if (res.Data != null && res.Success && res.Data.Count > 0)
                {
                    dict.Add(app.Name, res.Data);
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e,
                    "读取应用事件追踪配置失败，应用ID:{ClientId},资源地址:{Url}",
                    app.ClientId,
                    resourceUrl
                );
            }
        });
        return dict;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="resourceUrl"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<MenuTreeInfo>> GetMenuResourcesAsync(string resourceUrl)
    {
        try
        {
            var res = await GetRequest(resourceUrl)
                .GetAsync()
                .ReceiveJson<ApiResult<List<MenuTreeInfo>>>();
            return res.Data ?? new List<MenuTreeInfo>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "读取资源错误，资源地址:{Url}", resourceUrl);
            throw FriendlyException.Of("读取资源错误");
        }
    }

    /// <summary>
    /// 读取菜单资源
    /// </summary>
    /// <param name="resources"></param>
    /// <returns></returns>
    public async Task<List<ApplicationResourceInfo>> GetMenuResourcesAsync(
        IList<ResourceModel>? resources
    )
    {
        var appList = await _applicationQueryService.GetListAsync();
        if (appList.Count == 0)
        {
            return new List<ApplicationResourceInfo>();
        }

        const string resourceUrl = "/api/external/get-menu-resources";
        var result = new List<ApplicationResourceInfo>();
        await Parallel.ForEachAsync(appList, async (app, cancellationToken) =>
        {
            var url = $"{app.ApiUrl}{app.MenuResourceRoute ?? resourceUrl}";
            try
            {
                var res = await GetRequest(url)
                    .GetAsync(cancellationToken)
                    .ReceiveJson<ApiResult<List<MenuTreeInfo>>>();

                if (res.Data != null && res.Success && res.Data.Count > 0)
                {
                    IList<MenuTreeInfo> resourceList;
                    if (resources == null)
                    {
                        resourceList = res.Data;
                    }
                    else
                    {
                        var keys = resources
                            .Where(p => p.AppId == app.ClientId)
                            .Select(p => p.Key)
                            .ToList();
                        resourceList = _apiPermissionService
                            .GetPermissionFrontEndRoutingResources(res.Data, keys, app.ClientId)
                            .Where(p => p.Children != null && p.Children.Any())
                            .ToList();
                    }

                    result.Add(new ApplicationResourceInfo
                    {
                        ApplicationId = app.ClientId,
                        ApplicationName = app.Name,
                        Resources = resourceList
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e,
                    "加载应用资源失败，应用ID:{ClientId},资源地址:{Url}",
                    app.ClientId,
                    app.MenuResourceRoute ?? resourceUrl
                );
            }
        });
        return result;
    }

    /// <summary>
    /// 读取数据权限资源
    /// </summary>
    /// <returns></returns>
    public async Task<List<ApplicationDataPermissionInfo>> GetDataPermissionResourcesAsync()
    {
        var appList = await _applicationQueryService.GetListAsync();
        if (appList.Count == 0)
        {
            return new List<ApplicationDataPermissionInfo>();
        }

        const string resourceUrl = "/api/external/get-data-permission-resources";
        var result = new List<ApplicationDataPermissionInfo>();
        await Parallel.ForEachAsync(appList, async (app, cancellationToken) =>
        {
            var url = $"{app.ApiUrl}{app.PermissionResourceRoute ?? resourceUrl}";
            try
            {
                var res = await GetRequest(url)
                    .GetAsync(cancellationToken)
                    .ReceiveJson<ApiResult<ApplicationDataPermissionInfo>>();

                if (res.Data != null && res.Success)
                {
                    var d = res.Data;
                    result.Add(new ApplicationDataPermissionInfo
                    {
                        ApplicationId = app.ClientId,
                        ApplicationName = app.Name,
                        DataPermissionGroups = d.DataPermissionGroups,
                        ExtraSelectList = d.ExtraSelectList
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(
                    e,
                    "加载应用数据权限失败，应用ID:{ClientId},数据权限地址:{Url}",
                    app.ClientId,
                    app.PermissionResourceRoute ?? resourceUrl
                );
            }
        });
        return result;
    }

    /// <summary>
    /// 同步数据库
    /// </summary>
    /// <param name="tenantCode"></param>
    /// <returns></returns>
    public async Task SyncDatabaseAsync(string tenantCode)
    {
        const string resourceUrl = "/api/external/sync-database";
        var response = await _tenantQueryService.GetByCodeAsync(tenantCode);
        var list = response
            .Applications
            .Where(p => !string.IsNullOrEmpty(p.ApplicationApiUrl));
        await Parallel.ForEachAsync(
            list,
            async (app, cancellationToken) =>
            {
                var url = $"{app.ApplicationApiUrl}{resourceUrl}";
                try
                {
                    var res = await GetRequest(url)
                        .WithHeader("TenantId", tenantCode)
                        .WithHeader("AppId", app.AppId)
                        .GetAsync(cancellationToken)
                        .ReceiveJson<ApiResult<string>>();
                    if (res.Data == "ok")
                    {
                        _logger.LogInformation("同步数据库成功，应用名称:{ApplicationName}", app.ApplicationName);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "同步数据库失败，应用名称:{ApplicationName}", app.ApplicationName);
                }
            });
    }
}