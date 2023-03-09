using BasicPlatform.AppService.Organizations.Models;

namespace BasicPlatform.AppService.Organizations.Responses;

/// <summary>
/// 组织架构分页返回值
/// </summary>
public class GetOrganizationPagingResponse : OrganizationModel
{
    /// <summary>
    /// 创建人Id  
    /// </summary>
    public string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedUserName { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedUserName { get; set; }

    /// <summary>
    /// 部门负责人
    /// </summary>
    public string? LeaderName { get; set; }
}