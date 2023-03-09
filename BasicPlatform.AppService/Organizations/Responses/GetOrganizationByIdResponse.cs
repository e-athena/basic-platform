using BasicPlatform.AppService.Organizations.Models;

namespace BasicPlatform.AppService.Organizations.Responses;

/// <summary>
/// 
/// </summary>
public class GetOrganizationByIdResponse : OrganizationModel
{
    /// <summary>
    /// 角色ID列表
    /// </summary>
    public List<string> RoleIds { get; set; } = new();

    /// <summary>
    /// 部门负责人
    /// </summary>
    public string? LeaderName { get; set; }
}