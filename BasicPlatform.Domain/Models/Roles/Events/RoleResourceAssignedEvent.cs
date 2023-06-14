namespace BasicPlatform.Domain.Models.Roles.Events;

/// <summary>
/// 角色资源分配事件
/// </summary>
public class RoleResourceAssignedEvent : EventBase
{
    /// <summary>
    /// 资源列表
    /// </summary>
    public List<RoleResource> Resources { get; set; }

    public RoleResourceAssignedEvent(List<RoleResource> resources)
    {
        Resources = resources;
    }
}