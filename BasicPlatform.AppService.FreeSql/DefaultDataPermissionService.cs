using Athena.Infrastructure.QueryFilters;
using BasicPlatform.AppService.DataPermissions;

namespace BasicPlatform.AppService.FreeSql;

/// <summary>
/// 数据权限服务
/// </summary>
[Component(LifeStyle.Singleton)]
public class DefaultDataPermissionService : IDataPermissionService
{
    private readonly IFreeSql _freeSql;
    private readonly ICacheManager? _cacheManager;

    public DefaultDataPermissionService(IFreeSql freeSql)
    {
        _freeSql = freeSql;
        _cacheManager = ServiceLocator.Instance?.GetService(typeof(ICacheManager)) as ICacheManager;
    }

    /// <summary>
    /// 读取用户已有的策略查询过滤器组列表
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="resourceKey"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    public async Task<List<QueryFilterGroup>> GetPolicyQueryFilterGroupsAsync(
        string userId,
        string resourceKey,
        string? appId)
    {
        async Task<List<QueryFilterGroup>> QueryFunc()
        {
            // 组装查询过滤器
            var result = new List<QueryFilterGroup>();

            // 读取用户配置的策略
            var userPolicies = await _freeSql.Select<UserDataQueryPolicy>()
                .Where(p => p.UserId == userId)
                // 启用的
                .Where(p => p.Enabled)
                .Where(p => p.PolicyResourceKey == resourceKey)
                // 未过期的
                .Where(p => p.ExpireAt == null || p.ExpireAt > DateTime.Now)
                .ToListAsync();

            // 用户
            foreach (var userPolicy in userPolicies)
            {
                result.AddRange(userPolicy.Policies);
            }

            // 读取用户角色配置的策略
            var rolePolicies = await _freeSql.Select<RoleDataQueryPolicy>()
                .Where(p => _freeSql.Select<RoleUser>()
                    .As("c")
                    .Where(c => c.UserId == userId)
                    .ToList(c => c.RoleId)
                    .Contains(p.RoleId)
                )
                .Where(p => p.Enabled)
                .Where(p => p.PolicyResourceKey == resourceKey)
                // 如果已经在用户策略中配置了，则不再读取角色策略
                .Where(p => !userPolicies
                    .Select(c => c.PolicyResourceKey)
                    .Contains(p.PolicyResourceKey)
                )
                .ToListAsync();
            // 角色
            foreach (var rolePolicy in rolePolicies)
            {
                result.AddRange(rolePolicy.Policies);
            }

            return result;
        }

        if (_cacheManager == null)
        {
            return await QueryFunc();
        }

        // Key
        var key = string.Format(CacheConstant.UserPolicyFilterGroupQuery, userId, resourceKey);
        // 过期时间
        var expireTime = TimeSpan.FromMinutes(30);

        return await _cacheManager.GetOrCreateAsync(key, QueryFunc, expireTime) ?? new List<QueryFilterGroup>();
    }

    /// <summary>
    /// 获取用户所在组织/部门列表
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    public async Task<List<string>> GetUserOrganizationIdsAsync(string userId, string? appId)
    {
        async Task<List<string>> QueryFunc()
        {
            // 兼任职信息表
            var orgIds = await _freeSql.Select<UserAppointment>()
                .Where(p => p.UserId == userId)
                .ToListAsync(p => p.OrganizationId);

            // 用户组织
            var orgId = _freeSql.Select<User>()
                .Where(p => p.Id == userId)
                .First(p => p.OrganizationId);

            if (!string.IsNullOrEmpty(orgId))
            {
                orgIds.Add(orgId);
            }

            return orgIds;
        }

        if (_cacheManager == null)
        {
            return await QueryFunc();
        }

        // Key
        var key = string.Format(CacheConstant.UserOrganizationKey, userId);
        // 过期时间
        var expireTime = TimeSpan.FromMinutes(30);

        return await _cacheManager.GetOrCreateAsync(key, QueryFunc, expireTime) ?? new List<string>();
    }

    /// <summary>
    /// 获取用户所有组织/部门及下级组织/部门列表
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    public async Task<List<string>> GetUserOrganizationIdsTreeAsync(string userId, string? appId)
    {
        async Task<List<string>> QueryFunc()
        {
            // 查询用户所在的组织
            var list = await GetUserOrganizationIdsAsync(userId, appId);

            if (list.Count == 0)
            {
                return list;
            }

            var filters = list.Select(p => new QueryFilter
                {Key = "ParentPath", Operator = "contains", Value = p, XOR = "or"}).ToList();
            // 生成查询条件
            var filterWhere = QueryableExtensions.MakeFilterWhere<Organization>(filters, false);
            // 查询用户组织架构的下级组织
            var orgIds = _freeSql.Select<Organization>()
                .Where(filterWhere)
                .ToList(p => p.Id);
            list.AddRange(orgIds);

            // 数据去重
            return list.GroupBy(p => p).Select(p => p.Key).ToList();
        }

        if (_cacheManager == null)
        {
            return await QueryFunc();
        }

        // Key
        var key = string.Format(CacheConstant.UserOrganizationsKey, userId);
        // 过期时间
        var expireTime = TimeSpan.FromMinutes(30);
        return await _cacheManager.GetOrCreateAsync(key, QueryFunc, expireTime) ?? new List<string>();
    }
}