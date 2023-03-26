namespace BasicPlatform.AppService.FreeSql.Commons;

/// <summary>
/// 数据权限服务基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class DataPermissionServiceBase<T> : ServiceBase<T> where T : FullEntityCore, new()
{
    private readonly ISecurityContextAccessor _accessor;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWorkManager"></param>
    /// <param name="accessor"></param>
    public DataPermissionServiceBase(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor accessor) :
        base(unitOfWorkManager)
    {
        _accessor = accessor;
    }

    #region 新增

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected override T RegisterNew(T entity)
    {
        entity.CreatedUserId = UserId;
        entity.OrganizationalUnitIds = OrganizationalUnitIds;
        return base.RegisterNew(entity);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override Task<T> RegisterNewAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedUserId = UserId;
        entity.OrganizationalUnitIds = OrganizationalUnitIds;
        return base.RegisterNewAsync(entity, cancellationToken);
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    protected override List<T> RegisterNewRange(List<T> entities)
    {
        foreach (var entity in entities)
        {
            entity.CreatedUserId = UserId;
            entity.OrganizationalUnitIds = OrganizationalUnitIds;
        }

        return base.RegisterNewRange(entities);
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override Task<List<T>> RegisterNewRangeAsync(List<T> entities, CancellationToken cancellationToken)
    {
        foreach (var entity in entities)
        {
            entity.CreatedUserId = UserId;
            entity.OrganizationalUnitIds = OrganizationalUnitIds;
        }

        return base.RegisterNewRangeAsync(entities, cancellationToken);
    }

    #endregion

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

    /// <summary>
    /// 创建人组织架构Ids
    /// </summary>
    private string? OrganizationalUnitIds
    {
        get
        {
            if (IsRoot)
            {
                return null;
            }

            var orgList = GetUserOrganizationIds();
            return orgList.Count == 0 ? null : string.Join(",", orgList);
        }
    }

    /// <summary>
    /// 读取用户组织架构ID列表
    /// </summary>
    /// <returns></returns>
    private List<string> GetUserOrganizationIds()
    {
        // 任职表
        var orgIds = QueryNoTracking<OrganizationUser>()
            .Where(p => p.UserId == UserId)
            .ToList(p => p.OrganizationId);

        // 用户组织
        var orgId = QueryNoTracking<User>()
            .Where(p => p.Id == UserId)
            .First(p => p.OrganizationId);

        if (!string.IsNullOrEmpty(orgId))
        {
            orgIds.Add(orgId);
        }

        return orgIds;
    }
}