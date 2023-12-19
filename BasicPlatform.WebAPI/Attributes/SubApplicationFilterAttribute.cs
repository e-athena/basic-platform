using System.Net;

namespace BasicPlatform.WebAPI.Attributes;

/// <summary>
/// 
/// </summary>
public class SubApplicationFilterAttribute : ActionFilterAttribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // 是否跳过权限验证
        var hasAllowAnonymous = context
            .ActionDescriptor
            .EndpointMetadata
            .Any(p =>
                p.GetType() == typeof(AllowAnonymousAttribute)
            );

        // 如果有跳过权限验证标签
        if (hasAllowAnonymous)
        {
            base.OnActionExecuting(context);
            return;
        }

        // 获取授权服务
        var contextAccessor = context
            .HttpContext
            .RequestServices
            .GetService(typeof(ISecurityContextAccessor)) as ISecurityContextAccessor;

        if (contextAccessor == null)
        {
            base.OnActionExecuting(context);
            return;
        }

        // 如果当前请求是内网请求，则跳过
        if (IsPrivateNetwork(context.HttpContext.Connection.RemoteIpAddress))
        {
            base.OnActionExecuting(context);
            return;
        }
        
        // 如果是dapr调用，则跳过
        if (contextAccessor.UserAgent.Contains("dapr-sdk-dotnet"))
        {
            base.OnActionExecuting(context);
            return;
        }

        // 检查是否已登录，如果使用默认配置，则有可能已经登录
        if (contextAccessor.IsAuthenticated)
        {
            base.OnActionExecuting(context);
            return;
        }

        // 读取配置
        var appService = context
            .HttpContext
            .RequestServices
            .GetService(typeof(IApplicationQueryService)) as IApplicationQueryService;

        if (appService == null)
        {
            throw new Exception("未找到应用服务");
        }

        // 读取密钥
        var securityKey = appService.GetSecret(contextAccessor.AppId!);

        if (securityKey == null)
        {
            throw new Exception("未找到应用信息");
        }

        // 验证Token
        var pass = contextAccessor.ValidateToken(new JwtConfig
        {
            AppId = contextAccessor.AppId,
            Issuer = "basic-platform-sso-center",
            Audience = contextAccessor.AppId!,
            SecurityKey = securityKey
        }, contextAccessor.JwtTokenNotBearer, out _);
        if (!pass)
        {
            context.Result = new JsonResult(new
            {
                StatusCode = 401,
                Message = "用户未授权"
            });
            context.HttpContext.Response.StatusCode = 401;
            base.OnActionExecuting(context);
            return;
        }

        base.OnActionExecuting(context);
    }
    private static bool IsPrivateNetwork(IPAddress? ip)
    {
        if (ip == null){
            return true;
        }
        // 如果是::1
        if (ip.ToString() == "::1")
        {
            return true;
        }
        var bytes = ip.GetAddressBytes();
        switch (ip.AddressFamily)
        {
            case System.Net.Sockets.AddressFamily.InterNetwork:
                if (bytes[0] == 10 || 
                    (bytes[0] == 172 && bytes[1] < 32 && bytes[1] >= 16) || 
                    (bytes[0] == 192 && bytes[1] == 168))
                    return true;
                break;
            case System.Net.Sockets.AddressFamily.InterNetworkV6:
                if (ip.IsIPv6SiteLocal || ip.IsIPv6LinkLocal)
                    return true;
                break;
        }
        return false;
    }
}