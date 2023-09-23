using BasicPlatform.Domain.Models.Roles;

namespace BasicPlatform.Domain.Models.Users.Events;

/// <summary>
/// 用户列权限分配完成事件
/// </summary>
public class UserColumnPermissionAssignedEvent : EventBase
{
    /// <summary>
    /// 分配的权限
    /// </summary>
    public List<UserColumnPermission> Permissions { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissions"></param>
    public UserColumnPermissionAssignedEvent(List<UserColumnPermission> permissions)
    {
        Permissions = permissions;
    }
}