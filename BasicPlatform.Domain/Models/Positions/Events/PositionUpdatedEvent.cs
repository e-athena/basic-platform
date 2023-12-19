namespace BasicPlatform.Domain.Models.Positions.Events;

/// <summary>
/// 职位更新事件
/// </summary>
public class PositionUpdatedEvent : EventBase
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
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="organizationId"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="sort"></param>
    public PositionUpdatedEvent(string? organizationId, string name, string? description, int sort)
    {
        OrganizationId = organizationId;
        Name = name;
        Description = description;
        Sort = sort;
    }
}