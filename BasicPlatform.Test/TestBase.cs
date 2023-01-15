using System.Reflection;
using System.Security.Claims;
using Athena.Infrastructure.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
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
        services.AddCustomFreeSqlWithMySql(Configuration, new HostingEnvironment
        {
            EnvironmentName = "Development"
        }, aop =>
        {
            aop.CurdAfter += (_, e) =>
            {
                if (e.ElapsedMilliseconds > 200)
                {
                    Console.WriteLine($"执行SQL耗时 {e.ElapsedMilliseconds} ms");
                }
            };
        });
        services.AddCustomIntegrationEvent(Configuration);
        services.AddScoped<ISecurityContextAccessor, DefaultSecurityContextAccessor>();
        Provider = services.BuildServiceProvider();
        DbContext = Provider.GetService<IFreeSql>()!;
    }
}

public class DefaultSecurityContextAccessor : ISecurityContextAccessor
{
    public string CreateToken(JwtConfig config, List<Claim> claims, string scheme = "Bearer")
    {
        // throw new NotImplementedException();
        return string.Empty;
    }

    public string? AppId { get; }

    public string? MemberId { get; }
    public string? UserId => "63a4897bbd3497da92a27f5b";
    public string? RealName => "开发者";
    public string? UserName => "root";
    public bool IsRoot => true;
    public string? TenantId { get; }
    public string? Role { get; }
    public string? RoleName { get; }
    public bool IsRefreshCache { get; }
    public string JwtToken => string.Empty;
    public string JwtTokenNotBearer => string.Empty;
    public bool IsAuthenticated { get; }
    public string UserAgent => string.Empty;
    public string IpAddress => string.Empty;
}