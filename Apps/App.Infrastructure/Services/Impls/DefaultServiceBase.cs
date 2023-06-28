namespace App.Infrastructure.Services.Impls;

/// <summary>
/// 默认服务基类
/// </summary>
public class DefaultServiceBase
{
    private readonly ISecurityContextAccessor _accessor;
    private readonly string _apiUrl = "http://localhost:5078";

    public DefaultServiceBase(ISecurityContextAccessor accessor)
    {
        _accessor = accessor;
        var url = (AthenaProvider.Provider?.GetService(typeof(IConfiguration)) as IConfiguration)?
            .GetSection("MainApplicationApiUrl").Value;
        if (url != null)
        {
            _apiUrl = url;
        }
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
                throw new FriendlyException(res.StatusCode, res.ResponseMessage.ReasonPhrase ?? "未知错误");
            });
    }
}