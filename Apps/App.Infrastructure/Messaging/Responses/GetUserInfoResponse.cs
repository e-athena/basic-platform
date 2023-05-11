using Athena.Infrastructure.Enums;

namespace App.Infrastructure.Messaging.Responses;

/// <summary>
/// 读取用户信息响应
/// </summary>
public class GetUserInfoResponse
{
    /// <summary>
    /// ID
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 头像
    /// </summary>
    public string? Avatar { get; set; }

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
    /// 资源编码列表
    /// </summary>
    public List<string> ResourceCodes { get; set; } = new();

    /// <summary>
    /// 组织名称
    /// </summary>
    public string? OrganizationName { get; set; }

    /// <summary>
    /// 职位名称
    /// </summary>
    public string? PositionName { get; set; }
}