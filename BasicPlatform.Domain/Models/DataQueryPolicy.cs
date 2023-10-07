using BasicPlatform.Domain.Models.Users;

namespace BasicPlatform.Domain.Models;

/// <summary>
/// 数据查询策略
/// </summary>
[Table("authority_data_query_policies")]
public class DataQueryPolicy : FullEntityCore
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 资源名称
    /// </summary>
    public string ResourceName { get; set; } = null!;

    /// <summary>
    /// 资源Key
    /// </summary>
    public string ResourceKey { get; set; } = null!;

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
    /// 状态
    /// </summary>
    public Status Status { get; set; } = Status.Enabled;

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public User? CreatedUser { get; set; }

    /// <summary>
    /// 最后更新人
    /// </summary>
    public User? UpdatedUser { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DataQueryPolicy()
    {
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="resourceKey">资源Key</param>
    /// <param name="policy"></param>
    /// <param name="status"></param>
    /// <param name="createdUserId"></param>
    public DataQueryPolicy(string resourceKey, IList<QueryFilterGroup> policy, Status status, string? createdUserId)
    {
        ResourceKey = resourceKey;
        Policy = JsonSerializer.Serialize(policy);
        Status = status;
        CreatedUserId = createdUserId;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="policy"></param>
    /// <param name="updatedUserId"></param>
    public void Update(IList<QueryFilterGroup> policy, string? updatedUserId)
    {
        Policy = JsonSerializer.Serialize(policy);
        LastUpdatedUserId = updatedUserId;
    }

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="status"></param>
    /// <param name="updatedUserId"></param>
    public void ChangeStatus(Status status, string? updatedUserId)
    {
        Status = status;
        LastUpdatedUserId = updatedUserId;
    }
}