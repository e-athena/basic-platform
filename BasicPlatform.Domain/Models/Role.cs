namespace BasicPlatform.Domain.Models;

/// <summary>
/// 角色
/// </summary>
[Table("authority_roles")]
public class Role : EntityCore, ICreator, IUpdater
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(1024)]
    public string? Remarks { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <value></value>
    public Status Status { get; set; } = Status.Enabled;

    /// <summary>
    /// 创建人Id
    /// </summary>
    [MaxLength(36)]
    public string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public User? CreatedUser { get; set; }

    /// <summary>
    /// 最后更新人Id
    /// </summary>
    [MaxLength(36)]
    public string? UpdatedUserId { get; set; }

    /// <summary>
    /// 最后更新人
    /// </summary>
    public User? UpdatedUser { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Role()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="remarks"></param>
    /// <param name="createdUserId"></param>
    public Role(string name, string? remarks, string? createdUserId)
    {
        Name = name;
        Remarks = remarks;
        CreatedUserId = createdUserId;
    }
    
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="name"></param>
    /// <param name="remarks"></param>
    /// <param name="updatedUserId"></param>
    public void Update(string name, string? remarks, string? updatedUserId)
    {
        Name = name;
        Remarks = remarks;
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

    /// <summary>
    /// 分配权限
    /// </summary>
    /// <param name="updatedUserId"></param>
    public void AssignPermissions(string? updatedUserId)
    {
        UpdatedUserId = updatedUserId;
        UpdatedOn = DateTime.Now;
    }
}