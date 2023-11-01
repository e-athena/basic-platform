using System.Reflection;
using System.Security.Claims;
using Athena.Infrastructure.Auths;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BasicPlatform.Test;

public class TestBase
{
    /// <summary>
    /// 
    /// </summary>
    protected IConfigurationRoot Configuration { get; private set; }

    /// <summary>
    /// 服务提供程序，用于获取服务实例
    /// </summary>
    protected IServiceProvider Provider { get; private set; }

    protected IFreeSql DbContext { get; private set; }

    protected T GetService<T>()
    {
        return Provider.GetService<T>()!;
    }

    /// <summary>
    /// 
    /// </summary>
    protected TestBase()
    {
        var services = new ServiceCollection();
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();
        services.AddHttpContextAccessor();
        services.AddLogging(configure => { configure.AddConsole(); });
        services.AddCustomMediatR(
            Assembly.Load("BasicPlatform.AppService.FreeSql")
        );
        services.AddCustomServiceComponent(
            Assembly.Load("BasicPlatform.AppService.FreeSql"),
            Assembly.Load("BasicPlatform.Infrastructure")
        );
        services.AddCustomCsRedisCache(Configuration);
        services.AddCustomFreeSql(Configuration, true);
        services.AddCustomIntegrationEvent(Configuration,
            new[] {Assembly.Load("BasicPlatform.ProcessManager")});
        services.AddScoped<ISecurityContextAccessor, DefaultSecurityContextAccessor>();
        Provider = services.BuildServiceProvider();
        DbContext = Provider.GetService<IFreeSql>()!;
    }
}

public class DefaultSecurityContextAccessor : ISecurityContextAccessor
{
    public string CreateToken(List<Claim> claims)
    {
        return string.Empty;
    }

    public string CreateToken(List<Claim> claims, string scheme)
    {
        return string.Empty;
    }

    public bool ValidateToken(JwtConfig config, string? token, out ClaimsPrincipal? principal)
    {
        throw new NotImplementedException();
    }

    public string CreateToken(JwtConfig config, List<Claim> claims, bool hasScheme = true, string scheme = "Bearer")
    {
        return string.Empty;
    }

    public string CreateTokenNotScheme(JwtConfig config, List<Claim> claims)
    {
        return string.Empty;
    }

    public string CreateTokenNotScheme(List<Claim> claims)
    {
        return string.Empty;
    }

    public string? AppId => null;
    public string MemberId => "63a4897bbd3497da92a27f5b";
    public string UserId => "63a4897bbd3497da92a27f5b";
    public string RealName => "开发者";
    public string UserName => "root";
    public bool IsRoot => true;
    public bool IsTenantAdmin => false;
    public string? TenantId => null;
    public string? Role => null;
    public IList<string>? Roles { get; } = null;
    public string? RoleName => null;
    public IList<string>? RoleNames { get; } = null;
    public bool IsRefreshCache => false;
    public string JwtToken => string.Empty;
    public string JwtTokenNotBearer => string.Empty;
    public bool IsAuthenticated => true;
    public string UserAgent => string.Empty;
    public string IpAddress => string.Empty;
}