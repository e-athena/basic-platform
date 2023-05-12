namespace BasicPlatform.AppService.Applications.Models;

/// <summary>
/// 网站系统应用
/// </summary>
public class ApplicationQueryModel : QueryModelBase
{
    /// <summary>
    /// 名称
    /// </summary>
    [TableColumn(Width = 120, Sort = 1, Fixed = TableColumnFixed.Left)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 客户端ID
    /// </summary>
    [TableColumn(Width = 100, Sort = 2, Fixed = TableColumnFixed.Left)]
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// 客户端密钥
    /// </summary>
    [TableColumn(Sort = 3)]
    public string ClientSecret { get; set; } = null!;

    /// <summary>
    /// 前端地址
    /// </summary>
    [TableColumn(Sort = 4)]
    public string? FrontendUrl { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    [TableColumn(Sort = 5)]
    public string? ApiUrl { get; set; }

    /// <summary>
    /// 菜单资源地址
    /// </summary>
    [TableColumn(Sort = 6)]
    public string? MenuResourceRoute { get; set; }

    /// <summary>
    /// 权限资源地址
    /// </summary>
    [TableColumn(Sort = 7)]
    public string? PermissionResourceRoute { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [TableColumn(Sort = 8)]
    public string? Remarks { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <value></value>
    [TableColumn(Width = 90, Sort = 9)]
    public Status Status { get; set; }
}