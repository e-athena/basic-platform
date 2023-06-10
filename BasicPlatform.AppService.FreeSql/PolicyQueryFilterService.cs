using Athena.Infrastructure.DataPermission.FreeSql;
using Microsoft.Extensions.Logging;

namespace BasicPlatform.AppService.FreeSql;

/// <summary>
/// 策略查询过滤器服务实现类
/// </summary>
[Component(LifeStyle.Singleton)]
public class PolicyQueryFilterService : QueryFilterServiceBase, IQueryFilterService
{
    private readonly ICacheManager _cacheManager;
    private readonly ILogger<PolicyQueryFilterService> _logger;


    public PolicyQueryFilterService(
        ICacheManager cacheManager,
        ILoggerFactory loggerFactory,
        IFreeSql freeSql,
        IEnumerable<IDataPermission> dataPermissions,
        IDataPermissionService dataPermissionService
    ) : base(freeSql, dataPermissions, dataPermissionService)
    {
        _cacheManager = cacheManager;
        _logger = loggerFactory.CreateLogger<PolicyQueryFilterService>();
    }

    /// <summary>
    /// 读取查询过滤器
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public async Task<IList<QueryFilterGroup>?> GetAsync(string userId, Type type)
    {
        var resourceKey = type.Name;
        string? appId = null;
        // 缓存Key
        var cacheKey = string.Format(CacheConstant.UserPolicyQueryKey, userId, resourceKey);
        // 缓存过期时间
        var expireTime = TimeSpan.FromMinutes(30);
        // 读取缓存，如果缓存中存在，则直接返回，否则从数据库中读取，并写入缓存
        var result = await _cacheManager.GetOrCreateAsync(
            cacheKey,
            async () => await GetQueryFilterGroupList(userId, resourceKey, appId),
            expireTime) ?? new List<QueryFilterGroup>();
        foreach (var group in result)
        {
            // 检查是否存在对应的字段
            foreach (var newFilter in group.Filters)
            {
                var property = type.GetProperty(newFilter.Key);
                if (property == null)
                {
                    _logger.LogWarning("字段{Key}不存在", newFilter.Key);
                }
            }
        }

        return result;
    }
}
//
// [Component]
// public class TestIdDataPermission : AppQueryServiceBase<Position>, IDataPermission
// {
//     public TestIdDataPermission(IFreeSql freeSql, ISecurityContextAccessor accessor) : base(freeSql, accessor)
//     {
//     }
//
//     public string Label => "职位Id";
//     public string Key => "Id";
//     public string Value => "{TestId}";
//
//     public string GetSqlString()
//     {
//         return QueryNoTracking()
//             .Where(p => p.CreatedUserId == UserId)
//             .ToSql(p => new
//             {
//                 p.Id,
//                 MapKey = $"{Key},{Value}"
//             }, FieldAliasOptions.AsProperty);
//     }
// }
//
// [Component]
// public class TestNameDataPermission : AppQueryServiceBase<Position>, IDataPermission
// {
//     public TestNameDataPermission(IFreeSql freeSql, ISecurityContextAccessor accessor) : base(freeSql, accessor)
//     {
//     }
//
//     public string Label => "组织架构ID";
//     public string Key => "OrganizationId11";
//     public string Value => "{TestOrganizationId}";
//
//     public string GetSqlString()
//     {
//         return QueryNoTracking<Organization>()
//             .Where(p => p.CreatedUserId == "63a4897bbd3497da92a27f5b")
//             .ToSql(p => new
//             {
//                 p.Id,
//                 MapKey = $"{Key},{Value}"
//             }, FieldAliasOptions.AsProperty);
//     }
// }