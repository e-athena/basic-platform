namespace BasicPlatform.Domain.Models.Roles;

/// <summary>
/// 角色资源
/// </summary>
[Table("authority_role_resources")]
public class RoleResource : ValueObject
{
    /// <summary>
    /// 应用ID
    /// </summary>
    [MaxLength(36)]
    public string? AppId { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string RoleId { get; set; } = null!;

    /// <summary>
    /// 角色
    /// </summary>
    /// <value></value>
    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public virtual Role Role { get; set; } = null!;

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
    public RoleResource()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public RoleResource(string? appId, string roleId, string resourceKey, string resourceCode)
    {
        AppId = appId;
        RoleId = roleId ?? throw new ArgumentNullException(nameof(roleId));
        ResourceKey = resourceKey ?? throw new ArgumentNullException(nameof(resourceKey));
        ResourceCode = resourceCode ?? throw new ArgumentNullException(nameof(resourceCode));
    }
}