namespace BasicPlatform.Domain.Models.Roles.Events;

/// <summary>
/// 角色状态变更事件
/// </summary>
public class RoleStatusChangedEvent : EventBase
{
    /// <summary>
    /// 角色状态
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="status"></param>
    public RoleStatusChangedEvent(Status status)
    {
        Status = status;
    }
}