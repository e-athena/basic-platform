namespace BasicPlatform.AppService.Roles.Models;

/// <summary>
/// 角色数据查询策略模型
/// </summary>
public class RoleDataQueryPolicyModel
{
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
    public IList<QueryFilterGroup> Policies => string.IsNullOrEmpty(Policy)
        ? new List<QueryFilterGroup>()
        : JsonSerializer.Deserialize<IList<QueryFilterGroup>>(Policy) ?? new List<QueryFilterGroup>();

}