namespace BasicPlatform.AppService.FreeSql.Commons;

/// <summary>
/// 租户服务
/// </summary>
[Component(LifeStyle.Singleton)]
public class TenantService : QueryServiceBase<Tenant>, ITenantService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="multiTenancy"></param>
    public TenantService(FreeSqlMultiTenancy multiTenancy) : base(multiTenancy)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tenantCode"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    /// <exception cref="FriendlyException"></exception>
    public async Task<TenantInfo?> GetAsync(string tenantCode, string? appId)
    {
        var result = await MainQueryableNoTracking
            .Where(p => p.Code == tenantCode)
            .FirstAsync(p => new TenantInfo
            {
                ConnectionString = p.ConnectionString ?? string.Empty,
                DbKey = p.Code,
                IsolationLevel = p.IsolationLevel
            });
        if (result == null || result.IsolationLevel == TenantIsolationLevel.Shared)
        {
            return result;
        }

        var connectionString = SecurityHelper.Decrypt(result.ConnectionString);
        if (connectionString == null)
        {
            throw FriendlyException.Of("租户连接字符串解密失败");
        }

        result.ConnectionString = connectionString;

        return result;
    }
}