using System.Linq.Expressions;
using Athena.Infrastructure.QueryFilters;
using BasicPlatform.AppService.DataPermissions.Models;
using BasicPlatform.Infrastructure.Enums;

namespace BasicPlatform.AppService.FreeSql.Commons;

/// <summary>
/// 数据权限查询服务基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class DataPermissionQueryServiceBase<T> : QueryServiceBase<T> where T : FullEntityCore, new()
{
    private readonly ISecurityContextAccessor _accessor;
    private readonly IFreeSql _freeSql;
    private readonly ICacheManager? _cacheManager;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="freeSql"></param>
    /// <param name="accessor"></param>
    public DataPermissionQueryServiceBase(
        IFreeSql freeSql,
        ISecurityContextAccessor accessor
    ) :
        base(freeSql)
    {
        _freeSql = freeSql;
        _accessor = accessor;
        _cacheManager = ServiceLocator.Instance?.GetService(typeof(ICacheManager)) as ICacheManager;
    }

    /// <summary>
    /// 跳过权限查询
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    protected ISelect<T1> QuerySkipPermission<T1>() where T1 : class, new()
    {
        return _freeSql.Select<T1>().NoTracking();
    }

    /// <summary>
    /// 查询对象
    /// </summary>
    protected override ISelect<T> Queryable => QueryWithPermission<T>();

    /// <summary>
    /// 查询对象
    /// </summary>
    protected override ISelect<T> QueryableNoTracking => QueryNoTrackingWithPermission<T>();

    /// <summary>
    /// 查询对象
    /// </summary>
    /// <returns></returns>
    protected override ISelect<T> Query()
    {
        return QueryWithPermission<T>();
    }

    /// <summary>
    /// 查询对象
    /// </summary>
    /// <returns></returns>
    protected override ISelect<T> QueryNoTracking()
    {
        return QueryNoTrackingWithPermission<T>();
    }

    /// <summary>
    /// 查询对象
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    protected override ISelect<T1> Query<T1>()
    {
        return QueryWithPermission<T1>();
    }

    /// <summary>
    /// 查询对象
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    protected override ISelect<T1> QueryNoTracking<T1>()
    {
        return QueryNoTrackingWithPermission<T1>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    private ISelect<T1> QueryWithPermission<T1>() where T1 : class
    {
        var query = _freeSql.Queryable<T1>();
        return QueryWithPermission(query);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    private ISelect<T1> QueryNoTrackingWithPermission<T1>() where T1 : class
    {
        var query = _freeSql.Queryable<T1>().NoTracking();
        return QueryWithPermission(query);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    private ISelect<T1> QueryWithPermission<T1>(ISelect<T1> query) where T1 : class
    {
        // 如果是开发者帐号。则不需要过滤
        if (IsRoot)
        {
            return query;
        }

        // 数据访问范围
        var dataScopeList = GetUserDataScopes();
        // 读取当前模块的数据访问范围
        var dataScope = dataScopeList
            .FirstOrDefault(p => typeof(T1).Name == p.ResourceKey);
        // 如果该模块有全部数据的权限则不需要过滤
        if (dataScope == null)
        {
            var emptyResourceKeyDataPermissions = dataScopeList
                .Where(p => string.IsNullOrEmpty(p.ResourceKey))
                .Select(p => new DataPermission
                {
                    DataScope = p.DataScope,
                    DataScopeCustom = p.DataScopeCustom
                })
                .ToList();

            // 查询通用设置，如果包含全部的数据。则不需要过滤
            if (emptyResourceKeyDataPermissions.Any(dp => dp.DataScope == RoleDataScope.All))
            {
                return query;
            }

            var filterWhere1 = GenerateFilterWhere<T1>(emptyResourceKeyDataPermissions);
            // 如果没有任何数据权限，则返回空
            return query.Where(filterWhere1 ?? (p => false));
        }

        // 当前模块有全部数据的权限则不需要过滤
        if (dataScope.DataScope == RoleDataScope.All)
        {
            return query;
        }

        var dataPermissions = new List<DataPermission>
        {
            dataScope
        };
        var filterWhere = GenerateFilterWhere<T1>(dataPermissions);
        // 如果没有任何数据权限，则返回空
        return query.Where(filterWhere ?? (p => false));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    private static bool HasProperty<T1>(string propertyName)
    {
        return typeof(T1).GetProperties().Any(p => p.Name == propertyName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataPermissions"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    private Expression<Func<TResult, bool>>? GenerateFilterWhere<TResult>(ICollection<DataPermission> dataPermissions)
    {
        if (dataPermissions.Count == 0)
        {
            return null;
        }

        var filters = new List<QueryFilter>();
        var organizationIds = new List<string>();
        foreach (var data in dataPermissions)
        {
            if (data.DataScope == RoleDataScope.Self)
            {
                if (!HasProperty<TResult>("CreatedUserId"))
                {
                    continue;
                }

                filters.Add(new QueryFilter
                {
                    Key = "CreatedUserId",
                    Operator = "==",
                    Value = UserId!,
                    XOR = "or"
                });
                continue;
            }

            if (!HasProperty<TResult>("OrganizationalUnitIds"))
            {
                continue;
            }

            var orgIds = data.DataScope switch
            {
                RoleDataScope.Department => GetUserOrganizationIds(),
                RoleDataScope.DepartmentAndSub => GetUserOrganizationIdsTree(),
                RoleDataScope.Custom => data.DataScopeCustoms,
                _ => null
            };

            if (orgIds == null || orgIds.Count == 0)
            {
                continue;
            }

            organizationIds.AddRange(orgIds);
        }

        if (organizationIds.Count <= 0)
        {
            return QueryableExtensions.MakeFilterWhere<TResult>(filters, false);
        }

        // 去重
        organizationIds = organizationIds.GroupBy(p => p).Select(p => p.Key).ToList();

        foreach (var orgId in organizationIds)
        {
            filters.Add(new QueryFilter
            {
                Key = "OrganizationalUnitIds",
                Operator = "contains",
                Value = orgId,
                XOR = "or"
            });
        }

        return QueryableExtensions.MakeFilterWhere<TResult>(filters, false);
    }

    /// <summary>
    /// 用户ID
    /// </summary>
    protected string? UserId => _accessor.UserId;

    /// <summary>
    /// 是否为开发者帐号
    /// </summary>
    protected bool IsRoot => _accessor.IsRoot;

    /// <summary>
    /// 租户ID
    /// </summary>
    protected string? TenantId => _accessor.TenantId;

    #region 数据查询权限相关

    /// <summary>
    /// 读取用户组织架构ID列表
    /// </summary>
    /// <returns></returns>
    protected List<string> GetUserOrganizationIds(string? userId = null)
    {
        List<string> QueryFunc()
        {
            userId ??= UserId;
            // 兼任职信息表
            var orgIds = _freeSql.Select<UserAppointment>()
                .Where(p => p.UserId == userId)
                .ToList(p => p.OrganizationId);

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
            return QueryFunc();
        }

        // Key
        var key = string.Format(CacheConstant.UserOrganizationKey, userId ?? UserId);
        // 过期时间
        var expireTime = TimeSpan.FromMinutes(30);

        return _cacheManager.GetOrCreate(key, QueryFunc, expireTime) ?? new List<string>();
    }

    /// <summary>
    /// 读取用户组织架构及下级组织架构ID列表
    /// </summary>
    /// <returns></returns>
    protected List<string> GetUserOrganizationIdsTree(string? userId = null)
    {
        List<string> QueryFunc()
        {
            userId ??= UserId;
            // 查询用户所在的组织
            var list = GetUserOrganizationIds(userId);

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
            return QueryFunc();
        }

        // Key
        var key = string.Format(CacheConstant.UserOrganizationsKey, userId ?? UserId);
        // 过期时间
        var expireTime = TimeSpan.FromMinutes(30);
        return _cacheManager.GetOrCreate(key, QueryFunc, expireTime) ?? new List<string>();
    }

    /// <summary>
    /// 读取用户角色的数据范围列表
    /// </summary>
    /// <returns></returns>
    private List<DataPermission> GetUserDataScopes(string? userId = null)
    {
        if (_cacheManager == null)
        {
            return QueryFunc();
        }

        List<DataPermission> QueryFunc()
        {
            userId ??= UserId;
            var dataScopeList = _freeSql.Select<Role>()
                .Where(p => _freeSql
                    .Select<RoleUser>()
                    .As("c")
                    .Where(c => c.UserId == userId)
                    .Any(c => c.RoleId == p.Id)
                )
                .ToList(p => new
                {
                    p.DataScope,
                    p.DataScopeCustom
                });

            // 去重
            dataScopeList = dataScopeList.GroupBy(p => p).Select(p => p.Key).ToList();

            // 读取用户的角色数据权限
            var list = _freeSql.Select<RoleDataPermission>()
                // 读取用户的角色
                .Where(p => _freeSql
                    .Select<RoleUser>()
                    .As("c")
                    .Where(c => c.UserId == userId)
                    .Any(c => c.RoleId == p.RoleId)
                )
                // 启用的
                .Where(p => p.Enabled)
                .ToList(p => new DataPermission
                {
                    ResourceKey = p.ResourceKey,
                    DataScope = p.DataScope
                });

            // 读取用户的数据权限
            var userPermissionList = _freeSql.Select<UserDataPermission>()
                .Where(p => p.UserId == userId)
                // 启用的
                .Where(p => p.Enabled)
                // 读取未过期的
                .Where(p => p.ExpireAt == null || p.ExpireAt > DateTime.Now)
                .ToList(p => new DataPermission
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

        // Key
        var key = string.Format(CacheConstant.UserDataScopesKey, userId ?? UserId);
        // 过期时间
        var expireTime = TimeSpan.FromMinutes(30);
        return _cacheManager.GetOrCreate(key, QueryFunc, expireTime) ?? new List<DataPermission>();
    }

    #endregion
}