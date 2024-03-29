using BasicPlatform.AppService.Resources.Requests;
using BasicPlatform.Domain.Models.Roles;
using BasicPlatform.Domain.Models.Users;

namespace BasicPlatform.AppService.FreeSql.Resources;

/// <summary>
/// 资源请求处理程序
/// </summary>
public class ResourceRequestHandler : ServiceBase<Resource>,
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
    /// 同步资源
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> Handle(SyncResourceRequest request, CancellationToken cancellationToken)
    {
        var keys = request.Resources.Select(c => c.Key);
        // 查询系统存在的资源，但是请求中不存在的资源
        var notExists = await QueryableNoTracking
            .Where(p => p.AppId == request.AppId)
            .Where(p => !keys.Contains(p.Key))
            .ToListAsync(p => p.Key, cancellationToken);

        if (notExists.Count > 0)
        {
            // 删除用户资源关联
            await RegisterDeleteValueObjectAsync<UserResource>(p =>
                    notExists.Contains(p.ResourceKey) &&
                    p.AppId == request.AppId, cancellationToken
            );
            // 删除角色资源关联
            await RegisterDeleteValueObjectAsync<RoleResource>(p =>
                    notExists.Contains(p.ResourceKey) &&
                    p.AppId == request.AppId, cancellationToken
            );
            // 删除资源
            await RegisterDeleteAsync(p =>
                    notExists.Contains(p.Key) &&
                    p.AppId == request.AppId, cancellationToken
            );
        }

        // 查询请求中存在的资源，但是系统不存在的资源
        var exists = await QueryableNoTracking
            .Where(p => p.AppId == request.AppId)
            .Where(p => keys.Contains(p.Key))
            .ToListAsync(p => p.Key, cancellationToken);

        var count = 0;
        if (exists.Count == 0)
        {
            var newResources = request
                .Resources
                .Select(p => p.Key)
                // 过滤重复的
                .Distinct()
                // 过滤已经存在的
                .Where(p => !exists.Contains(p))
                .Select(key => new Resource(request.AppId, key, 0, Status.Enabled, UserId))
                .ToList();

            if (newResources.Count > 0)
            {
                await RegisterNewRangeAsync(newResources, cancellationToken);
            }

            count = newResources.Count;
        }

        // 更新用户和角色已分配的资源
        // 处理角色资源
        var roleResources = await QueryNoTracking<RoleResource>()
            .Where(p => p.AppId == request.AppId)
            .ToListAsync(p => new
            {
                p.RoleId,
                p.ResourceKey
            }, cancellationToken);
        if (roleResources.Count > 0)
        {
            // 按角色分组处理
            var roleResourceGroups = roleResources
                .GroupBy(p => p.RoleId)
                .ToList();
            var roleIds = roleResourceGroups
                .Select(c => c.Key)
                .ToList();
            // 删除角色资源关联
            await RegisterDeleteValueObjectAsync<RoleResource>(p =>
                    roleIds.Contains(p.RoleId) &&
                    p.AppId == request.AppId,
                cancellationToken
            );
            var newRoleResources = new List<RoleResource>();
            // 重新添加角色资源关联
            foreach (var group in roleResourceGroups)
            {
                var roleId = group.Key;
                foreach (var item in group)
                {
                    var rm = request.Resources.FirstOrDefault(p => p.Key == item.ResourceKey);
                    if (rm == null)
                    {
                        continue;
                    }

                    newRoleResources.Add(new RoleResource(request.AppId, roleId, item.ResourceKey, rm.Code));
                }
            }

            if (newRoleResources.Count > 0)
            {
                await RegisterNewRangeValueObjectAsync(newRoleResources, cancellationToken);
            }
        }

        // 处理用户资源
        var userResources = await QueryNoTracking<UserResource>()
            .Where(p => p.AppId == request.AppId)
            .ToListAsync(p => new
            {
                p.UserId,
                p.ResourceKey
            }, cancellationToken);
        if (userResources.Count <= 0)
        {
            return count;
        }

        // 按用户分组处理
        var userResourceGroups = userResources
            .GroupBy(p => p.UserId)
            .ToList();
        var userIds = userResourceGroups
            .Select(c => c.Key)
            .ToList();
        // 删除用户资源关联
        await RegisterDeleteValueObjectAsync<UserResource>(p =>
                userIds.Contains(p.UserId) &&
                p.AppId == request.AppId,
            cancellationToken
        );
        var newUserResources = new List<UserResource>();
        // 重新添加用户资源关联
        foreach (var group in userResourceGroups)
        {
            var userId = group.Key;
            foreach (var item in group)
            {
                var rm = request.Resources.FirstOrDefault(p => p.Key == item.ResourceKey);
                if (rm == null)
                {
                    continue;
                }

                newUserResources.Add(new UserResource(request.AppId, userId, item.ResourceKey, rm.Code));
            }
        }

        if (newUserResources.Count > 0)
        {
            await RegisterNewRangeValueObjectAsync(newUserResources, cancellationToken);
        }

        return count;
    }
}