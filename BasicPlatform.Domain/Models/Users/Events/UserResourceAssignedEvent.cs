namespace BasicPlatform.Domain.Models.Users.Events;

/// <summary>
/// 用户资源权限分配完成事件
/// </summary>
public class UserResourceAssignedEvent : EventBase
{
    /// <summary>
    /// 分配的权限
    /// </summary>
    public List<UserResource> Resources { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="resources"></param>
    public UserResourceAssignedEvent(List<UserResource> resources)
    {
        Resources = resources;
    }
}