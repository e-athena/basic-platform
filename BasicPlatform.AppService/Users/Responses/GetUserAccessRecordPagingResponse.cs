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
    /// 浏览器名称
    /// </summary>
    public string? BrowserName
    {
        get => UserAgentInfo?.UA.Family;
        set => value = UserAgentInfo?.UA.Family;
    }

    /// <summary>
    /// 浏览器版本号
    /// </summary>
    public string? BrowserVersion
    {
        get => $"{UserAgentInfo?.UA.Major}.{UserAgentInfo?.UA.Minor}.{UserAgentInfo?.UA.Patch}";
        set => value = $"{UserAgentInfo?.UA.Major}.{UserAgentInfo?.UA.Minor}.{UserAgentInfo?.UA.Patch}";
    }

    /// <summary>
    /// 操作系统名称
    /// </summary>
    public string? OsName
    {
        get => UserAgentInfo?.OS.Family;
        set => value = UserAgentInfo?.OS.Family;
    }

    /// <summary>
    /// 操作系统版本号
    /// </summary>
    public string? OsVersion
    {
        get => $"{UserAgentInfo?.OS.Major}.{UserAgentInfo?.OS.Minor}.{UserAgentInfo?.OS.Patch}";
        set => value = $"{UserAgentInfo?.OS.Major}.{UserAgentInfo?.OS.Minor}.{UserAgentInfo?.OS.Patch}";
    }
}