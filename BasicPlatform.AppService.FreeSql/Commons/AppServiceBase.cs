namespace BasicPlatform.AppService.FreeSql.Commons;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class AppServiceBase<T> : ServiceBase<T> where T : EntityCore, new()
{
    private readonly ISecurityContextAccessor _accessor;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWorkManager"></param>
    /// <param name="accessor"></param>
    public AppServiceBase(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor accessor) :
        base(unitOfWorkManager)
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

    /// <summary>
    /// IP地址
    /// </summary>
    protected string IpAddress => _accessor.IpAddress;

    /// <summary>
    /// 是否为租户管理员
    /// </summary>
    protected bool IsTenantAdmin => _accessor.IsTenantAdmin;

    /// <summary>
    /// 是否租户环境
    /// </summary>
    protected bool IsTenantEnvironment => !string.IsNullOrEmpty(TenantId);
}