using BasicPlatform.AppService.Positions.Models;

namespace BasicPlatform.AppService.Positions.Responses;

/// <summary>
/// 读取职位分页列表响应类
/// </summary>
public class GetPositionPagingResponse : PositionModel
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
    /// 组织名称
    /// </summary>
    public string? OrganizationName { get; set; }

}