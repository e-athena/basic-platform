using BasicPlatform.Domain.Models.Tenants;

namespace BasicPlatform.AppService.FreeSql.Commons;

/// <summary>
/// 租户服务
/// </summary>
[Component(LifeStyle.Singleton)]
public class TenantService : ITenantService
{
    private readonly IFreeSql _freeSql;

    public TenantService(IFreeSql freeSql)
    {
        _freeSql = freeSql;
    }

    public async Task<TenantInfo?> GetAsync(string? tenantCode)
    {
        var result = await _freeSql.Queryable<Tenant>()
            .Where(p => p.Code == tenantCode)
            .FirstAsync(p => new TenantInfo
            {
                ConnectionString = p.ConnectionString,
                DbKey = p.Code,
                DataType = null
            });
        if (result == null)
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