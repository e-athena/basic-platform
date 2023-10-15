namespace BasicPlatform.Domain.Models.Roles.Events;

/// <summary>
/// 角色列权限分配完成事件
/// </summary>
public class RoleColumnPermissionAssignedEvent : EventBase
{
    /// <summary>
    /// 分配的权限
    /// </summary>
    public List<RoleColumnPermission> Permissions { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissions"></param>
    public RoleColumnPermissionAssignedEvent(List<RoleColumnPermission> permissions)
    {
        Permissions = permissions;
    }
}