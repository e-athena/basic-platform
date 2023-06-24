namespace BasicPlatform.WebAPI.Services.Impls;

/// <summary>
/// 默认服务基类
/// </summary>
public abstract class DefaultServiceBase
{
    private readonly BasicAuthConfig _config;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public DefaultServiceBase(IOptions<BasicAuthConfig> options)
    {
        _config = options.Value;
    }

    /// <summary>
    /// 获取请求对象
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    protected IFlurlRequest GetRequest(string url)
    {
        return url
            .WithBasicAuth(_config.UserName, _config.Password)
            .OnError(act =>
            {
                var res = act.Response;
                throw new FriendlyException(res.StatusCode, res.ResponseMessage.ReasonPhrase ?? "未知错误");
            });
    }
}