using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Responses;

/// <summary>
/// 
/// </summary>
public class GetUserByIdResponse : UserQueryModel
{
    /// <summary>
    /// 角色
    /// </summary>
    public List<string> RoleIds { get; set; } = new();

    /// <summary>
    /// 组织架构
    /// </summary>
    public string? OrganizationPath { get; set; }
}