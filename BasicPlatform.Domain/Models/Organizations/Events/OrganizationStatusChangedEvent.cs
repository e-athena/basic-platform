namespace BasicPlatform.Domain.Models.Organizations.Events;

/// <summary>
/// 组织状态变更事件
/// </summary>
public class OrganizationStatusChangedEvent : EventBase
{
    /// <summary>
    /// 组织状态
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="status"></param>
    public OrganizationStatusChangedEvent(Status status)
    {
        Status = status;
    }
}