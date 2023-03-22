using BasicPlatform.AppService.Roles.Requests;

namespace BasicPlatform.AppService.FreeSql.Roles;

/// <summary>
/// 角色请求处理程序
/// </summary>
public class RoleRequestHandler : AppServiceBase<Role>,
    IRequestHandler<CreateRoleRequest, string>,
    IRequestHandler<UpdateRoleRequest, string>,
    IRequestHandler<RoleStatusChangeRequest, string>
{
    public RoleRequestHandler(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor contextAccessor)
        : base(unitOfWorkManager, contextAccessor)
    {
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var entity = new Role(
            request.Name,
            request.Remarks,
            UserId
        );
        await RegisterNewAsync(entity, cancellationToken);
        if (request.Resources is not {Count: > 0})
        {
            return entity.Id;
        }

        var roleResources = request
            .Resources
            .Select(p => new RoleResource(entity.Id, p.Key, p.Code))
            .ToList();
        await RegisterNewRangeValueObjectAsync(roleResources, cancellationToken);

        return entity.Id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(UpdateRoleRequest request, CancellationToken cancellationToken)
    {
        var entity = await GetForUpdateAsync(request.Id!, cancellationToken);
        // 删除旧的资源
        await RegisterDeleteValueObjectAsync<RoleResource>(p => p.RoleId == request.Id, cancellationToken);
        entity.Update(request.Name, request.Remarks, UserId);
        await RegisterDirtyAsync(entity, cancellationToken);
        if (request.Resources is not {Count: > 0})
        {
            return entity.Id;
        }

        // 添加新的资源
        var roleResources = request
            .Resources
            .Select(p => new RoleResource(entity.Id, p.Key, p.Code))
            .ToList();

        await RegisterNewRangeValueObjectAsync(roleResources, cancellationToken);

        return entity.Id;
    }

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(RoleStatusChangeRequest request, CancellationToken cancellationToken)
    {
        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        entity.StatusChange(UserId);
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }
}