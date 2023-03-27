using BasicPlatform.AppService.Roles.Requests;

namespace BasicPlatform.AppService.FreeSql.Roles;

/// <summary>
/// 角色请求处理程序
/// </summary>
public class RoleRequestHandler : AppServiceBase<Role>,
    IRequestHandler<CreateRoleRequest, string>,
    IRequestHandler<UpdateRoleRequest, string>,
    IRequestHandler<RoleStatusChangeRequest, string>,
    IRequestHandler<AssignRoleResourcesRequest, string>,
    IRequestHandler<AssignRoleUsersRequest, string>,
    IRequestHandler<AssignRoleDataPermissionsRequest, string>
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
            request.DataScope,
            request.DataScopeCustom,
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
    public async Task<string> Handle(UpdateRoleRequest request, CancellationToken cancellationToken)
    {
        var entity = await GetForUpdateAsync(request.Id!, cancellationToken);
        // 删除旧的资源
        await RegisterDeleteValueObjectAsync<RoleResource>(p => p.RoleId == request.Id, cancellationToken);
        entity.Update(request.Name,
            request.DataScope,
            request.DataScopeCustom,
            request.Remarks, UserId
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
    public async Task<string> Handle(RoleStatusChangeRequest request, CancellationToken cancellationToken)
    {
        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        entity.StatusChange(UserId);
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }

    /// <summary>
    /// 分配资源
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<string> Handle(AssignRoleResourcesRequest request, CancellationToken cancellationToken)
    {
        // 删除旧数据
        await RegisterDeleteValueObjectAsync<RoleResource>(
            p => p.RoleId == request.Id, cancellationToken
        );
        if (request.Resources.Count <= 0)
        {
            return request.Id;
        }

        // 新增新数据
        var roleResources = request
            .Resources
            .Select(p => new RoleResource(request.Id, p.Key, p.Code))
            .ToList();
        await RegisterNewRangeValueObjectAsync(roleResources, cancellationToken);

        return request.Id;
    }

    /// <summary>
    /// 分配用户
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<string> Handle(AssignRoleUsersRequest request, CancellationToken cancellationToken)
    {
        // 删除旧数据
        await RegisterDeleteValueObjectAsync<RoleUser>(
            p => p.RoleId == request.Id, cancellationToken
        );
        if (request.UserIds.Count <= 0)
        {
            return request.Id;
        }

        // 新增新数据
        var roleResources = request
            .UserIds
            .Select(userId => new RoleUser(request.Id, userId))
            .ToList();
        await RegisterNewRangeValueObjectAsync(roleResources, cancellationToken);

        return request.Id;
    }

    /// <summary>
    /// 分配数据权限
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(AssignRoleDataPermissionsRequest request, CancellationToken cancellationToken)
    {
        // 删除旧数据
        await RegisterDeleteValueObjectAsync<RoleDataPermission>(
            p => p.RoleId == request.Id, cancellationToken
        );
        if (request.Permissions.Count <= 0)
        {
            return request.Id;
        }

        // 新增新数据
        var roleResources = request
            .Permissions
            .Select(p => new RoleDataPermission(request.Id, p.ResourceKey, p.DataScope, p.DataScopeCustom, p.Enabled))
            .ToList();
        await RegisterNewRangeValueObjectAsync(roleResources, cancellationToken);

        return request.Id;
    }
}