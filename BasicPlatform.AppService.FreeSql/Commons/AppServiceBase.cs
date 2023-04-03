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
    /// 读取信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="FriendlyException"></exception>
    protected async Task<T> GetForEditAsync(string? id)
    {
        var entity = await Queryable
            .Where(p => p.Id == id)
            .ToOneAsync();

        if (entity == null)
        {
            throw FriendlyException.Of("无权限操作或找不到数据。");
        }

        return entity;
    }
}