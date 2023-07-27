namespace BasicPlatform.Domain.Models.Tenants;

/// <summary>
/// 租户资源
/// </summary>
[Table("tenant_resources")]
public class TenantResource : ValueObject
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
    public string? ApplicationId { get; set; }

    /// <summary>
    /// 应用
    /// </summary>
    /// <value></value>
    public virtual Application? Application { get; set; } = null!;

    /// <summary>
    /// 资源Key
    /// </summary>
    [MaxLength(128)]
    public string ResourceKey { get; set; } = null!;

    /// <summary>
    /// 资源代码
    /// <remarks>多个用逗号分割</remarks>
    /// </summary>
    /// <value></value>
    [MaxLength(256)]
    public string ResourceCode { get; set; } = null!;

    /// <summary>
    /// 资源代码列表
    /// </summary>
    public IList<string> ResourceCodes =>
        string.IsNullOrEmpty(ResourceCode) ? new List<string>() : ResourceCode.Split(",");
    
    /// <summary>
    /// 
    /// </summary>
    public TenantResource()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="applicationId"></param>
    /// <param name="tenantId"></param>
    /// <param name="resourceKey"></param>
    /// <param name="resourceCode"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public TenantResource(string? applicationId, string tenantId, string resourceKey, string resourceCode)
    {
        ApplicationId = applicationId;
        TenantId = tenantId ?? throw new ArgumentNullException(nameof(tenantId));
        ResourceKey = resourceKey ?? throw new ArgumentNullException(nameof(resourceKey));
        ResourceCode = resourceCode ?? throw new ArgumentNullException(nameof(resourceCode));
    }
}