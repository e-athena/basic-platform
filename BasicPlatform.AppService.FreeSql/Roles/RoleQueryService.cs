using BasicPlatform.AppService.Roles;
using BasicPlatform.AppService.Roles.Models;
using BasicPlatform.AppService.Roles.Requests;
using BasicPlatform.AppService.Roles.Responses;

namespace BasicPlatform.AppService.FreeSql.Roles;

/// <summary>
/// 角色查询服务接口实现类
/// </summary>
[Component]
public class RoleQueryService : QueryServiceBase<Role>, IRoleQueryService
{
    public RoleQueryService(IFreeSql freeSql) : base(freeSql)
    {
    }

    /// <summary>
    /// 读取分页数据
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Paging<GetRolePagingResponse>> GetPagingAsync(GetRolePagingRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await QueryableNoTracking
            .HasWhere(request.Keyword, p => p.Name.Contains(request.Keyword!))
            .HasWhere(request.DataScope, p => request.DataScope!.Contains(p.DataScope))
            .ToPagingAsync(request, p => new GetRolePagingResponse
            {
                CreatedUserName = p.CreatedUser!.RealName,
                UpdatedUserName = p.LastUpdatedUser!.RealName
            }, cancellationToken);
        return result;
    }

    /// <summary>
    /// 读取详情
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GetRoleByIdResponse> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var entity = await QueryableNoTracking
            .Where(p => p.Id == id)
            .FirstAsync<GetRoleByIdResponse>(cancellationToken);
        if (entity is null)
        {
            throw FriendlyException.Of("角色不存在");
        }

        // 读取资源代码
        entity.Resources = await QueryNoTracking<RoleResource>()
            .Where(p => p.RoleId == id)
            .ToListAsync(p => new ResourceModel
            {
                Key = p.ResourceKey,
                Code = p.ResourceCode
            }, cancellationToken);

        if (entity is {DataScope: RoleDataScope.Custom, DataScopeCustomList.Count: > 0})
        {
            // 读取自定义数据访问范围
            entity.DataScopeCustomSelectList = await QueryNoTracking<Organization>()
                .Where(p => entity.DataScopeCustomList.Contains(p.Id))
                .ToListAsync(p => new SelectViewModel
                {
                    Label = p.Name,
                    Value = p.Id,
                    Disabled = false
                }, cancellationToken);
        }

        // 读取角色用户
        entity.Users = await QueryNoTracking<RoleUser>()
            .Where(p => p.RoleId == id)
            .ToListAsync(p => new SelectViewModel
            {
                Value = p.UserId,
                Label = p.User.RealName,
                Disabled = false,
                Extend = p.User.UserName
            }, cancellationToken);

        return entity;
    }

    /// <summary>
    /// 读取下拉列表
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<SelectViewModel>> GetSelectListAsync(CancellationToken cancellationToken = default)
    {
        var list = await QueryNoTracking().ToListAsync(cancellationToken);
        return list
            .Select(p => new SelectViewModel
            {
                Disabled = p.Status == Status.Disabled,
                Label = $"{p.Name}[数据范围:{p.DataScope.ToDescription()}]",
                Value = p.Id
            })
            .ToList();
    }

    /// <summary>
    /// 读取用户拥有的角色
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<string>> GetRoleIdsByUserIdAsync(string userId,
        CancellationToken cancellationToken = default)
    {
        // 读取用户兼任职表
        var organizationIds = await Query<UserAppointment>()
            .Where(p => p.UserId == userId)
            // 组织架构是启用的
            .Where(p => p.Organization.Status == Status.Enabled)
            .ToListAsync(p => p.OrganizationId, cancellationToken);
        // 组织架构Id
        var organizationIdList = new List<string>();
        // 读取下级组织架构Id列表
        foreach (var organizationId in organizationIds)
        {
            var itemIds = await QueryNoTracking<Organization>()
                .Where(p => !string.IsNullOrEmpty(p.ParentPath) && p.ParentPath.Contains(organizationId))
                // 组织架构是启用的
                .Where(p => p.Status == Status.Enabled)
                .ToListAsync(p => p.Id, cancellationToken);
            if (itemIds.Count > 0)
            {
                organizationIdList.AddRange(itemIds);
            }
        }

        // 读取组织架构角色
        var roleIds1 = await QueryNoTracking<OrganizationRole>()
            .As("b")
            .Where(p => organizationIdList.Contains(p.OrganizationId))
            // 组织架构是启用的
            .Where(p => p.Organization.Status == Status.Enabled)
            // 角色是启用的
            .Where(p => p.Role.Status == Status.Enabled)
            .ToListAsync(p => p.RoleId, cancellationToken);

        // 读取用户角色
        var roleIds3 = await QueryNoTracking<RoleUser>()
            .As("d")
            .Where(p => p.UserId == userId)
            // 角色是启用的
            .Where(p => p.Role.Status == Status.Enabled)
            .ToListAsync(p => p.RoleId, cancellationToken);

        return roleIds1.Union(roleIds3).ToList();
    }

    /// <summary>
    /// 读取角色拥有的权限
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<List<RoleDataPermissionModel>> GetDataPermissionsAsync(string id)
    {
        return QueryNoTracking<RoleDataPermission>()
            .Where(p => p.RoleId == id)
            .ToListAsync<RoleDataPermissionModel>();
    }
}