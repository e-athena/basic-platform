using BasicPlatform.AppService.Roles.Models;
using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Responses;

/// <summary>
/// 
/// </summary>
public class GetUserByUserNameResponse : UserModel
{
    /// <summary>
    /// 角色列表
    /// </summary>
    public List<RoleModel> Roles { get; set; } = new();

    /// <summary>
    /// 组织架构名称
    /// </summary>
    public List<string> OrganizationName { get; set; } = new();
}