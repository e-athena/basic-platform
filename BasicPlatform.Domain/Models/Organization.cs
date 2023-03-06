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
    
    // 负责人
    // prop

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
    public string? UpdatedUserId { get; set; }

    /// <summary>
    /// 最后更新人
    /// </summary>
    public virtual User? UpdatedUser { get; set; }

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
    /// <param name="remarks"></param>
    /// <param name="status"></param>
    /// <param name="userId"></param>
    public Organization(string? parentId, string name, string? remarks, Status status, string? userId)
    {
        ParentId = parentId;
        Name = name;
        Remarks = remarks;
        Status = status;
        CreatedUserId = userId;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="name"></param>
    /// <param name="remarks"></param>
    /// <param name="userId"></param>
    public void Update(string? parentId, string name, string? remarks, string? userId)
    {
        ParentId = parentId;
        Name = name;
        Remarks = remarks;
        UpdatedUserId = userId;
    }

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="updatedUserId"></param>
    public void StatusChange(string? updatedUserId)
    {
        Status = Status == Status.Disabled ? Status.Enabled : Status.Disabled;
        UpdatedUserId = updatedUserId;
        UpdatedOn = DateTime.Now;
    }
}