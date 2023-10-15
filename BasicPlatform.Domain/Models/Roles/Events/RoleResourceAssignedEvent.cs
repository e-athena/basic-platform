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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="resources"></param>
    public RoleResourceAssignedEvent(List<RoleResource> resources)
    {
        Resources = resources;
    }
}