namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 添加用户访问记录
/// </summary>
public class AddUserAccessRecordRequest : ITxRequest<long>
{
    /// <summary>
    /// 访问地址
    /// </summary>
    public string? AccessUrl { get; set; }
}