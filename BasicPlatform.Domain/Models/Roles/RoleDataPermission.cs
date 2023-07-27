namespace BasicPlatform.Domain.Models.Roles;

/// <summary>
/// 角色数据权限
/// </summary>
[Table("authority_role_data_permissions")]
public class RoleDataPermission : ValueObject
{
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
    /// 角色ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string RoleId { get; set; } = null!;

    /// <summary>
    /// 角色
    /// </summary>
    /// <value></value>
    public virtual Role Role { get; set; } = null!;

    /// <summary>
    /// 基础资源Key
    /// </summary>
    public string ResourceKey { get; set; } = null!;

    /// <summary>
    /// 数据访问范围
    /// </summary>
    public RoleDataScope DataScope { get; set; }

    /// <summary>
    /// 自定义数据访问范围(组织Id)
    /// <remarks>多个组织使用逗号分割</remarks>
    /// </summary>
    [MaxLength(-1)]
    public string? DataScopeCustom { get; set; }

    /// <summary>
    /// 策略资源Key
    /// </summary>
    public string PolicyResourceKey { get; set; } = null!;

    /// <summary>
    /// 策略
    /// </summary>
    [MaxLength(-1)]
    public string Policy { get; set; } = null!;

    /// <summary>
    /// 策略
    /// </summary>
    [JsonIgnore]
    public IList<QueryFilterGroup> Policies => string.IsNullOrEmpty(Policy)
        ? new List<QueryFilterGroup>()
        : JsonSerializer.Deserialize<IList<QueryFilterGroup>>(Policy) ?? new List<QueryFilterGroup>();

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public RoleDataPermission()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="applicationId"></param>
    /// <param name="roleId"></param>
    /// <param name="resourceKey"></param>
    /// <param name="dataScope"></param>
    /// <param name="dataScopeCustom"></param>
    /// <param name="policyResourceKey"></param>
    /// <param name="policy"></param>
    /// <param name="enabled"></param>
    public RoleDataPermission(
        string? applicationId,
        string roleId,
        string resourceKey,
        RoleDataScope dataScope,
        string? dataScopeCustom,
        string policyResourceKey,
        IList<QueryFilterGroup>? policy,
        bool enabled)
    {
        ApplicationId = applicationId;
        RoleId = roleId;
        ResourceKey = resourceKey;
        DataScope = dataScope;
        DataScopeCustom = dataScopeCustom;
        PolicyResourceKey = policyResourceKey;
        Policy = string.Empty;
        if (policy is {Count: > 0})
        {
            Policy = JsonSerializer.Serialize(policy);
        }

        Enabled = enabled;
    }
}