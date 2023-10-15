namespace App.Infrastructure.Services.Impls;

/// <summary>
/// 默认服务基类
/// </summary>
public class DefaultServiceBase
{
    private readonly string? _basicAuthUserName;
    private readonly string? _basicAuthPassword;
    private readonly ISecurityContextAccessor _accessor;
    private readonly string _apiUrl = "http://localhost:5078";

    public DefaultServiceBase(ISecurityContextAccessor accessor)
    {
        _accessor = accessor;
        var config = AthenaProvider.Provider?.GetService(typeof(IConfiguration)) as IConfiguration;
        if (config == null)
        {
            return;
        }

        var url = config.GetSection("MainApplicationApiUrl").Value;
        if (url != null)
        {
            _apiUrl = url;
        }

        _basicAuthUserName = config.GetSection("BasicAuthConfig").GetValue<string>("UserName");
        _basicAuthPassword = config.GetSection("BasicAuthConfig").GetValue<string>("Password");
    }

    /// <summary>
    /// 获取请求对象
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    protected IFlurlRequest GetRequest(string url)
    {
        return $"{_apiUrl}{url}"
            .WithHeader("AppId", _accessor.AppId)
            .WithHeader("TenantId", _accessor.TenantId)
            .WithOAuthBearerToken(_accessor.JwtTokenNotBearer)
            .OnError(act =>
            {
                var res = act.Response;
                if (res != null)
                {
                    throw new FriendlyException(res.StatusCode, res.ResponseMessage.ReasonPhrase ?? "未知错误", url);
                }
            });
    }

    /// <summary>
    /// 获取请求对象
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    /// <exception cref="FriendlyException"></exception>
    protected IFlurlRequest GetRequestWithBasicAuth(string url)
    {
        if (_basicAuthUserName == null || _basicAuthPassword == null)
        {
            throw new FriendlyException("未配置BasicAuth");
        }

        return $"{_apiUrl}{url}"
            .WithBasicAuth(_basicAuthUserName, _basicAuthPassword)
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