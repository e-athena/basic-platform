using BasicPlatform.Domain.Models.Users;

namespace BasicPlatform.Domain.Models;

/// <summary>
/// 资源
/// <remarks>对应页面上的模块、菜单和按钮</remarks>
/// </summary>
[Table("authority_resources")]
public class Resource : EntityCore, ICreator, IUpdater
{
    /// <summary>
    /// 应用ID
    /// </summary>
    [MaxLength(36)]
    public string ApplicationId { get; set; } = null!;

    /// <summary>
    /// 应用
    /// </summary>
    /// <value></value>
    public virtual Application? Application { get; set; }

    /// <summary>
    /// 唯一Key
    /// </summary>
    [MaxLength(128)]
    public string Key { get; set; } = null!;

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
    public string? LastUpdatedUserId { get; set; }

    /// <summary>
    /// 最后更新人
    /// </summary>
    public virtual User? LastUpdatedUser { get; set; }

    public Resource()
    {
    }

    public Resource(string applicationId, string key, int sort, Status status, string? userId)
    {
        ApplicationId = applicationId;
        Key = key;
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
        LastUpdatedUserId = userId;
    }

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="userId"></param>
    public void ChangeStatus(string? userId)
    {
        Status = Status == Status.Enabled ? Status.Disabled : Status.Enabled;
        LastUpdatedUserId = userId;
    }
}