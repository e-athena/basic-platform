using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 更新用户请求类
/// </summary>
public class UpdateUserRequest : UserModel,ITxRequest<string>
{
    /// <summary>
    /// 组织架构Ids
    /// </summary>
    public IList<string> OrganizationIds { get; set; } = new List<string>();
    
    /// <summary>
    /// 角色Ids
    /// </summary>
    public IList<string> RoleIds { get; set; } = new List<string>();
}