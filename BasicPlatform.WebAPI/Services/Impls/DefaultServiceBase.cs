namespace BasicPlatform.WebAPI.Services.Impls;

/// <summary>
/// 默认服务基类
/// </summary>
public abstract class DefaultServiceBase
{
    private readonly IOptionsMonitor<BasicAuthConfig> _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public DefaultServiceBase(IOptionsMonitor<BasicAuthConfig> options)
    {
        _options = options;
    }

    /// <summary>
    /// 获取请求对象
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    protected IFlurlRequest GetRequest(string url)
    {
        // return url
        //     .WithBasicAuth(_config.UserName, _config.Password);
        return url
            .WithTimeout(30)
            .WithBasicAuth(_options.CurrentValue.UserName, _options.CurrentValue.Password)
            .OnError(act =>
            {
                var res = act.Response;
                if (res != null)
                {
                    throw new FriendlyException(res.StatusCode, res.ResponseMessage.ReasonPhrase ?? "未知错误", url);
                }
            });
    }
}