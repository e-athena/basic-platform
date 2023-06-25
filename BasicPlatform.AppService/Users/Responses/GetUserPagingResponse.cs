using BasicPlatform.AppService.Users.Models;
using BasicPlatform.Domain.Models.Users;

namespace BasicPlatform.AppService.Users.Responses;

/// <summary>
/// 读取用户分页数据响应类
/// </summary>
[DataPermission(typeof(User), "用户管理模块", AppId = "system")]
public class GetUserPagingResponse : UserQueryModel
{
    /// <summary>
    /// 组织名称
    /// </summary>
    [TableColumn(Sort = 6, Title = "所属组织")]
    public string? OrganizationName { get; set; }

    /// <summary>
    /// 职位名称
    /// </summary>
    [TableColumn(Sort = 7)]
    public string? PositionName { get; set; }
}