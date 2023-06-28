namespace BasicPlatform.Domain.Models.Tenants;

/// <summary>
/// 租户应用配置
/// </summary>
[Table("tenant_applications")]
public class TenantApplication : ValueObject
{
    /// <summary>
    /// 租户ID
    /// </summary>
    [MaxLength(36)]
    public string TenantId { get; set; } = null!;

    /// <summary>
    /// 组织架构
    /// </summary>
    public virtual Tenant Tenant { get; set; } = null!;

    /// <summary>
    /// 应用ID
    /// </summary>
    [MaxLength(36)]
    public string ApplicationId { get; set; } = null!;

    /// <summary>
    /// 应用
    /// </summary>
    public virtual Application Application { get; set; } = null!;

    /// <summary>
    /// 数据库链接字符串
    /// </summary>
    public string ConnectionString { get; set; } = null!;

    /// <summary>
    /// 订阅过期时间
    /// <remarks>为空时永久有效</remarks>
    /// </summary>
    public DateTime? ExpiredTime { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedUserId { get; set; }

    public TenantApplication()
    {
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="applicationId"></param>
    /// <param name="connectionString"></param>
    /// <param name="expiredTime"></param>
    /// <param name="createdUserId"></param>
    /// <param name="isEnabled"></param>
    public TenantApplication(
        string tenantId,
        string applicationId,
        string connectionString,
        DateTime? expiredTime,
        string? createdUserId,
        bool isEnabled
    )
    {
        TenantId = tenantId;
        ApplicationId = applicationId;
        ConnectionString = connectionString != string.Empty ? SecurityHelper.Encrypt(connectionString) : string.Empty;
        ExpiredTime = expiredTime;
        CreatedUserId = createdUserId;
        IsEnabled = isEnabled;
    }
}