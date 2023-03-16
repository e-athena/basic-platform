using BasicPlatform.AppService.ExternalPages.Models;
using BasicPlatform.AppService.Roles;
using BasicPlatform.AppService.Roles.Models;
using BasicPlatform.AppService.Users;
using BasicPlatform.AppService.Users.Models;
using BasicPlatform.AppService.Users.Requests;
using BasicPlatform.AppService.Users.Responses;

namespace BasicPlatform.AppService.FreeSql.Users;

/// <summary>
/// 用户服务接口实现类
/// </summary>
[Component(LifeStyle.Transient)]
public class UserQueryService : AppQueryServiceBase<User>, IUserQueryService
{
    private readonly IRoleQueryService _roleQueryService;

    public UserQueryService(
        IFreeSql freeSql,
        ISecurityContextAccessor accessor,
        IRoleQueryService roleQueryService
    ) : base(freeSql, accessor)
    {
        _roleQueryService = roleQueryService;
    }

    /// <summary>
    /// 读取分页列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Paging<GetUserPagingResponse>> GetPagingAsync(GetUserPagingRequest request)
    {
        ISelect<Organization>? organizationQuery = null;
        if (request.OrganizationId != null)
        {
            organizationQuery = QueryNoTracking<Organization>()
                .As("o")
                // 当前组织架构及下级组织架构
                .Where(p => p.ParentPath.Contains(request.OrganizationId!) || p.Id == request.OrganizationId);
        }

        ISelect<RoleUser>? userRoleQuery = null;
        if (request.RoleId != null)
        {
            userRoleQuery = QueryNoTracking<RoleUser>()
                .As("d")
                .Where(p => p.RoleId == request.RoleId);
        }

        var result = await QueryableNoTracking
            .HasWhere(request.Keyword, p =>
                p.UserName.Contains(request.Keyword!) ||
                p.RealName.Contains(request.Keyword!) ||
                p.PhoneNumber!.Contains(request.Keyword!) ||
                p.Email!.Contains(request.Keyword!)
            )
            .HasWhere(request.Status, p => request.Status!.Contains(p.Status))
            .HasWhere(request.Gender, p => request.Gender!.Contains(p.Gender))
            .HasWhere(organizationQuery, p => organizationQuery!.Any(o => o.Id == p.OrganizationId))
            .HasWhere(userRoleQuery, p => userRoleQuery!.Any(d => d.UserId == p.Id))
            .ToPagingAsync(request, p => new GetUserPagingResponse
            {
                CreatedUserName = p.CreatedUser!.RealName,
                UpdatedUserName = p.UpdatedUser!.RealName,
                OrganizationName = p.Organization!.Name,
                PositionName = p.Position!.Name
            });

        return result;
    }


    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GetUserByIdResponse> GetAsync(string id, CancellationToken cancellationToken = default)

    {
        var result = await Queryable
            .Where(p => p.Id == id)
            .ToOneAsync(p => new GetUserByIdResponse
            {
                Password = string.Empty
            }, cancellationToken);

        if (result == null)
        {
            throw FriendlyException.Of("找不到数据");
        }

        // 读取角色
        var roleIds = await Query<RoleUser>()
            .Where(p => p.UserId == id)
            .ToListAsync(p => p.RoleId, cancellationToken);

        result.RoleIds.AddRange(roleIds);

        return result;
    }

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task<GetUserByUserNameResponse> GetByUserNameAsync(string userName)
    {
        var result = await Query()
            .Where(p => p.UserName == userName)
            .ToOneAsync(p => new GetUserByUserNameResponse
            {
                Id = p.Id,
                UserName = p.UserName,
                RealName = p.RealName,
                Password = p.Password,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                Status = p.Status
            });

        if (result == null)
        {
            throw FriendlyException.Of("登录名或密码错误");
        }

        result.OrganizationName = await QueryNoTracking<OrganizationUser>()
            .Where(p => p.UserId == result.Id)
            .ToListAsync(p => p.Organization.Name);

        // 读取角色
        var roleIds = Query<RoleUser>()
            .As("e")
            .Where(p => p.UserId == result.Id);

        // 读取组织架构用户
        var organizationIds = await Query<OrganizationUser>()
            .Where(p => p.UserId == result.Id)
            .ToListAsync(p => p.OrganizationId);
        // 组织架构Id
        var organizationIdList = new List<string>();
        // 读取下级组织架构Id列表
        foreach (var organizationId in organizationIds)
        {
            var itemIds = await Query<Organization>()
                .Where(p => p.ParentPath.Contains(organizationId))
                .ToListAsync(p => p.Id);
            if (itemIds.Count > 0)
            {
                organizationIdList.AddRange(itemIds);
            }
        }

        // 读取组织架构角色
        var roleIds1 = Query<OrganizationRole>()
            .As("b")
            .Where(p => organizationIdList.Contains(p.OrganizationId));

        // 读取角色你信息
        var roles = await QueryNoTracking<Role>()
            .Where(p =>
                roleIds.Any(e => e.RoleId == p.Id) ||
                roleIds1.Any(b => b.RoleId == p.Id)
            )
            .ToListAsync<RoleModel>();
        result.Roles.AddRange(roles);
        return result;
    }

    /// <summary>
    /// 根据角色Id读取用户列表
    /// </summary>
    /// <param name="roleId">角色Id</param>
    /// <returns></returns>
    public async Task<List<string?>> GetIdsByRoleIdAsync(string roleId)
    {
        var list = new List<string>();
        var userIds = await Query<RoleUser>()
            .Where(p => p.RoleId == roleId)
            .ToListAsync(p => p.UserId);
        list.AddRange(userIds);

        var organizationIds = Query<OrganizationRole>()
            .Where(p => p.RoleId == roleId);

        var userIds1 = await Query<OrganizationUser>()
            .Where(p => organizationIds.Any(c => c.OrganizationId == p.OrganizationId))
            .ToListAsync(p => p.UserId);
        list.AddRange(userIds1);

        var result = list.GroupBy(userId => userId)
            .Select(userId => userId.FirstOrDefault())
            .ToList();

        if (result.Count == 0)
        {
            throw FriendlyException.Of("该角色下无用户信息");
        }

        return result;
    }

    /// <summary>
    /// 读取当前用户信息
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FriendlyException"></exception>
    public async Task<GetCurrentUserResponse> GetCurrentUserAsync()
    {
        if (string.IsNullOrEmpty(UserId))
        {
            throw FriendlyException.Of("用户未登录");
        }

        var result = await QueryableNoTracking
            .Where(p => p.Id == UserId)
            .ToOneAsync(p => new GetCurrentUserResponse
            {
                OrganizationName = p.Organization!.Name,
                PositionName = p.Position!.Name
            });

        if (result == null)
        {
            throw FriendlyException.Of("找不到数据");
        }

        // 用户拥有的资源代码
        result.ResourceCodes = await GetResourceCodesAsync(UserId);

        return result;
    }

    /// <summary>
    /// 读取树形选择框数据列表(系统用户)
    /// </summary>
    /// <returns></returns>
    public async Task<List<SelectViewModel>> GetSelectListAsync(string? organizationId)
    {
        ISelect<Organization>? organizationQuery = null;
        if (organizationId != null)
        {
            organizationQuery = QueryNoTracking<Organization>()
                .As("o")
                // 当前组织架构及下级组织架构
                .Where(p => p.ParentPath.Contains(organizationId) || p.Id == organizationId);
        }

        var result = await Queryable
            .Where(p => p.Status == Status.Enabled)
            .HasWhere(organizationQuery, p => organizationQuery!.Any(o => o.Id == p.OrganizationId))
            .ToListAsync(t1 => new SelectViewModel
            {
                Label = t1.RealName,
                Value = t1.Id,
                Disabled = t1.Status == Status.Disabled,
                Extend = t1.UserName
            });

        return result;
    }

    /// <summary>
    /// 读取组织架构用户树形列表
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FriendlyException"></exception>
    public async Task<List<CascaderViewModel>> GetOrganizationUserTreeSelectListAsync()
    {
        var res = await QueryNoTracking<OrganizationUser>()
            .Where(p => QueryNoTracking<Organization>()
                .As("o")
                .Any(o => o.Id == p.OrganizationId)
            )
            .ToListAsync(p => new
            {
                p.User.RealName,
                p.UserId,
                OrganizationName = p.Organization.Name,
                p.OrganizationId,
                OrganizationParentId = p.Organization.ParentId,
                p.Organization.ParentPath
            });

        var orgIds = new List<string?>();
        foreach (var item in res)
        {
            orgIds.AddRange(item.ParentPath.Split(','));
            orgIds.Add(item.OrganizationId);
        }

        orgIds = orgIds.GroupBy(p => p).Select(p => p.Key).ToList();

        // 读取组织架构列表
        var organizationList = await QueryNoTracking<Organization>()
            .Where(p => orgIds.Contains(p.Id) || orgIds.Contains(p.ParentId))
            .ToListAsync();
        var list = new List<dynamic>();
        list.AddRange(res);
        var result = new List<CascaderViewModel>();
        var parentId = list.MinBy(p => p.ParentPath.Length)?.OrganizationParentId;
        GetTreeChildren(organizationList, list, result, parentId, true);

        return result;
    }


    /// <summary>
    /// 读取组织架构用户树形列表
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FriendlyException"></exception>
    public async Task<List<CascaderViewModel>> GetOrganizationAndUserTreeSelectListAsync()
    {
        var res = await QueryNoTracking<OrganizationUser>()
            .Where(p => QueryNoTracking<Organization>().As("o")
                .Any(o => o.Id == p.OrganizationId)
            )
            .ToListAsync(p => new
            {
                p.User.RealName,
                p.UserId,
                OrganizationName = p.Organization.Name,
                p.OrganizationId,
                OrganizationParentId = p.Organization.ParentId,
                p.Organization.ParentPath
            });

        var orgIds = new List<string?>();
        foreach (var item in res)
        {
            orgIds.AddRange(item.ParentPath.Split(','));
            orgIds.Add(item.OrganizationId);
        }

        orgIds = orgIds.GroupBy(p => p).Select(p => p.Key).ToList();

        // 读取组织架构列表
        var organizationList = await QueryNoTracking<Organization>()
            .Where(p => orgIds.Contains(p.Id) || orgIds.Contains(p.ParentId))
            .ToListAsync();
        var list = new List<dynamic>();
        list.AddRange(res);
        var result = new List<CascaderViewModel>();
        var parentId = list.MinBy(p => p.ParentPath.Length)?.OrganizationParentId;
        GetTreeChildren(organizationList, list, result, parentId);

        return result;
    }

    /// <summary>
    /// 读取用户资源
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<ResourceModel>> GetUserResourceAsync(string? userId)
    {
        userId ??= UserId;
        var result = await GetResourceCodeInfoAsync(userId!);
        // 去重
        return result
            .RoleResources
            .Union(result.UserResources)
            .ToList();
    }

    /// <summary>
    /// 读取当前登录用户外部页面列表
    /// </summary>
    /// <returns></returns>
    public async Task<IList<ExternalPageModel>> GetCurrentUserExternalPagesAsync()
    {
        // 读取公共的和自己创建的
        var result = await QueryNoTracking<ExternalPage>()
            .Where(p => p.OwnerId == null || p.OwnerId == UserId)
            .ToListAsync<ExternalPageModel>();
        return result;
    }

    /// <summary>
    /// 读取用户资源代码
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<GetUserResourceCodeInfoResponse> GetResourceCodeInfoAsync(string userId)
    {
        var roleResources = new List<ResourceModel>();
        // 用户拥有的角色
        var roleIds = await _roleQueryService.GetRoleIdsByUserIdAsync(userId);
        if (roleIds.Count > 0)
        {
            // 角色资源
            roleResources = await QueryNoTracking<RoleResource>()
                .Where(p => roleIds.Contains(p.RoleId))
                .ToListAsync(p => new ResourceModel
                {
                    Key = p.ResourceKey,
                    Code = p.ResourceCode
                });
        }

        // 用户资源
        var userResources = await QueryNoTracking<UserResource>()
            .Where(p => p.UserId == userId)
            // 读取未过期的
            .Where(p => p.ExpireAt == null || p.ExpireAt > DateTime.Now)
            .ToListAsync(p => new ResourceModel
            {
                Key = p.ResourceKey,
                Code = p.ResourceCode
            });

        return new GetUserResourceCodeInfoResponse
        {
            RoleResources = roleResources,
            UserResources = userResources
        };
    }

    /// <summary>
    /// 读取用户资源代码列表
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<string>> GetResourceCodesAsync(string userId)
    {
        var userResources = await GetUserResourceAsync(userId);
        var list = new List<string>();
        foreach (var model in userResources)
        {
            list.AddRange(model.Codes);
        }

        return list;
    }

    /// <summary>
    /// 读取当前用户自定表格列列表
    /// </summary>
    /// <param name="moduleName">模块名</param>
    /// <returns></returns>
    public Task<List<UserCustomColumnModel>> GetCurrentUserCustomColumnsAsync(string moduleName)
    {
        return QueryNoTracking<UserCustomColumn>()
            .Where(p => p.ModuleName == moduleName)
            .Where(p => p.UserId == UserId)
            .ToListAsync<UserCustomColumnModel>();
    }

    /// <summary>
    /// 递归读取
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="list"></param>
    /// <param name="results"></param>
    /// <param name="parentId"></param>
    /// <param name="onlyUser"></param>
    private static void GetTreeChildren(IList<Organization> entities,
        IList<dynamic> list,
        ICollection<CascaderViewModel> results,
        string parentId = "",
        bool onlyUser = false)
    {
        var result = string.IsNullOrEmpty(parentId)
            ? entities.Where(p => string.IsNullOrEmpty(p.ParentId)).ToList()
            : entities.Where(p => p.ParentId == parentId).ToList();

        foreach (var item in result)
        {
            var res = new CascaderViewModel
            {
                Label = item.Name,
                Value = $"@{item.Id}",
            };
            if (entities.Any(p => p.ParentId == item.Id))
            {
                res.Children = new List<CascaderViewModel>();
                GetTreeChildren(entities, list, res.Children, item.Id, onlyUser);
            }

            if (onlyUser)
            {
                if (list.All(p => p.OrganizationId != item.Id))
                {
                    continue;
                }

                results.Add(res);
            }
            else
            {
                results.Add(res);
                if (list.All(p => p.OrganizationId != item.Id))
                {
                    continue;
                }
            }

            res.Children ??= new List<CascaderViewModel>();
            res.Children.AddRange(list
                .Where(p => p.OrganizationId == item.Id)
                .Select(p => new CascaderViewModel
                {
                    Label = p.RealName,
                    Value = p.UserId,
                }).ToList());
        }
    }
}