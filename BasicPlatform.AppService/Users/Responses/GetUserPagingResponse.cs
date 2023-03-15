using Athena.Infrastructure.DataPermission.Attributes;
using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Responses;

/// <summary>
/// 读取用户分页数据响应类
/// </summary>
[DataPermission("用户管理模块")]
public class GetUserPagingResponse : UserModel
{
    /// <summary>
    /// 创建人ID
    /// </summary>
    [TableColumn(Show = false)]
    public string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    [TableColumn(Show = false)]
    public string? CreatedUserName { get; set; }

    /// <summary>
    /// 更新人ID
    /// </summary>
    [TableColumn(Show = false)]
    public string? UpdatedUserId { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    [TableColumn(Show = false)]
    public string? UpdatedUserName { get; set; }

    /// <summary>
    /// 组织名称
    /// </summary>
    [TableColumn(Sort = 6,Title = "所属组织")]
    public string? OrganizationName { get; set; }

    /// <summary>
    /// 职位名称
    /// </summary>
    [TableColumn(Sort = 7)]
    public string? PositionName { get; set; }
}