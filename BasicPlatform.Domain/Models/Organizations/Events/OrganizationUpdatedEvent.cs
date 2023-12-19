namespace BasicPlatform.Domain.Models.Organizations.Events;

/// <summary>
/// 组织更新事件
/// </summary>
public class OrganizationUpdatedEvent : EventBase
{
    /// <summary>
    /// 父级Id
    /// </summary>
    public string? ParentId { get; set; }

    /// <summary>
    /// 组织名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 组织负责人Id
    /// </summary>
    public string? LeaderId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="name"></param>
    /// <param name="leaderId"></param>
    /// <param name="remarks"></param>
    /// <param name="sort"></param>
    public OrganizationUpdatedEvent(string? parentId, string name, string? leaderId, string? remarks,
        int sort)
    {
        ParentId = parentId;
        Name = name;
        LeaderId = leaderId;
        Remarks = remarks;
        Sort = sort;
    }
}