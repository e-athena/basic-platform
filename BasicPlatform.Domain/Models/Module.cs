namespace BasicPlatform.Domain.Models;

/// <summary>
/// 模块
/// </summary>
[Table("AuthorityModules")]
public class Module : EntityCore, ICreator, IUpdater
{
    /// <summary>
    /// 父级ID
    /// </summary>
    [MaxLength(36)]
    public string? ParentId { get; set; }

    /// <summary>
    /// 父级ID
    /// </summary>
    public virtual Module? Parent { get; set; }

    /// <summary>
    /// 上级路径，用逗号分割，用于快速检索
    /// </summary>
    public string ParentPath { get; set; } = string.Empty;

    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [MaxLength(12)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 跳转路径
    /// </summary>
    [MaxLength(512)]
    public string Path { get; set; } = null!;

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; } = Status.Enabled;

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(512)]
    public string? Remarks { get; set; }

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

    public Module()
    {
    }

    public Module(
        string? parentId,
        string name,
        string path,
        int sort, Status status,
        string? userId
    )
    {
        ParentId = parentId;
        Name = name;
        Path = path;
        Sort = sort;
        Status = status;
        CreatedUserId = userId;
        UpdatedUserId = userId;
    }

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="userId"></param>
    public void StatusChange(string? userId)
    {
        Status = Status == Status.Disabled ? Status.Enabled : Status.Disabled;
        UpdatedUserId = userId;
        UpdatedOn = DateTime.Now;
    }
}