namespace BasicPlatform.Domain.Models;

/// <summary>
/// 职位
/// </summary>
[Table("authority_positions")]
public class Position : FullEntityCore
{
    /// <summary>
    /// 组织架构ID
    /// <remarks>为空时为通用职位</remarks>
    /// </summary>
    [MaxLength(36)]
    public string? OrganizationId { get; set; }

    /// <summary>
    /// 组织架构
    /// </summary>
    public virtual Organization? Organization { get; set; }

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
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    // /// <summary>
    // /// 创建人Id
    // /// </summary>
    // [MaxLength(36)]
    // public string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public virtual User? CreatedUser { get; set; }

    // /// <summary>
    // /// 最后更新人Id
    // /// </summary>
    // [MaxLength(36)]
    // public string? UpdatedUserId { get; set; }

    /// <summary>
    /// 最后更新人
    /// </summary>
    public virtual User? UpdatedUser { get; set; }


    /// <summary>
    /// 
    /// </summary>
    public Position()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationId"></param>
    /// <param name="name"></param>
    /// <param name="remarks"></param>
    /// <param name="status"></param>
    /// <param name="sort"></param>
    /// <param name="createdUserId"></param>
    public Position(string? organizationId, string name, string? remarks, Status status, int sort,
        string? createdUserId)
    {
        OrganizationId = organizationId;
        Name = name;
        Remarks = remarks;
        Status = status;
        Sort = sort;
        CreatedUserId = createdUserId;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="organizationId"></param>
    /// <param name="name"></param>
    /// <param name="remarks"></param>
    /// <param name="sort"></param>
    /// <param name="updatedUserId"></param>
    public void Update(string? organizationId, string name, string? remarks, int sort, string? updatedUserId)
    {
        OrganizationId = organizationId;
        Name = name;
        Remarks = remarks;
        Sort = sort;
        UpdatedUserId = updatedUserId;
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