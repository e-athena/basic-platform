using Athena.Infrastructure.Caching;

namespace App.Infrastructure.Services.Impls;

/// <summary>
/// 租户服务
/// </summary>
[Component]
public class TenantService : DefaultServiceBase, ITenantService
{
    private readonly ILogger<TenantService> _logger;
    private readonly ICacheManager _cacheManager;
    private const string ApiUrl = "http://localhost:5078";

    public TenantService(
        ISecurityContextAccessor accessor,
        ILoggerFactory loggerFactory,
        ICacheManager cacheManager
    ) : base(accessor)
    {
        _cacheManager = cacheManager;
        _logger = loggerFactory.CreateLogger<TenantService>();
    }

    /// <summary>
    /// 读取租户信息
    /// </summary>
    /// <param name="tenantCode"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    public async Task<TenantInfo?> GetAsync(string tenantCode, string? appId)
    {
        var cacheKey = $"tenant:connection-string:{tenantCode}";
        if (!string.IsNullOrEmpty(appId))
        {
            cacheKey += ":" + appId;
        }

        return await _cacheManager.GetOrCreateAsync(cacheKey, async () =>
        {
            const string url = $"{ApiUrl}/api/SubApplication/GetTenantConnectionString";
            var result = await GetRequest(url)
                .SetQueryParam("tenantCode", tenantCode)
                .SetQueryParam("appId", appId)
                .GetJsonAsync<ApiResult<string>>();

            if (!result.Success)
            {
                throw FriendlyException.Of("读取租户信息失败");
            }

            var connectionString = SecurityHelper.Decrypt(result.Data!);
            if (connectionString != null)
            {
                return new TenantInfo
                {
                    ConnectionString = connectionString,
                    DbKey = tenantCode
                };
            }

            _logger.LogWarning("租户连接字符串解密失败");
            throw FriendlyException.Of("读取租户信息失败");
        }, TimeSpan.FromDays(1));
    }
}