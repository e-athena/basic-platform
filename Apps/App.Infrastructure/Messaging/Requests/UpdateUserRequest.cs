using Athena.Infrastructure.Enums;

namespace App.Infrastructure.Messaging.Requests;

/// <summary>
/// 更新用户
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; set; } = null!;
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string RealName { get; set; } = null!;

    /// <summary>
    /// 性别
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string? NickName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    /// <value></value>
    public string? Email { get; set; }

    /// <summary>
    /// 所属组织ID
    /// </summary>
    public string OrganizationId { get; set; } = null!;

    /// <summary>
    /// 所属职位ID
    /// </summary>
    public string? PositionId { get; set; }

    /// <summary>
    /// 角色Ids
    /// </summary>
    public IList<string> RoleIds { get; set; } = new List<string>();
}