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

    public TenantService(
        ISecurityContextAccessor accessor,
        ILoggerFactory loggerFactory,
        ICacheManager cacheManager) : base(accessor)
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
        var cacheKey = $"tenant:{tenantCode}";
        if (!string.IsNullOrEmpty(appId))
        {
            cacheKey += ":" + appId;
        }

        return await _cacheManager.GetOrCreateAsync(cacheKey, async () =>
        {
            const string url = "/api/Util/GetTenantInfo";
            var result = await GetRequestWithBasicAuth(url)
                .SetQueryParam("tenantCode", tenantCode)
                .SetQueryParam("appId", appId)
                .GetJsonAsync<ApiResult<TenantInfo>>();

            if (!result.Success)
            {
                throw FriendlyException.Of("读取租户信息失败", result.Message);
            }

            var data = result.Data;

            if (data == null)
            {
                throw FriendlyException.Of("读取租户信息失败");
            }

            if (data.IsolationLevel == TenantIsolationLevel.Shared)
            {
                return new TenantInfo
                {
                    ConnectionString = string.Empty,
                    DbKey = tenantCode,
                    IsolationLevel = data.IsolationLevel
                };
            }

            var connectionString = SecurityHelper.Decrypt(data.ConnectionString);
            if (connectionString != null)
            {
                return new TenantInfo
                {
                    ConnectionString = connectionString,
                    DbKey = tenantCode,
                    IsolationLevel = data.IsolationLevel
                };
            }

            _logger.LogWarning("租户连接字符串解密失败");
            throw FriendlyException.Of("读取租户信息失败");
        }, TimeSpan.FromDays(1));
    }
}