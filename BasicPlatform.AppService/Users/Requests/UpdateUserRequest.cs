using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 更新用户请求类
/// </summary>
public class UpdateUserRequest : UserModel, ITxTraceRequest<string>
{
    /// <summary>
    /// 角色Ids
    /// </summary>
    public IList<string> RoleIds { get; set; } = new List<string>();

    /// <summary>
    /// 
    /// </summary>
    public string? RootTraceId { get; set; }
}