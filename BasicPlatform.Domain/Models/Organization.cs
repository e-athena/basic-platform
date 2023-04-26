namespace BasicPlatform.Domain.Models;

/// <summary>
/// 组织机构
/// </summary>
[Table("authority_organizations")]
// ReSharper disable once ClassNeverInstantiated.Global
public class Organization : EntityCore, ICreator, IUpdater
{
    /// <summary>
    /// 所属上级Id
    /// </summary>
    [MaxLength(36)]
    public string? ParentId { get; set; }

    /// <summary>
    /// 所属上级
    /// </summary>
    public virtual Organization? Parent { get; set; }

    /// <summary>
    /// 上级路径，用逗号分割，用于快速检索
    /// </summary>
    public string ParentPath { get; set; } = string.Empty;

    /// <summary>
    /// 名称
    /// </summary>
    [MaxLength(32)]
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(1000)]
    public string? Remarks { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; } = Status.Enabled;

    /// <summary>
    /// 部门负责人Id
    /// </summary>
    [MaxLength(36)]
    public string? LeaderId { get; set; }

    /// <summary>
    /// 部门负责人
    /// </summary>
    public virtual User? Leader { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 创建人Id
    /// </summary>
    [MaxLength(36)]
    public string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public virtual User? CreatedUser { get; set; }

    /// <summary>
    /// 最后更新人Id
    /// </summary>
    [MaxLength(36)]
    public string? LastUpdatedUserId { get; set; }

    /// <summary>
    /// 最后更新人
    /// </summary>
    public virtual User? LastUpdatedUser { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Organization()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="name"></param>
    /// <param name="leaderId"></param>
    /// <param name="remarks"></param>
    /// <param name="status"></param>
    /// <param name="sort"></param>
    /// <param name="userId"></param>
    public Organization(string? parentId, string name, string? leaderId, string? remarks, Status status, int sort,
        string? userId)
    {
        ParentId = parentId;
        Name = name;
        LeaderId = leaderId;
        Remarks = remarks;
        Status = status;
        Sort = sort;
        CreatedUserId = userId;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="name"></param>
    /// <param name="leaderId"></param>
    /// <param name="remarks"></param>
    /// <param name="sort"></param>
    /// <param name="userId"></param>
    public void Update(string? parentId, string name, string? leaderId, string? remarks, int sort, string? userId)
    {
        ParentId = parentId;
        Name = name;
        LeaderId = leaderId;
        Remarks = remarks;
        Sort = sort;
        LastUpdatedUserId = userId;
    }

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="updatedUserId"></param>
    public void StatusChange(string? updatedUserId)
    {
        Status = Status == Status.Disabled ? Status.Enabled : Status.Disabled;
        LastUpdatedUserId = updatedUserId;
        UpdatedOn = DateTime.Now;
    }
}