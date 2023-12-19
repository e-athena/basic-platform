using BasicPlatform.Domain.Models.Users;

namespace BasicPlatform.Domain.Models;

/// <summary>
/// 资源
/// </summary>
[Table("authority_resources")]
public class Resource : FullEntityCore
{
    /// <summary>
    /// 应用ID
    /// </summary>
    [MaxLength(36)]
    public string AppId { get; set; } = null!;

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
    /// 创建人
    /// </summary>
    public virtual User? CreatedUser { get; set; }

    /// <summary>
    /// 最后更新人
    /// </summary>
    public virtual User? LastUpdatedUser { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Resource()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="key"></param>
    /// <param name="sort"></param>
    /// <param name="status"></param>
    /// <param name="userId"></param>
    public Resource(string appId, string key, int sort, Status status, string? userId)
    {
        AppId = appId;
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