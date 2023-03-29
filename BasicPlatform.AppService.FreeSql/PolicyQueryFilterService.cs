using Athena.Infrastructure;
using Athena.Infrastructure.QueryFilters;

namespace BasicPlatform.AppService.FreeSql;

/// <summary>
/// 策略查询过滤器服务实现类
/// </summary>
[Component(LifeStyle.Singleton)]
public class PolicyQueryFilterService : IQueryFilterService
{
    private readonly ICacheManager _cacheManager;

    public PolicyQueryFilterService(ICacheManager cacheManager)
    {
        _cacheManager = cacheManager;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public async Task<IList<QueryFilterGroup>?> GetAsync(string userId, Type type)
    {
        // Key
        var key = string.Format(CacheConstant.UserPolicyQueryKey, userId, type.Name);
        // 过期时间
        var expireTime = TimeSpan.FromMinutes(30);
        return await _cacheManager.GetOrCreateAsync(key, () =>
        {
            var result = new List<QueryFilterGroup>
            {
                new()
                {
                    XOR = "or",
                    Filters = new List<QueryFilter>
                    {
                        new()
                        {
                            Key = "XXX",
                            XOR = "and",
                            Operator = "contains",
                            Value = "1723"
                        },
                        new()
                        {
                            Key = "Password",
                            XOR = "and",
                            Operator = "contains",
                            Value = "4EIErJhVETg"
                        }
                    }
                }
            };
            return Task.FromResult(result);
        }, expireTime);
    }
}