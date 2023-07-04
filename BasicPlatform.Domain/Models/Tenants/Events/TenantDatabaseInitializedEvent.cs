namespace BasicPlatform.Domain.Models.Tenants.Events;

/// <summary>
/// 租户数据库已初始化事件
/// </summary>
public class TenantDatabaseInitializedEvent : EventBase
{
    /// <summary>
    /// 更新人
    /// </summary>
    public string? LastUpdatedUserId { get; set; }


    public TenantDatabaseInitializedEvent(string? lastUpdatedUserId)
    {
        LastUpdatedUserId = lastUpdatedUserId;
    }
}