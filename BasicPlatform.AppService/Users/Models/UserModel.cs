namespace BasicPlatform.AppService.Users.Models;

/// <summary>
/// 用户信息
/// </summary>
public class UserModel : ViewModelBase
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string RealName { get; set; } = null!;

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
    /// 状态
    /// </summary>
    /// <value></value>
    public Status Status { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled => Status == Status.Enabled;

    /// <summary>
    /// 密码是否相等
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public bool PasswordEquals(string password)
    {
        return PasswordHash.ValidatePassword(password, Password);
    }
}