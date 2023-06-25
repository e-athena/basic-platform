using Athena.Infrastructure.Domain;
using Athena.Infrastructure.Jwt;
using Athena.Infrastructure.SqlSugar.Bases;

namespace CRM.QueryServices.SqlSugar.Commons;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class AppQueryServiceBase<T> : QueryServiceBase<T> where T : EntityCore, new()
{
    private readonly ISecurityContextAccessor _accessor;
    private readonly ISqlSugarClient _sqlSugarClient;

    public AppQueryServiceBase(ISqlSugarClient sqlSugarClient, ISecurityContextAccessor accessor) : base(sqlSugarClient)
    {
        _sqlSugarClient = sqlSugarClient;
        _accessor = accessor;
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

    /// <summary>
    /// 是否为租户管理员
    /// </summary>
    protected bool IsTenantAdmin => _accessor.IsTenantAdmin;

    /// <summary>
    /// 是否租户环境
    /// </summary>
    protected bool IsTenantEnvironment => !string.IsNullOrEmpty(TenantId);

    /// <summary>
    /// 主租户
    /// </summary>
    private SqlSugarScopeProvider DefaultScopeProvider => _sqlSugarClient
        .AsTenant()
        .GetConnectionScope("default");
    
    /// <summary>
    /// 主租户查询
    /// </summary>
    protected ISugarQueryable<T> DefaultQueryableNoTracking => DefaultScopeProvider.Queryable<T>();

    /// <summary>
    /// 主租户查询
    /// </summary>
    protected ISugarQueryable<T1> DefaultQueryNoTracking<T1>() where T1 : class
    {
        return DefaultScopeProvider.Queryable<T1>();
    }
}