namespace BasicPlatform.Domain.Models;

/// <summary>
/// 用户数据权限
/// </summary>
[Table("authority_user_data_permissions")]
public class UserDataPermission : ValueObject
{
    /// <summary>
    /// 用户ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 用户
    /// </summary>
    /// <value></value>
    public virtual User User { get; set; } = null!;

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
    /// 有效期至
    /// <remarks>为空时永久有效</remarks>
    /// </summary>
    public DateTime? ExpireAt { get; set; }

    public UserDataPermission()
    {
    }

    public UserDataPermission(
        string userId,
        string resourceKey,
        RoleDataScope dataScope,
        string? dataScopeCustom,
        string policyResourceKey,
        IList<QueryFilterGroup>? policy,
        bool enabled,
        DateTime? expireAt)
    {
        UserId = userId; 
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
        ExpireAt = expireAt;
    }
}