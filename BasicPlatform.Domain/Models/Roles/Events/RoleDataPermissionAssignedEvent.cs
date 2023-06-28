namespace BasicPlatform.Domain.Models.Roles.Events;

/// <summary>
/// 角色数据权限分配完成事件
/// </summary>
public class RoleDataPermissionAssignedEvent : EventBase
{
    /// <summary>
    /// 分配的权限
    /// </summary>
    public List<RoleDataPermission> Permissions { get; set; }

    public RoleDataPermissionAssignedEvent(List<RoleDataPermission> permissions)
    {
        Permissions = permissions;
    }
}