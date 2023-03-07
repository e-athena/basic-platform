namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 更新用户登录信息请求
/// </summary>
public class UpdateUserLoginInfoRequest : ITxRequest<bool>
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public string Id { get; set; } = null!;
}