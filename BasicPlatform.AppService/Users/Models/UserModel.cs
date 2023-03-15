namespace BasicPlatform.AppService.Users.Models;

/// <summary>
/// 用户信息
/// </summary>
public class UserModel : ModelBase
{
    /// <summary>
    /// 用户名
    /// </summary>
    [TableColumn(Sort = 1, Width = 100, Required = true)]
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 密码
    /// </summary>
    [TableColumn(Ignore = true)]
    public string? Password { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [TableColumn(Sort = 0, Width = 50)]
    public string? Avatar { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [TableColumn(Sort = 2, Width = 100, Required = false)]
    public string RealName { get; set; } = null!;

    /// <summary>
    /// 性别
    /// </summary>
    [TableColumn(Sort = 3, Width = 80, Required = false)]
    public Gender Gender { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [TableColumn(Show = false, Sort = 8, Width = 100, Required = false)]
    public string? NickName { get; set; }


    /// <summary>
    /// 手机号
    /// </summary>
    [TableColumn(Sort = 5, Width = 115, Required = false)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    /// <value></value>
    [TableColumn(Show = false, Sort = 10, Width = 115, Required = false)]
    public string? Email { get; set; }

    /// <summary>
    /// 所属组织ID
    /// </summary>
    [TableColumn(Show = false)]
    public string OrganizationId { get; set; } = null!;

    /// <summary>
    /// 所属职位ID
    /// </summary>
    [TableColumn(Show = false)]
    public string PositionId { get; set; } = null!;

    /// <summary>
    /// 状态
    /// </summary>
    /// <value></value>
    [TableColumn(Sort = 9, Width = 90, Required = false)]
    public Status Status { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [TableColumn(Show=false,Ignore = true)]
    public bool IsEnabled => Status == Status.Enabled;

    /// <summary>
    /// 密码是否相等
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public bool PasswordEquals(string password)
    {
        return PasswordHash.ValidatePassword(password, Password ?? "");
    }
}