namespace BasicPlatform.Domain.Models.Applications.Events;

/// <summary>
/// 子应用更新事件
/// </summary>
public class ApplicationUpdatedEvent : EventBase
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 客户端ID
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// 是否使用系统默认的客户端密钥
    /// </summary>
    public bool UseDefaultClientSecret { get; set; }

    /// <summary>
    /// 前端地址
    /// </summary>
    public string? FrontendUrl { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    public string? ApiUrl { get; set; }

    /// <summary>
    /// 菜单资源路由
    /// </summary>
    public string? MenuResourceRoute { get; set; }

    /// <summary>
    /// 权限资源地址
    /// </summary>
    public string? PermissionResourceRoute { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <param name="clientId"></param>
    /// <param name="useDefaultClientSecret"></param>
    /// <param name="frontendUrl"></param>
    /// <param name="apiUrl"></param>
    /// <param name="menuResourceRoute"></param>
    /// <param name="permissionResourceRoute"></param>
    /// <param name="remarks"></param>
    public ApplicationUpdatedEvent(string name, string clientId, bool useDefaultClientSecret, string? frontendUrl,
        string? apiUrl, string? menuResourceRoute, string? permissionResourceRoute, string? remarks)
    {
        Name = name;
        ClientId = clientId;
        UseDefaultClientSecret = useDefaultClientSecret;
        FrontendUrl = frontendUrl;
        ApiUrl = apiUrl;
        MenuResourceRoute = menuResourceRoute;
        PermissionResourceRoute = permissionResourceRoute;
        Remarks = remarks;
    }
}