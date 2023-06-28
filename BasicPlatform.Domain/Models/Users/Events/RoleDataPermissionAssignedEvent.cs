namespace BasicPlatform.Domain.Models.Users.Events;

/// <summary>
/// 用户数据权限分配完成事件
/// </summary>
public class UserDataPermissionAssignedEvent : EventBase
{
    /// <summary>
    /// 分配的权限
    /// </summary>
    public List<UserDataPermission> Permissions { get; set; }

    public UserDataPermissionAssignedEvent(List<UserDataPermission> permissions)
    {
        Permissions = permissions;
    }
}