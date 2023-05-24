using System.Security.Claims;
using BasicPlatform.AppService.Applications;
using Microsoft.AspNetCore.Mvc.Filters;

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
        }, contextAccessor.JwtTokenNotBearer, out var principal);
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
        // // 读取子应用的用户信息，然后创建用于访问主应用的Token
        // // 获取缓存管理器
        // var cacheManager = context
        //     .HttpContext
        //     .RequestServices
        //     .GetService(typeof(ICacheManager)) as ICacheManager;
        // // 缓存key
        // var cacheKey = $"{contextAccessor.AppId}:{principal!.FindFirstValue(ClaimTypes.NameIdentifier)}";
        // // 读取缓存
        // var token = cacheManager?.GetOrCreate(cacheKey, () =>
        // {
        //     // Claims
        //     var claims = new List<Claim>
        //     {
        //         new(ClaimTypes.NameIdentifier, principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty),
        //         new(ClaimTypes.Name, principal.FindFirstValue(ClaimTypes.Name) ?? string.Empty),
        //     };
        //     var role = principal.FindFirstValue(ClaimTypes.Role);
        //     if (!string.IsNullOrEmpty(role))
        //     {
        //         claims.Add(new Claim(ClaimTypes.Role, role));
        //     }
        //
        //     var roleName = principal.FindFirstValue("RoleName");
        //     if (!string.IsNullOrEmpty(roleName))
        //     {
        //         claims.Add(new Claim("RoleName", roleName));
        //     }
        //
        //     var realName = principal.FindFirstValue("RealName");
        //     if (!string.IsNullOrEmpty(realName))
        //     {
        //         claims.Add(new Claim("RealName", realName));
        //     }
        //
        //     // 创建Token
        //     var token = contextAccessor.CreateToken(claims);
        //     return token;
        // }, TimeSpan.FromMinutes(30));
        // // 设置到请求头Authorization中
        // context.HttpContext.Request.Headers["Authorization"] = token;
        base.OnActionExecuting(context);
    }
}