namespace BasicPlatform.Domain.Models;

/// <summary>
/// 职位
/// </summary>
[Table("authority_positions")]
// ReSharper disable once ClassNeverInstantiated.Global
public class Position : EntityCore, ICreator, IUpdater
{
    /// <summary>
    /// 上级
    /// </summary>
    [MaxLength(36)]
    public string? ParentId { get; set; }

    /// <summary>
    /// 上级
    /// </summary>
    public virtual Position? Parent { get; set; }

    /// <summary>
    /// 上级路径，用逗号分割，用于快速检索
    /// </summary>
    public string ParentPath { get; set; } = string.Empty;

    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; }

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

    public Position()
    {
    }

    public Position(string? parentId, string name, string? remarks, Status status, string? userId)
    {
        ParentId = parentId;
        Name = name;
        Remarks = remarks;
        Status = status;
        CreatedUserId = userId;
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

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="parentPath"></param>
    /// <param name="name"></param>
    /// <param name="remarks"></param>
    /// <param name="userId"></param>
    public void Update(string? parentId, string parentPath, string name, string? remarks, string? userId)
    {
        ParentId = parentId;
        ParentPath = parentPath;
        Name = name;
        Remarks = remarks;
        UpdatedUserId = userId;
        UpdatedOn = DateTime.Now;
    }
}