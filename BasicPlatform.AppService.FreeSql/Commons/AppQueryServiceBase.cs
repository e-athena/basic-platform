namespace BasicPlatform.AppService.FreeSql.Commons;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class AppQueryServiceBase<T> : QueryServiceBase<T> where T : EntityCore, new()
{
    private readonly ISecurityContextAccessor _accessor;
    private readonly FreeSqlMultiTenancy? _multiTenancy;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="freeSql"></param>
    /// <param name="accessor"></param>
    public AppQueryServiceBase(IFreeSql freeSql, ISecurityContextAccessor accessor) :
        base(freeSql)
    {
        _accessor = accessor;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="multiTenancy"></param>
    /// <param name="accessor"></param>
    public AppQueryServiceBase(FreeSqlMultiTenancy multiTenancy, ISecurityContextAccessor accessor) :
        base(multiTenancy)
    {
        _multiTenancy = multiTenancy;
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
    protected IFreeSql DefaultFreeSql =>
        _multiTenancy?.Use("default") ??
        throw new NullReferenceException("FreeSqlMultiTenancy is null.");

    /// <summary>
    /// 主租户查询
    /// </summary>
    protected ISelect<T> DefaultQueryableNoTracking => DefaultFreeSql.Select<T>().NoTracking();

    /// <summary>
    /// 主租户查询
    /// </summary>
    protected ISelect<T1> DefaultQueryNoTracking<T1>() where T1 : class
    {
        return DefaultFreeSql.Select<T1>().NoTracking();
    }
}