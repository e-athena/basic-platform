using BasicPlatform.AppService.Roles.Models;

namespace BasicPlatform.AppService.Roles.Responses;

/// <summary>
/// 读取角色分页列表响应类
/// </summary>
public class GetRolePagingResponse : RoleModel
{
    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedUserName { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <value></value>
    public Status Status { get; set; }
}