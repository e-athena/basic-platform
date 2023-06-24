namespace App.Infrastructure.Services.Impls;

/// <summary>
/// 默认服务基类
/// </summary>
public class DefaultServiceBase
{
    private readonly ISecurityContextAccessor _accessor;

    public DefaultServiceBase(ISecurityContextAccessor accessor)
    {
        _accessor = accessor;
    }

    /// <summary>
    /// 获取请求对象
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    protected IFlurlRequest GetRequest(string url)
    {
        return url
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