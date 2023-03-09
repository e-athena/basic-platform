using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Responses;

/// <summary>
/// 读取用户分页数据响应类
/// </summary>
public class GetUserPagingResponse : UserModel
{
    /// <summary>
    /// 创建人ID
    /// </summary>
    public string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedUserName { get; set; }

    /// <summary>
    /// 更新人ID
    /// </summary>
    public string? UpdatedUserId { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedUserName { get; set; }

    /// <summary>
    /// 组织名称
    /// </summary>
    public string? OrganizationName { get; set; }

    /// <summary>
    /// 职位名称
    /// </summary>
    public string? PositionName { get; set; }
}