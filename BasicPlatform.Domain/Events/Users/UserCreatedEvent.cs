namespace BasicPlatform.Domain.Events.Users;

/// <summary>
/// 
/// </summary>
public class UserCreatedEvent : EventBase
{
    /// <summary>
    /// 
    /// </summary>
    public string UserName { get; set; } = null!;
}