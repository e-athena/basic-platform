using BasicPlatform.Domain.Models.Applications.Events;
using BasicPlatform.Domain.Models.Users;

namespace BasicPlatform.Domain.Models.Applications;

/// <summary>
/// 业务子应用
/// </summary>
[Table("authority_applications")]
public class Application : FullEntityCore
{
    /// <summary>
    /// 运行环境
    /// </summary>
    [MaxLength(32)]
    public string Environment { get; set; } = "Development";

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 客户端ID
    /// </summary>
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// 客户端密钥
    /// </summary>
    public string ClientSecret { get; set; } = null!;

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
    /// 状态
    /// </summary>
    public Status Status { get; set; } = Status.Enabled;

    /// <summary>
    /// 创建人
    /// </summary>
    public virtual User? CreatedUser { get; set; }

    /// <summary>
    /// 最后更新人
    /// </summary>
    public virtual User? LastUpdatedUser { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Application()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="environment"></param>
    /// <param name="name"></param>
    /// <param name="clientId"></param>
    /// <param name="useDefaultClientSecret"></param>
    /// <param name="frontendUrl"></param>
    /// <param name="apiUrl"></param>
    /// <param name="menuResourceRoute"></param>
    /// <param name="permissionResourceRoute"></param>
    /// <param name="remarks"></param>
    /// <param name="createdUserId"></param>
    public Application(
        string environment,
        string name,
        string clientId,
        bool useDefaultClientSecret,
        string? frontendUrl,
        string? apiUrl,
        string? menuResourceRoute,
        string? permissionResourceRoute,
        string? remarks,
        string? createdUserId
    )
    {
        Environment = environment;
        Name = name;
        ClientId = clientId;
        UseDefaultClientSecret = useDefaultClientSecret;
        ClientSecret = Guid.NewGuid().ToString();
        FrontendUrl = frontendUrl;
        ApiUrl = apiUrl;
        MenuResourceRoute = menuResourceRoute;
        PermissionResourceRoute = permissionResourceRoute;
        Remarks = remarks;
        CreatedUserId = createdUserId;

        ApplyEvent(new ApplicationCreatedEvent(
            environment,
            name,
            clientId,
            ClientSecret,
            useDefaultClientSecret,
            frontendUrl,
            apiUrl,
            menuResourceRoute,
            permissionResourceRoute,
            remarks
        ));
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="name"></param>
    /// <param name="clientId"></param>
    /// <param name="useDefaultClientSecret"></param>
    /// <param name="frontendUrl"></param>
    /// <param name="apiUrl"></param>
    /// <param name="menuResourceRoute"></param>
    /// <param name="permissionResourceRoute"></param>
    /// <param name="remarks"></param>
    /// <param name="updatedUserId"></param>
    public void Update(
        string name,
        string clientId,
        bool useDefaultClientSecret,
        string? frontendUrl, string? apiUrl, string? menuResourceRoute,
        string? permissionResourceRoute, string? remarks, string? updatedUserId)
    {
        Name = name;
        ClientId = clientId;
        UseDefaultClientSecret = useDefaultClientSecret;
        FrontendUrl = frontendUrl;
        ApiUrl = apiUrl;
        MenuResourceRoute = menuResourceRoute;
        PermissionResourceRoute = permissionResourceRoute;
        Remarks = remarks;
        LastUpdatedUserId = updatedUserId;

        ApplyEvent(new ApplicationUpdatedEvent(
            name,
            clientId,
            useDefaultClientSecret,
            frontendUrl,
            apiUrl,
            menuResourceRoute,
            permissionResourceRoute,
            remarks
        ));
    }

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="updatedUserId"></param>
    public void StatusChange(string? updatedUserId)
    {
        Status = Status == Status.Disabled ? Status.Enabled : Status.Disabled;
        LastUpdatedUserId = updatedUserId;
        UpdatedOn = DateTime.Now;

        ApplyEvent(new ApplicationStatusChangedEvent(Status));
    }
}