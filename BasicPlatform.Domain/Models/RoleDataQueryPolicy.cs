namespace BasicPlatform.Domain.Models;

/// <summary>
/// 角色数据查询策略
/// </summary>
[Table("authority_role_data_query_policies")]
public class RoleDataQueryPolicy : ValueObject
{
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
    public string BaseResourceKey { get; set; } = null!;

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

    public RoleDataQueryPolicy()
    {
    }

    public RoleDataQueryPolicy(string roleId, string baseResourceKey, string policyResourceKey,
        IList<QueryFilterGroup>? policy, bool enabled)
    {
        RoleId = roleId;
        BaseResourceKey = baseResourceKey;
        PolicyResourceKey = policyResourceKey;
        Policy = string.Empty;
        if (policy is {Count: > 0})
        {
            Policy = JsonSerializer.Serialize(policy);
        }

        Enabled = Policy != string.Empty && enabled;
    }
}