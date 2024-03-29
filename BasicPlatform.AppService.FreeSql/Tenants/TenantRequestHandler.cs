using BasicPlatform.AppService.Tenants.Requests;

namespace BasicPlatform.AppService.FreeSql.Tenants;

/// <summary>
/// 租户请求处理程序
/// </summary>
public class TenantRequestHandler : ServiceBase<Tenant>,
    IRequestHandler<CreateTenantRequest, string>,
    IRequestHandler<UpdateTenantRequest, string>,
    IRequestHandler<ChangeTenantStatusRequest, string>,
    IRequestHandler<AssignTenantResourcesRequest, string>,
    IRequestHandler<InitTenantRequest, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWorkManager"></param>
    /// <param name="contextAccessor"></param>
    public TenantRequestHandler(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor contextAccessor)
        : base(unitOfWorkManager, contextAccessor)
    {
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(CreateTenantRequest request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.Code))
        {
            // 租户编码唯一检查
            var exists = await QueryableNoTracking.Where(p => p.Code == request.Code).AnyAsync(cancellationToken);
            if (exists)
            {
                throw FriendlyException.Of("租户编码已存在");
            }
        }

        var id = ObjectId.GenerateNewStringId();
        var entity = new Tenant(
            id,
            request.Name,
            request.Code,
            request.IsolationLevel,
            request.ContactName,
            request.ContactPhoneNumber,
            request.ContactEmail,
            request.ConnectionString,
            request.Remarks,
            request.EffectiveTime,
            request.ExpiredTime,
            UserId,
            request.Applications.Select(x =>
                    new TenantApplication(id,
                        x.AppId,
                        x.IsolationLevel,
                        x.ConnectionString,
                        x.ExpiredTime,
                        UserId,
                        x.IsEnabled
                    ))
                .ToList()
        );
        await RegisterNewAsync(entity, cancellationToken);
        return entity.Id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(UpdateTenantRequest request, CancellationToken cancellationToken)
    {
        // 租户编码唯一检查
        var exists = await QueryableNoTracking
            .Where(p => p.Code == request.Code)
            .Where(p => p.Id != request.Id)
            .AnyAsync(cancellationToken);
        if (exists)
        {
            throw FriendlyException.Of("租户编码已存在");
        }

        var entity = await GetAsync(request.Id!, cancellationToken);
        entity.Update(
            request.Name,
            request.Code!,
            request.IsolationLevel,
            request.ContactName,
            request.ContactPhoneNumber,
            request.ContactEmail,
            request.ConnectionString,
            request.Remarks,
            request.EffectiveTime,
            request.ExpiredTime,
            UserId,
            request.Applications.Select(x =>
                    new TenantApplication(
                        request.Id!,
                        x.AppId,
                        x.IsolationLevel,
                        x.ConnectionString,
                        x.ExpiredTime,
                        UserId,
                        x.IsEnabled
                    ))
                .ToList()
        );
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }

    /// <summary>
    /// 变更状态
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(ChangeTenantStatusRequest request, CancellationToken cancellationToken)
    {
        var entity = await GetAsync(request.Id, cancellationToken);
        entity.ChangeStatus(UserId);
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }

    /// <summary>
    /// 分配资源
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(AssignTenantResourcesRequest request, CancellationToken cancellationToken)
    {
        var entity = await GetAsync(request.Id, cancellationToken);
        entity.AssignResources(request
                .Resources
                .Select(p => new TenantResource(p.AppId, request.Id, p.Key, p.Code))
                .ToList(),
            UserId);
        await RegisterDirtyAsync(entity, cancellationToken);
        return request.Id;
    }

    /// <summary>
    /// 初始化租户数据库
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="FriendlyException"></exception>
    public async Task<string> Handle(InitTenantRequest request, CancellationToken cancellationToken)
    {
        var entity = await Queryable.Where(p => p.Code == request.Code).FirstAsync(cancellationToken);
        if (entity == null)
        {
            throw FriendlyException.Of("租户不存在");
        }

        // 已处理过，不再处理
        if (entity.IsInitDatabase)
        {
            return entity.Id;
        }

        entity.InitDatabase(request.UserId ?? UserId);
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }
}