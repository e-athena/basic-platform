using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 更新用户自定义列请求类
/// </summary>
public class UpdateUserCustomColumnsRequest : ITxRequest<long>
{
    /// <summary>
    /// 应用ID
    /// </summary>
    [MaxLength(64)]
    public string? AppId { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// 所属模块
    /// </summary>
    [MaxLength(128)]
    public string ModuleName { get; set; } = null!;

    /// <summary>
    /// 表格列列表
    /// </summary>
    public IList<UserCustomColumnModel> Columns { get; set; } = null!;
}