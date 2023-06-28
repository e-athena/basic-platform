namespace CMS.AppService.FreeSql.Commons;

/// <summary>
/// 应用查询服务基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class AppQueryServiceBase<T> : QueryServiceBase<T> where T : EntityCore, new()
{
    private readonly ISecurityContextAccessor _accessor;

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