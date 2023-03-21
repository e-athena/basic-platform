using UAParser;

namespace BasicPlatform.AppService.Users.Responses;

/// <summary>
/// 读取用户访问记录分页响应
/// </summary>
public class GetUserAccessRecordPagingResponse
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [MaxLength(36)]
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserRealName { get; set; }

    /// <summary>
    /// 访问时间
    /// </summary>
    public DateTime AccessTime { get; set; }

    /// <summary>
    /// 访问IP
    /// </summary>
    [MaxLength(50)]
    public string AccessIp { get; set; } = null!;

    /// <summary>
    /// 访问物理地址
    /// </summary>
    [MaxLength(200)]
    public string? AccessPhysicalAddress { get; set; }

    /// <summary>
    /// 访问地址
    /// </summary>
    [MaxLength(200)]
    public string AccessUrl { get; set; } = null!;

    /// <summary>
    /// 用户代理
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// 
    /// </summary>
    private ClientInfo? _userAgentInfo;

    /// <summary>
    /// 
    /// </summary>
    private ClientInfo? UserAgentInfo
    {
        get
        {
            if (_userAgentInfo != null)
            {
                return _userAgentInfo;
            }

            if (string.IsNullOrEmpty(UserAgent))
            {
                return null;
            }

            _userAgentInfo = Parser.GetDefault().Parse(UserAgent);
            return _userAgentInfo;
        }
    }

    /// <summary>
    /// 浏览器
    /// </summary>
    public string? Browser => UserAgentInfo?.UA.ToString();

    /// <summary>
    /// 操作系统
    /// </summary>
    public string? Os => UserAgentInfo?.OS.ToString();

    /// <summary>
    /// 设备
    /// </summary>
    public string? Device => UserAgentInfo?.Device.ToString();
}