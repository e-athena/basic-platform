using BasicPlatform.AppService.Roles.Models;

namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 分配用户数据权限请求类
/// </summary>
public class AssignUserDataPermissionsRequest : ITxRequest<string>
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 权限列表
    /// </summary>
    public IList<RoleDataPermissionModel> Permissions { get; set; } = new List<RoleDataPermissionModel>();

    /// <summary>
    /// 有效期至
    /// <remarks>为空时永久有效</remarks>
    /// </summary>
    public DateTime? ExpireAt { get; set; }
}