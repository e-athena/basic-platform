namespace BasicPlatform.Domain.Models.Users.Events;

/// <summary>
/// 用户状态变更事件
/// </summary>
public class UserStatusChangedEvent: EventBase
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="status"></param>
    public UserStatusChangedEvent(Status status)
    {
        Status = status;
    }
}