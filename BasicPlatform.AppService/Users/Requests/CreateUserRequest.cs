namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 创建用户请求类
/// </summary>
public class CreateUserRequest : ITxRequest<string>
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// 真实姓名
    /// </summary>
    [MaxLength(16)]
    public string RealName { get; set; } = null!;

    /// <summary>
    /// 手机号
    /// </summary>
    [MaxLength(11)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    /// <value></value>
    [MaxLength(256)]
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>
    /// 组织架构Ids
    /// </summary>
    public IList<string> OrganizationIds { get; set; } = new List<string>();

    /// <summary>
    /// 角色Ids
    /// </summary>
    public IList<string> RoleIds { get; set; } = new List<string>();
}