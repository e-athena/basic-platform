namespace BasicPlatform.AppService.FreeSql.Commons;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class AppQueryServiceBase<T> : QueryServiceBase<T> where T : EntityCore, new()
{
    private readonly ISecurityContextAccessor _accessor;
    private readonly IFreeSql _freeSql;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="freeSql"></param>
    /// <param name="accessor"></param>
    public AppQueryServiceBase(IFreeSql freeSql, ISecurityContextAccessor accessor) :
        base(freeSql)
    {
        _freeSql = freeSql;
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
}