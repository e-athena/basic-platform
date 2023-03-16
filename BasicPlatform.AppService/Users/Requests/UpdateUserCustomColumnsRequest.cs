using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 更新用户自定义列请求类
/// </summary>
public class UpdateUserCustomColumnsRequest : ITxRequest<long>
{
    /// <summary>
    /// 所属模块
    /// </summary>
    [MaxLength(128)]
    public string? ModuleName { get; set; }

    /// <summary>
    /// 表格列列表
    /// </summary>
    public IList<UserCustomColumnModel> Columns { get; set; } = null!;
}