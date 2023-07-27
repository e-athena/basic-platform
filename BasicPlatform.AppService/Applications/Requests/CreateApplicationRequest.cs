namespace BasicPlatform.AppService.Applications.Requests;

/// <summary>
/// 创建网站系统应用请求类
/// </summary>
public class CreateApplicationRequest : ITxRequest<string>
{
    /// <summary>
    /// 运行环境
    /// </summary>
    public string Environment { get; set; } = null!;

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 客户端ID
    /// </summary>
    public string ClientId { get; set; } = null!;

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
    /// 菜单资源地址
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
}