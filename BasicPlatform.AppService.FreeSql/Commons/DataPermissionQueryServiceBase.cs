using System.Linq.Expressions;
using Athena.Infrastructure.QueryFilters;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="freeSql"></param>
    /// <param name="accessor"></param>
    public DataPermissionQueryServiceBase(IFreeSql freeSql, ISecurityContextAccessor accessor) :
        base(freeSql)
    {
        _freeSql = freeSql;
        _accessor = accessor;
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
        // 如果是开发者帐号。则不需要过滤
        if (IsRoot)
        {
            return base.Query<T1>();
        }

        // 数据访问范围
        var dataScopeList = GetUserRoleDataScopeListAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        // 如果包含全部的数据。则不需要过滤
        if (dataScopeList.Any(dataScope => dataScope == RoleDataScope.All))
        {
            return base.Query<T1>();
        }

        var filterWhere = GenerateFilterWhere<T1>(dataScopeList);
        return _freeSql.Queryable<T1>().HasWhere(filterWhere, filterWhere!);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <returns></returns>
    private ISelect<T1> QueryNoTrackingWithPermission<T1>() where T1 : class
    {
        // 如果是开发者帐号。则不需要过滤
        if (IsRoot)
        {
            return base.QueryNoTracking<T1>();
        }

        // 数据访问范围
        var dataScopeList = GetUserRoleDataScopeListAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        // 如果包含全部的数据。则不需要过滤
        if (dataScopeList.Any(dataScope => dataScope == RoleDataScope.All))
        {
            return base.QueryNoTracking<T1>();
        }

        var filterWhere = GenerateFilterWhere<T1>(dataScopeList);
        return _freeSql.Queryable<T1>().NoTracking().HasWhere(filterWhere, filterWhere!);
    }

    private bool HasProperty<T1>(string propertyName)
    {
        return typeof(T1).GetProperties().Any(p => p.Name == propertyName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataScopeList"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    private Expression<Func<TResult, bool>>? GenerateFilterWhere<TResult>(ICollection<RoleDataScope> dataScopeList)
    {
        if (dataScopeList.Count == 0)
        {
            return null;
        }

        var filters = new List<QueryFilter>();
        var organizationIds = new List<string>();
        foreach (var scope in dataScopeList)
        {
            if (scope == RoleDataScope.Self)
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

            var orgIds = scope switch
            {
                RoleDataScope.Department => GetUserOrganizationIds(),
                RoleDataScope.DepartmentAndSub => GetUserOrganizationIdsTree(),
                RoleDataScope.Custom => GetUserRoleDataScopeCustomList(),
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
        userId ??= UserId;
        // 任职表
        var orgIds = _freeSql.Select<OrganizationUser>()
            .Where(p => p.UserId == UserId)
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

    /// <summary>
    /// 读取用户组织架构及下级组织架构ID列表
    /// </summary>
    /// <returns></returns>
    protected List<string> GetUserOrganizationIdsTree(string? userId = null)
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
        var filterWhere = QueryableExtensions.MakeFilterWhere<Organization>(filters, false);
        // 查询用户组织架构的下级组织
        var orgIds = _freeSql.Select<Organization>()
            .Where(filterWhere)
            .ToList(p => p.Id);
        list.AddRange(orgIds);

        // 数据去重
        return list.GroupBy(p => p).Select(p => p.Key).ToList();
        ;
    }

    /// <summary>
    /// 读取用户角色的数据范围列表
    /// </summary>
    /// <returns></returns>
    protected async Task<List<RoleDataScope>> GetUserRoleDataScopeListAsync(string? userId = null)
    {
        userId ??= UserId;
        var dataScopeList = await _freeSql.Select<Role>()
            .Where(p => _freeSql
                .Select<RoleUser>()
                .As("c")
                .Where(c => c.UserId == userId)
                .Any(c => c.RoleId == p.Id)
            )
            .ToListAsync(p => p.DataScope);

        // 去重
        return dataScopeList.GroupBy(p => p).Select(p => p.Key).ToList();
    }

    /// <summary>
    /// 读取用户角色的自定义数据范围列表
    /// </summary>
    /// <returns></returns>
    protected List<string> GetUserRoleDataScopeCustomList(string? userId = null)
    {
        userId ??= UserId;
        var dataScopeList = _freeSql.Select<Role>()
            .Where(p => _freeSql
                .Select<RoleUser>()
                .As("c")
                .Where(c => c.UserId == userId)
                .Any(c => c.RoleId == p.Id)
            )
            // 过滤掉空值
            .Where(p => !string.IsNullOrEmpty(p.DataScopeCustom))
            .ToList(p => p.DataScopeCustom);

        // 展开DataScopeCustom
        var list = new List<string>();
        foreach (var ids in dataScopeList.Select(item => item!.Split(",", StringSplitOptions.RemoveEmptyEntries)))
        {
            list.AddRange(ids);
        }

        // 去重
        return list.GroupBy(p => p).Select(p => p.Key).ToList();
    }

    #endregion
}