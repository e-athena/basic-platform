using BasicPlatform.AppService.Roles;
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
            .ToPagingAsync(request, p => new GetRolePagingResponse
            {
                CreatedUserName = p.CreatedUser!.RealName,
                UpdatedUserName = p.UpdatedUser!.RealName
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
        entity.ResourceCodes = await QueryNoTracking<RoleResource>()
            .Where(p => p.RoleId == id)
            .ToListAsync(p => p.ResourceCode, cancellationToken);

        return entity;
    }

    /// <summary>
    /// 读取下拉列表
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<List<SelectViewModel>> GetSelectListAsync(CancellationToken cancellationToken = default)
    {
        return QueryableNoTracking
            .ToListAsync(p => new SelectViewModel
            {
                Disabled = p.Status == Status.Disabled,
                Label = p.Name,
                Value = p.Id
            }, cancellationToken);
    }
}