namespace BasicPlatform.Domain.Models.Tenants.Events;

/// <summary>
/// 租户资源分配事件
/// </summary>
public class TenantResourceAssignedEvent : EventBase
{
    /// <summary>
    /// 资源列表
    /// </summary>
    public List<TenantResource> Resources { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="resources"></param>
    public TenantResourceAssignedEvent(List<TenantResource> resources)
    {
        Resources = resources;
    }
}