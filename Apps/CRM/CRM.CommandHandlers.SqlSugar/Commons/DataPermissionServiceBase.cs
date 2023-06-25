using Athena.Infrastructure.DataPermission;
using Athena.Infrastructure.Providers;
using Athena.Infrastructure.SqlSugar.Bases;

namespace CRM.CommandHandlers.SqlSugar.Commons;

/// <summary>
/// 数据权限服务基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class DataPermissionServiceBase<T> : ServiceBase<T> where T : FullEntityCore, new()
{
    private readonly ISecurityContextAccessor _accessor;
    private readonly IDataPermissionService? _dataPermissionService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sqlSugarClient"></param>
    /// <param name="accessor"></param>
    public DataPermissionServiceBase(ISqlSugarClient sqlSugarClient, ISecurityContextAccessor accessor) :
        base(sqlSugarClient)
    {
        _accessor = accessor;
        _dataPermissionService =
            AthenaProvider.Provider?.GetService(typeof(IDataPermissionService)) as IDataPermissionService;
    }

    #region 新增

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected override bool RegisterNew(T entity)
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
    protected override Task<bool> RegisterNewAsync(T entity, CancellationToken cancellationToken = default)
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
    protected override bool RegisterNewRange(List<T> entities)
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
    protected override Task<bool> RegisterNewRangeAsync(List<T> entities, CancellationToken cancellationToken = default)
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
    /// 是否为租户管理员
    /// </summary>
    protected bool IsTenantAdmin => _accessor.IsTenantAdmin;

    /// <summary>
    /// 创建人组织架构Ids
    /// </summary>
    private string? OrganizationalUnitIds
    {
        get
        {
            if (IsRoot || IsTenantAdmin)
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
    protected List<string> GetUserOrganizationIds(string? userId = null)
    {
        userId ??= UserId;

        if (_dataPermissionService == null || userId == null)
        {
            return new List<string>();
        }

        return _dataPermissionService.GetUserOrganizationIds(userId, null);
    }
}