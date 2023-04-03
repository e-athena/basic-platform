namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 重置用户密码请求类
/// </summary>
public class ResetUserPasswordRequest : ITxRequest<string>
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public string Id { get; set; } = null!;
}