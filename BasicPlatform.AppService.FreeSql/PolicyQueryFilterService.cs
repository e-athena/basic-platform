using Athena.Infrastructure.QueryFilters;
using BasicPlatform.AppService.DataPermissions;
using Microsoft.Extensions.Logging;

namespace BasicPlatform.AppService.FreeSql;

/// <summary>
/// 策略查询过滤器服务实现类
/// </summary>
[Component(LifeStyle.Singleton)]
public class PolicyQueryFilterService : IQueryFilterService
{
    private readonly ICacheManager _cacheManager;
    private readonly IFreeSql _freeSql;
    private readonly ILogger<PolicyQueryFilterService> _logger;
    private readonly DataPermissionFactory _dataPermissionFactory;
    private readonly IDataPermissionService _dataPermissionService;

    public PolicyQueryFilterService(
        ICacheManager cacheManager, IFreeSql freeSql,
        ILoggerFactory loggerFactory,
        IEnumerable<IDataPermission> dataPermissions,
        IDataPermissionService dataPermissionService)
    {
        _cacheManager = cacheManager;
        _freeSql = freeSql;
        _dataPermissionService = dataPermissionService;
        _logger = loggerFactory.CreateLogger<PolicyQueryFilterService>();
        _dataPermissionFactory = new DataPermissionFactory(dataPermissions);
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
        var result = await _cacheManager.GetOrCreateAsync(cacheKey, async () =>
        {
            var result = await _dataPermissionService.GetPolicyQueryFilterGroupsAsync(userId, resourceKey, appId);
            if (result.Count == 0)
            {
                return new List<QueryFilterGroup>();
            }

            // 基础权限列表
            var basicList = new List<string>
            {
                // 当前登录用户
                "{SelfUserId}",
                // 当前登录用户所在组织
                "{SelfOrganizationId}",
                // 当前登录用户所在组织的下级组织
                "{SelfOrganizationChildrenIds}",
            };
            var hasSelfOrganizationId = false;
            var hasSelfOrganizationChildrenIds = false;
            var extraSqlList = new List<string>();

            // 处理占位符
            foreach (var group in result)
            {
                foreach (var filter in group.Filters)
                {
                    // 如果包含基础的占位符
                    if (basicList.Any(key => key == filter.Value))
                    {
                        switch (filter.Value)
                        {
                            // 当前登录人
                            case "{SelfUserId}":
                                filter.Value = userId;
                                break;
                            // 当前登录人部门
                            case "{SelfOrganizationId}":
                                hasSelfOrganizationId = true;
                                break;
                            // 当前登录人部门及下级部门
                            case "{SelfOrganizationChildrenIds}":
                                hasSelfOrganizationChildrenIds = true;
                                break;
                        }

                        continue;
                    }

                    // 动态查询条件
                    var dataPermission = _dataPermissionFactory.GetInstances()
                        .Where(p => p.Key == filter.Key)
                        .FirstOrDefault(p => p.Value == filter.Value);
                    if (dataPermission == null)
                    {
                        continue;
                    }

                    // 获取查询的SQL
                    extraSqlList.Add(dataPermission.GetSqlString());
                }
            }

            // 有本部门查询条件
            IList<string>? selfOrganizationIds = null;
            // 有本部门及下级部门查询条件
            IList<string>? selfOrganizationChildrenIds = null;
            // 读取基本权限
            if (hasSelfOrganizationId)
            {
                selfOrganizationIds = await _dataPermissionService.GetUserOrganizationIdsAsync(userId, appId);
            }

            if (hasSelfOrganizationChildrenIds)
            {
                selfOrganizationChildrenIds =
                    await _dataPermissionService.GetUserOrganizationIdsTreeAsync(userId, appId);
            }

            List<(string Id, string MapKey)>? dynamicList = null;
            if (extraSqlList.Count > 0)
            {
                // 构建查询表达式
                var query = _freeSql.Queryable<object>();
                foreach (var sql in extraSqlList)
                {
                    query = query.WithSql(sql);
                }

                // 从数据库中读取动态列表，as1为值，as2为Key
                dynamicList = await query.ToListAsync<(string, string)>("Id,MapKey");
            }

            var newResult = new List<QueryFilterGroup>();
            // 处理占位符数据
            foreach (var group in result)
            {
                var newGroup = new QueryFilterGroup
                {
                    XOR = group.XOR,
                };
                var newFilters = new List<QueryFilter>();
                foreach (var filter in group.Filters)
                {
                    switch (filter)
                    {
                        // 设置了组织权限且查询到了数据
                        case {Value: "{SelfOrganizationId}", Key: "OrganizationId"} when selfOrganizationIds != null:
                            // 展开查询
                            newFilters.AddRange(selfOrganizationIds
                                .Select(organizationId => new QueryFilter
                                {
                                    Key = "OrganizationalUnitIds",
                                    Operator = "contains",
                                    Value = organizationId,
                                    XOR = "or"
                                }));
                            continue;
                        // 设置了组织权限，但是没有组织数据
                        case {Value: "{SelfOrganizationId}", Key: "OrganizationId"}:
                            newFilters.Add(new QueryFilter
                            {
                                Key = "OrganizationalUnitIds",
                                Operator = "==",
                                Value = "false",
                                XOR = "and"
                            });
                            continue;
                        // 设置了组织权限且查询到了数据
                        case {Value: "{SelfOrganizationChildrenIds}", Key: "OrganizationId"}
                            when selfOrganizationChildrenIds != null:
                            newFilters.AddRange(selfOrganizationChildrenIds
                                .Select(organizationId => new QueryFilter
                                {
                                    Key = "OrganizationalUnitIds",
                                    Operator = "contains",
                                    Value = organizationId,
                                    XOR = "or"
                                }));
                            continue;
                        // 设置了组织权限，但是没有组织数据
                        case {Value: "{SelfOrganizationChildrenIds}", Key: "OrganizationId"}:
                            newFilters.Add(new QueryFilter
                            {
                                Key = "OrganizationalUnitIds",
                                Operator = "==",
                                Value = "false",
                                XOR = "and"
                            });
                            continue;
                    }

                    if (dynamicList == null)
                    {
                        newFilters.Add(filter);
                        continue;
                    }

                    var mapKey = $"{filter.Key},{filter.Value}";
                    // 分组
                    var groupList = dynamicList.Where(p => p.MapKey == mapKey).ToList();
                    if (groupList.Count == 0)
                    {
                        newFilters.Add(filter);
                        continue;
                    }

                    // 去重赋值
                    var list = groupList.Select(p => p.Id).Distinct().ToList();
                    filter.Value = string.Join(",", list);
                    newFilters.Add(filter);
                }

                newGroup.Filters = newFilters;
                newResult.Add(newGroup);
            }

            return newResult;
        }, expireTime) ?? new List<QueryFilterGroup>();
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