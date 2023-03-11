using BasicPlatform.AppService.Resources.Models;
using BasicPlatform.AppService.Resources.Requests;

namespace BasicPlatform.WebAPI.Controllers.Permissions;

/// <summary>
/// 资源管理
/// </summary>
[Menu("资源管理",
    ModuleCode = "permission",
    ModuleName = "权限管理",
    ModuleIcon = "SafetyOutlined",
    ModuleRoutePath = "/permission",
    ModuleSort = 2,
    
    RoutePath = "/permission/resource",
    Sort = 1,
    Description = "资源包含菜单树以及功能，由系统生成，用于控制系统菜单的展示及功能权限。"
)]
public class ResourceController : CustomControllerBase
{
    private readonly IMediator _mediator;
    private readonly IApiPermissionService _apiPermissionService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="apiPermissionService"></param>
    public ResourceController(IMediator mediator, IApiPermissionService apiPermissionService)
    {
        _mediator = mediator;
        _apiPermissionService = apiPermissionService;
    }

    /// <summary>
    /// 同步
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(DisplayName = "同步资源", Description = "该操作会增量添加资源数据，存在且已分配的数据不会被删除，不存在且分配的的数据将会被删除")]
    public Task<int> SyncAsync(CancellationToken cancellationToken)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resources = _apiPermissionService.GetResourceCodeInfos(assembly);
        var request = new SyncResourceRequest
        {
            Resources = resources.Select(p => new ResourceModel
            {
                Key = p.Key,
                Code = p.Code
            }).ToList()
        };
        return _mediator.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// 重新初始化资源
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiPermission(DisplayName = "重置资源", Description = "该操作会重置所有资源数据，所有被分配的资源也将会被清空")]
    public Task<int> ReinitializeAsync(CancellationToken cancellationToken)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resources = _apiPermissionService.GetResourceCodeInfos(assembly);
        var request = new ReinitializeResourceRequest
        {
            Resources = resources.Select(p => new ResourceModel
            {
                Key = p.Key,
                Code = p.Code
            }).ToList()
        };
        return _mediator.SendAsync(request, cancellationToken);
    }
}