using BasicPlatform.AppService.Resources.Requests;

namespace BasicPlatform.AppService.FreeSql.Resources;

/// <summary>
/// 资源请求处理程序
/// </summary>
public class ResourceRequestHandler : AppServiceBase<Resource>,
    IRequestHandler<ReinitializeResourceRequest, int>,
    IRequestHandler<SyncResourceRequest, int>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWorkManager"></param>
    /// <param name="accessor"></param>
    public ResourceRequestHandler(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor accessor)
        : base(unitOfWorkManager, accessor)
    {
    }

    /// <summary>
    /// 重新初始化资源
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> Handle(ReinitializeResourceRequest request, CancellationToken cancellationToken)
    {
        // 删除用户资源关联
        await RegisterDeleteValueObjectAsync<UserResource>(p => true, cancellationToken);
        // 删除角色资源关联
        await RegisterDeleteValueObjectAsync<RoleResource>(p => true, cancellationToken);
        // 删除资源
        await RegisterDeleteAsync(p => true, cancellationToken);

        if (request.ResourceCodes.Count <= 0)
        {
            return 0;
        }

        // 重新初始化资源
        var entities = request
            .ResourceCodes
            // 过滤重复的
            .Distinct()
            .Select(code => new Resource(code, 0, Status.Enabled, UserId))
            .ToList();

        // 批量新增
        await RegisterNewRangeAsync(entities, cancellationToken);

        return entities.Count;
    }

    /// <summary>
    /// 同步资源
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<int> Handle(SyncResourceRequest request, CancellationToken cancellationToken)
    {
        // 查询系统存在的资源，但是请求中不存在的资源
        var notExists = await QueryableNoTracking
            .Where(p => !request.ResourceCodes.Contains(p.Code))
            .ToListAsync(p => p.Code, cancellationToken);

        if (notExists.Count > 0)
        {
            // 删除用户资源关联
            await RegisterDeleteValueObjectAsync<UserResource>(p =>
                    notExists.Contains(p.ResourceCode), cancellationToken
            );
            // 删除角色资源关联
            await RegisterDeleteValueObjectAsync<RoleResource>(p =>
                    notExists.Contains(p.ResourceCode), cancellationToken
            );
        }

        // 查询请求中存在的资源，但是系统不存在的资源
        var exists = await QueryableNoTracking
            .Where(p => request.ResourceCodes.Contains(p.Code))
            .ToListAsync(p => p.Code, cancellationToken);

        if (exists.Count <= 0)
        {
            return 0;
        }

        var newResources = request
            .ResourceCodes
            // 过滤重复的
            .Distinct()
            // 过滤已经存在的
            .Where(p => !exists.Contains(p))
            .Select(code => new Resource(code, 0, Status.Enabled, UserId))
            .ToList();

        if (newResources.Count > 0)
        {
            await RegisterNewRangeAsync(newResources, cancellationToken);
        }

        return newResources.Count;
    }
}