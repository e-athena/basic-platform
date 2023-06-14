namespace BasicPlatform.Domain.Models.Roles.Events;

/// <summary>
/// 角色用户分配事件
/// </summary>
public class RoleUserAssignedEvent : EventBase
{
    /// <summary>
    /// 用户ID列表
    /// </summary>
    public List<string> UserIds { get; set; }

    public RoleUserAssignedEvent(List<string> userIds)
    {
        UserIds = userIds;
    }
}