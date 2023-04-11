using BasicPlatform.AppService.Applications.Requests;

namespace BasicPlatform.AppService.FreeSql.Applications;

/// <summary>
/// 网站系统应用请求处理程序
/// </summary>
public class ApplicationRequestHandler : AppServiceBase<Application>,
    IRequestHandler<CreateApplicationRequest, string>,
    IRequestHandler<UpdateApplicationRequest, string>
{
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
        // 检查clientId是否已存在
        if (await QueryableNoTracking.AnyAsync(x => x.ClientId == request.ClientId, cancellationToken))
        {
            throw FriendlyException.Of("ClientId已存在");
        }

        var entity = new Application(
            request.Name,
            request.ClientId,
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
        if (await QueryableNoTracking.AnyAsync(x => x.ClientId == request.ClientId && x.Id != request.Id, cancellationToken))
        {
            throw FriendlyException.Of("ClientId已存在");
        }
        
        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        entity.Update(request.Name,
            request.ClientId,
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
}