using System.Security.Claims;
using Athena.Infrastructure.Jwt;
using Athena.Infrastructure.Mvc.Messaging.Requests;
using Microsoft.Extensions.Options;

namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 帐户控制器
/// </summary>
[EnableCors]
[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ISecurityContextAccessor _securityContextAccessor;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="securityContextAccessor"></param>
    public AccountController(ISecurityContextAccessor securityContextAccessor)
    {
        _securityContextAccessor = securityContextAccessor;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<dynamic> LoginAsync(LoginRequest request)
    {
        var token = _securityContextAccessor.CreateToken(new List<Claim>
        {
            new(ClaimTypes.Name, request.UserName),
        });

        return await Task.FromResult(new
        {
            Status = "ok",
            Type = "account",
            CurrentAuthority = token,
        });
    }

    /// <summary>
    /// 当前用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<dynamic> CurrentUserAsync()
    {
        return await Task.FromResult(new
        {
            Id = 1,
            Avatar = "https://gw.alipayobjects.com/zos/antfincdn/XAosXuNZyF/BiazfanxmamNRoxxVxka.png",
            Email = "5fd2@c1xa4mje.com.cn",
            PhoneNumber = "13979442544",
            RealName = "锺国贤",
            UserName = "admin",
            Name = "锺国贤",
            Roles = "admin",
            Access = "admin"
        });
    }
}