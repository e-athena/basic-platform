using BasicPlatform.AppService.Organizations.Models;

namespace BasicPlatform.AppService.Organizations.Requests;

/// <summary>
/// 创建组织架构请求类
/// </summary>
public class CreateOrganizationRequest : OrganizationModel, ITxRequest<string>
{
    /// <summary>
    /// 角色Ids
    /// </summary>
    public IList<string> RoleIds { get; set; } = new List<string>();
}