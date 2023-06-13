namespace BasicPlatform.Domain.Models.Users.Events;

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