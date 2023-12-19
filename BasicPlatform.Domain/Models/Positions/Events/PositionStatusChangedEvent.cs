namespace BasicPlatform.Domain.Models.Positions.Events;

/// <summary>
/// 职位状态变更事件
/// </summary>
public class PositionStatusChangedEvent : EventBase
{
    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="status"></param>
    public PositionStatusChangedEvent(Status status)
    {
        Status = status;
    }
}