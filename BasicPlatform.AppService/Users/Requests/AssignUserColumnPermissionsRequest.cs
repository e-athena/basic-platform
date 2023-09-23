using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 分配用户列权限请求类
/// </summary>
public class AssignUserColumnPermissionsRequest : ITxRequest<string>
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 权限列表
    /// </summary>
    public IList<UserColumnPermissionModel> Permissions { get; set; } = new List<UserColumnPermissionModel>();

    /// <summary>
    /// 有效期至
    /// <remarks>为空时永久有效</remarks>
    /// </summary>
    public DateTime? ExpireAt { get; set; }
}