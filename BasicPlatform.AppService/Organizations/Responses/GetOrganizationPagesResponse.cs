using BasicPlatform.AppService.Organizations.Models;

namespace BasicPlatform.AppService.Organizations.Responses;

/// <summary>
/// 组织架构分页返回值
/// </summary>
public class GetOrganizationPagesResponse : OrganizationViewModel
{
    /// <summary>
    /// 创建人Id  
    /// </summary>
    public string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedUserName { get; set; }
}