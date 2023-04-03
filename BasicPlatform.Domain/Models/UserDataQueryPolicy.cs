namespace BasicPlatform.Domain.Models;

/// <summary>
/// 用户数据查询策略
/// </summary>
[Table("authority_user_data_query_policies")]
public class UserDataQueryPolicy : ValueObject
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

    /// <summary>
    /// 有效期至
    /// <remarks>为空时永久有效</remarks>
    /// </summary>
    public DateTime? ExpireAt { get; set; }


    public UserDataQueryPolicy()
    {
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="baseResourceKey"></param>
    /// <param name="policyResourceKey"></param>
    /// <param name="policy"></param>
    /// <param name="enabled"></param>
    /// <param name="expireAt"></param>
    public UserDataQueryPolicy(string userId, string baseResourceKey, string policyResourceKey,
        IList<QueryFilterGroup>? policy, bool enabled,
        DateTime? expireAt)
    {
        UserId = userId;
        BaseResourceKey = baseResourceKey;
        PolicyResourceKey = policyResourceKey;
        ExpireAt = expireAt;
        Policy = string.Empty;
        if (policy is {Count: > 0})
        {
            Policy = JsonSerializer.Serialize(policy);
        }

        Enabled = Policy != string.Empty && enabled;
    }
}