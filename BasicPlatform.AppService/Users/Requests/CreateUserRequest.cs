using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 创建用户请求类
/// </summary>
public class CreateUserRequest : UserModel, ITxRequest<string>
{
    /// <summary>
    /// 角色Ids
    /// </summary>
    public IList<string> RoleIds { get; set; } = new List<string>();
}