using BasicPlatform.Domain.Models.Organizations;
using BasicPlatform.Domain.Models.Roles;
using BasicPlatform.Domain.Models.Users;

namespace BasicPlatform.AppService.FreeSql.Commons;

/// <summary>
/// 数据权限服务
/// </summary>
[Component(LifeStyle.Singleton)]
public class DefaultDataPermissionService : IDataPermissionService
{
    private readonly IFreeSql _freeSql;
    private readonly ICacheManager? _cacheManager;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="freeSql"></param>
    public DefaultDataPermissionService(IFreeSql freeSql)
    {
        _freeSql = freeSql;
        _cacheManager = AthenaProvider.Provider?.GetService(typeof(ICacheManager)) as ICacheManager;
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
        if (_cacheManager == null)
        {
            return await QueryFunc();
        }

        // Key
        var key = string.Format(CacheConstant.UserPolicyFilterGroupQueryKey, userId, resourceKey);
        // 过期时间
        var expireTime = TimeSpan.FromMinutes(30);

        return await _cacheManager.GetOrCreateAsync(key, QueryFunc, expireTime) ?? new List<QueryFilterGroup>();

        async Task<List<QueryFilterGroup>> QueryFunc()
        {
            // 组装查询过滤器
            var result = new List<QueryFilterGroup>();

            // 读取用户配置的策略
            var userPolicies = await _freeSql.Select<UserDataPermission>()
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

            var userPolicyResourceKeys = userPolicies
                .Select(p => p.PolicyResourceKey)
                .ToList();

            // 读取用户角色配置的策略
            var rolePolicies = await _freeSql.Select<RoleDataPermission>()
                .Where(p => _freeSql.Select<RoleUser>()
                    .As("c")
                    .Where(c => c.UserId == userId)
                    .ToList(c => c.RoleId)
                    .Contains(p.RoleId)
                )
                .Where(p => p.Enabled)
                .Where(p => p.PolicyResourceKey == resourceKey)
                // 如果已经在用户策略中配置了，则不再读取角色策略
                .WhereIf(userPolicyResourceKeys.Count > 0,
                    p => !userPolicyResourceKeys.Contains(p.PolicyResourceKey)
                )
                .ToListAsync();
            // 角色
            foreach (var rolePolicy in rolePolicies)
            {
                result.AddRange(rolePolicy.Policies);
            }

            return result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    public List<string> GetUserOrganizationIds(string userId, string? appId)
    {
        return GetUserOrganizationIdsAsync(userId, appId).ConfigureAwait(false).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 获取用户所在组织/部门列表
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    public async Task<List<string>> GetUserOrganizationIdsAsync(string userId, string? appId)
    {
        if (_cacheManager == null)
        {
            return await QueryFunc();
        }

        // Key
        var key = string.Format(CacheConstant.UserOrganizationKey, userId);
        // 过期时间
        var expireTime = TimeSpan.FromMinutes(30);

        return await _cacheManager.GetOrCreateAsync(key, QueryFunc, expireTime) ?? new List<string>();

        async Task<List<string>> QueryFunc()
        {
            var result = new List<string>();

            // 用户组织
            var orgId = _freeSql.Select<User>()
                .Where(p => p.Id == userId)
                .First(p => p.OrganizationId);

            result.Add(orgId);

            // 兼任职信息表
            var orgIds = await _freeSql.Select<UserAppointment>()
                .Where(p => p.UserId == userId)
                .ToListAsync(p => p.OrganizationId);

            result.AddRange(orgIds);

            return result;
        }
    }

    /// <summary>
    /// 获取用户所有组织/部门及下级组织/部门列表
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    public List<string> GetUserOrganizationIdsTree(string userId, string? appId)
    {
        return GetUserOrganizationIdsTreeAsync(userId, appId).ConfigureAwait(false).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 获取用户所有组织/部门及下级组织/部门列表
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    public async Task<List<string>> GetUserOrganizationIdsTreeAsync(string userId, string? appId)
    {
        if (_cacheManager == null)
        {
            return await QueryFunc();
        }

        // Key
        var key = string.Format(CacheConstant.UserOrganizationsKey, userId);
        // 过期时间
        var expireTime = TimeSpan.FromMinutes(30);
        return await _cacheManager.GetOrCreateAsync(key, QueryFunc, expireTime) ?? new List<string>();

        async Task<List<string>> QueryFunc()
        {
            // 查询用户所在的组织
            var list = await GetUserOrganizationIdsAsync(userId, appId);

            if (list.Count == 0)
            {
                return list;
            }

            var filters = list.Select(p => new QueryFilter
            {
                Key = "ParentPath",
                Operator = "contains",
                Value = p,
                XOR = "or"
            }).ToList();
            // 生成查询条件
            var filterWhere = filters.MakeFilterWhere<Organization>(false);
            // 查询用户组织架构的下级组织
            var orgIds = _freeSql.Select<Organization>()
                .Where(filterWhere)
                .ToList(p => p.Id);
            list.AddRange(orgIds);

            // 数据去重
            return list.GroupBy(p => p).Select(p => p.Key).ToList();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    public List<DataPermission> GetUserDataScopes(string userId, string? appId)
    {
        return GetUserDataScopesAsync(userId, appId).ConfigureAwait(false).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    public async Task<List<DataPermission>> GetUserDataScopesAsync(string userId, string? appId)
    {
        if (_cacheManager == null)
        {
            return await QueryFunc();
        }

        // Key
        var key = string.Format(CacheConstant.UserDataScopesKey, userId);
        // 过期时间
        var expireTime = TimeSpan.FromMinutes(30);
        return await _cacheManager.GetOrCreateAsync(key, QueryFunc, expireTime) ?? new List<DataPermission>();

        async Task<List<DataPermission>> QueryFunc()
        {
            var dataScopeList = await _freeSql.Select<Role>()
                .Where(p => _freeSql
                    .Select<RoleUser>()
                    .As("c")
                    .Where(c => c.UserId == userId)
                    .Any(c => c.RoleId == p.Id)
                )
                .ToListAsync(p => new
                {
                    p.DataScope,
                    p.DataScopeCustom
                });

            // 去重
            dataScopeList = dataScopeList
                .GroupBy(p => p)
                .Select(p => p.Key)
                .ToList();

            // 读取用户的角色数据权限
            var list = await _freeSql.Select<RoleDataPermission>()
                // 读取用户的角色
                .Where(p => _freeSql
                    .Select<RoleUser>()
                    .As("c")
                    .Where(c => c.UserId == userId)
                    .Any(c => c.RoleId == p.RoleId)
                )
                // 启用的
                .Where(p => p.Enabled)
                .ToListAsync(p => new DataPermission
                {
                    ResourceKey = p.ResourceKey,
                    DataScope = p.DataScope
                });

            // 读取用户的数据权限
            var userPermissionList = await _freeSql.Select<UserDataPermission>()
                .Where(p => p.UserId == userId)
                // 启用的
                .Where(p => p.Enabled)
                // 读取未过期的
                .Where(p => p.ExpireAt == null || p.ExpireAt > DateTime.Now)
                .ToListAsync(p => new DataPermission
                {
                    ResourceKey = p.ResourceKey,
                    DataScope = p.DataScope
                });

            // 以用户的为准，因为可对用户进行个性化设置
            foreach (var item in userPermissionList)
            {
                // 查询
                var single = list
                    .Where(p => p.DataScope != item.DataScope)
                    .FirstOrDefault(p => p.ResourceKey == item.ResourceKey);
                if (single == null)
                {
                    list.Add(item);
                    continue;
                }

                single.DataScope = item.DataScope;
                single.DataScopeCustom = item.DataScopeCustom;
            }

            // 去重
            list = list
                .GroupBy(p => p.ResourceKey)
                .Select(p => p.First())
                .ToList();

            // 添加通用的数据范围
            foreach (var item in dataScopeList)
            {
                list.Add(new DataPermission
                {
                    DataScope = item.DataScope
                });
            }

            return list;
        }
    }
}