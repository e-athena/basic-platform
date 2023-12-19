namespace BasicPlatform.Domain.Models.Applications.Events;

/// <summary>
/// 子应用状态变更事件
/// </summary>
public class ApplicationStatusChangedEvent: EventBase
{
    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="status"></param>
    public ApplicationStatusChangedEvent(Status status)
    {
        Status = status;
    }
}