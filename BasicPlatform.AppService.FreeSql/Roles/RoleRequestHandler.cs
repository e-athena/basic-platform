using BasicPlatform.AppService.Roles.Requests;
using BasicPlatform.AppService.Users;
using BasicPlatform.Domain.Models.Roles;

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
    private readonly IUserQueryService _userQueryService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitOfWorkManager"></param>
    /// <param name="contextAccessor"></param>
    /// <param name="userQueryService"></param>
    public RoleRequestHandler(UnitOfWorkManager unitOfWorkManager, ISecurityContextAccessor contextAccessor,
        IUserQueryService userQueryService)
        : base(unitOfWorkManager, contextAccessor)
    {
        _userQueryService = userQueryService;
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
    public async Task<string> Handle(AssignRoleResourcesRequest request, CancellationToken cancellationToken)
    {
        // 如果不是超级管理员，则需要检查分配的权限中是否有超过自己的权限，按ApplicationId分组，如果分配的权限中包含超过自己的权限，则抛出异常
        if (!IsRoot)
        {
            var resources = await _userQueryService.GetUserResourceAsync(null, null);
            var resourceKeys = resources
                .GroupBy(p => p.ApplicationId)
                .Select(p => new
                {
                    ApplicationId = p.Key,
                    Keys = p.Select(k => k.Key).ToList()
                }).ToList();

            var assignResourceKeys = request
                .Resources
                .GroupBy(p => p.ApplicationId)
                .Select(p => new
                {
                    ApplicationId = p.Key,
                    Keys = p.Select(k => k.Key).ToList()
                }).ToList();

            if (assignResourceKeys
                .Any(p =>
                    !resourceKeys
                        .Any(k =>
                            k.ApplicationId == p.ApplicationId &&
                            k.Keys.Any(p.Keys.Contains)
                        )
                ))
            {
                throw FriendlyException.Of("分配的权限中包含超过自己的权限，请检查！");
            }
        }

        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        entity.AssignResources(request
                .Resources
                .Select(p => new RoleResource(p.ApplicationId, request.Id, p.Key, p.Code))
                .ToList(),
            UserId
        );
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
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
        var entity = await GetForUpdateAsync(request.Id, cancellationToken);
        entity.AssignUsers(request.UserIds, UserId);
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }

    /// <summary>
    /// 分配数据权限
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(AssignRoleDataPermissionsRequest request, CancellationToken cancellationToken)
    {
        var entity = await GetForUpdateAsync(request.Id, cancellationToken);

        // 分配权限
        entity.AssignDataPermissions(request
                .Permissions
                .Select(p => new RoleDataPermission(
                    p.ApplicationId,
                    request.Id,
                    p.ResourceKey,
                    p.DataScope,
                    p.DataScopeCustom,
                    p.PolicyResourceKey,
                    p.QueryFilterGroups,
                    p.Enabled)
                )
                .ToList(),
            UserId
        );
        await RegisterDirtyAsync(entity, cancellationToken);
        return entity.Id;
    }
}