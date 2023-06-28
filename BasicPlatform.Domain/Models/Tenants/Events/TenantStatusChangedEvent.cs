namespace BasicPlatform.Domain.Models.Tenants.Events;

/// <summary>
/// 租户状态变更成功事件
/// </summary>
public class TenantStatusChangedEvent : EventBase
{
    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; }

    public TenantStatusChangedEvent(Status status)
    {
        Status = status;
    }
}