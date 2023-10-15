using BasicPlatform.AppService.Applications.Requests;

namespace BasicPlatform.AppService.FreeSql.Applications;

/// <summary>
/// 网站系统应用请求处理程序
/// </summary>
public class ApplicationRequestHandler : ServiceBase<Application>,
    IRequestHandler<CreateApplicationRequest, string>,
    IRequestHandler<UpdateApplicationRequest, string>,
    IRequestHandler<ApplicationStatusChangeRequest, string>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWorkManager"></param>
    /// <param name="accessor"></param>
    public ApplicationRequestHandler(
        UnitOfWorkManager unitOfWorkManager,
        ISecurityContextAccessor accessor) : base(unitOfWorkManager, accessor)
    {
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(CreateApplicationRequest request, CancellationToken cancellationToken)
    {
        var exists = await QueryableNoTracking
            .Where(p => p.Environment == request.Environment)
            .AnyAsync(x => x.ClientId == request.ClientId, cancellationToken);
        // 检查clientId是否已存在
        if (exists)
        {
            if (request.Environment == null)
            {
                throw FriendlyException.Of("ClientId已存在");
            }

            throw FriendlyException.Of($"{request.Environment}环境下的ClientId已存在");
        }

        var entity = new Application(
            request.Environment,
            request.Name,
            request.ClientId,
            request.UseDefaultClientSecret,
            request.FrontendUrl,
            request.ApiUrl,
            request.MenuResourceRoute,
            request.PermissionResourceRoute,
            request.Remarks,
            UserId
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
    public async Task<string> Handle(UpdateApplicationRequest request, CancellationToken cancellationToken)
    {
        // 检查clientId是否已存在
        if (await QueryableNoTracking
                .Where(p => p.Environment == request.Environment)
                .Where(p => p.ClientId == request.ClientId)
                .AnyAsync(x => x.Id != request.Id, cancellationToken))
        {
            throw FriendlyException.Of("ClientId已存在");
        }

        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        entity.Update(request.Name,
            request.ClientId,
            request.UseDefaultClientSecret,
            request.FrontendUrl,
            request.ApiUrl,
            request.MenuResourceRoute,
            request.PermissionResourceRoute,
            request.Remarks,
            UserId
        );
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(ApplicationStatusChangeRequest request, CancellationToken cancellationToken)
    {
        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        entity.StatusChange(UserId);
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }
}