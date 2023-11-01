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
}