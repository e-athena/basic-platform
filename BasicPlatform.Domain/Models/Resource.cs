namespace BasicPlatform.Domain.Models;

/// <summary>
/// 资源
/// <remarks>对应页面上的模块、菜单和按钮</remarks>
/// </summary>
[Table("authority_resources")]
public class Resource : EntityCore, ICreator, IUpdater
{
    /// <summary>
    /// 唯一代码
    /// </summary>
    /// <value></value>
    [MaxLength(256)]
    public string Code { get; set; } = null!;

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; } = Status.Enabled;

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

    public Resource()
    {
    }

    public Resource(string code, int sort, Status status, string? userId)
    {
        Code = code;
        Sort = sort;
        Status = status;
        CreatedUserId = userId;
    }

    /// <summary>
    /// 更新排序值
    /// </summary>
    /// <param name="sort"></param>
    /// <param name="userId"></param>
    public void UpdateSort(int sort, string? userId)
    {
        Sort = sort;
        UpdatedUserId = userId;
    }

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="userId"></param>
    public void ChangeStatus(string? userId)
    {
        Status = Status == Status.Enabled ? Status.Disabled : Status.Enabled;
        UpdatedUserId = userId;
    }
}