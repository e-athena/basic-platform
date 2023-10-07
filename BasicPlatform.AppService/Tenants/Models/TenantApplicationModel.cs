namespace BasicPlatform.AppService.Tenants.Models;

/// <summary>
/// 租户应用
/// </summary>
public class TenantApplicationModel
{
    /// <summary>
    /// 租户ID
    /// </summary>
    public string? TenantId { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public string ApplicationName { get; set; } = null!;

    /// <summary>
    /// 应用接口地址
    /// </summary>
    public string? ApplicationApiUrl { get; set; }

    /// <summary>
    /// 应用客户端ID
    /// </summary>
    public string ApplicationClientId { get; set; } = null!;

    /// <summary>
    /// 租户类型
    /// </summary>
    public TenantIsolationLevel IsolationLevel { get; set; }

    /// <summary>
    /// 数据库链接字符串
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// 订阅过期时间
    /// <remarks>为空时永久有效</remarks>
    /// </summary>
    public DateTime? ExpiredTime { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; set; }
}