namespace BasicPlatform.Domain.Models.Positions.Events;

/// <summary>
/// 职位创建事件
/// </summary>
public class PositionCreatedEvent : EventBase
{
    /// <summary>
    /// 组织Id
    /// </summary>
    public string? OrganizationId { get; set; }

    /// <summary>
    /// 职位名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 职位描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationId"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="status"></param>
    /// <param name="sort"></param>
    public PositionCreatedEvent(string? organizationId, string name, string? description, Status status, int sort)
    {
        OrganizationId = organizationId;
        Name = name;
        Description = description;
        Status = status;
        Sort = sort;
    }
}