using BasicPlatform.AppService.Roles.Models;
using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Responses;

/// <summary>
/// 
/// </summary>
public class GetUserByUserNameResponse : UserQueryModel
{
    /// <summary>
    /// 角色列表
    /// </summary>
    public List<RoleModel> Roles { get; set; } = new();
}