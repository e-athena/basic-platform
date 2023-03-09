using BasicPlatform.AppService.Positions;
using BasicPlatform.AppService.Positions.Requests;
using BasicPlatform.AppService.Positions.Responses;

namespace BasicPlatform.AppService.FreeSql.Positions;

/// <summary>
/// 职位查询服务接口实现类
/// </summary>
[Component]
public class PositionQueryService : QueryServiceBase<Position>, IPositionQueryService
{
    public PositionQueryService(IFreeSql freeSql) : base(freeSql)
    {
    }

    /// <summary>
    /// 读取分页数据
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Paging<GetPositionPagingResponse>> GetPagingAsync(GetPositionPagingRequest request,
        CancellationToken cancellationToken = default)
    {
        ISelect<Organization>? organizationQuery = null;
        if (request.OrganizationId != null)
        {
            organizationQuery = QueryNoTracking<Organization>()
                .As("o")
                // 当前组织架构及下级组织架构
                .Where(p => p.ParentPath.Contains(request.OrganizationId!) || p.Id == request.OrganizationId);
        }
        var result = await QueryableNoTracking
            .HasWhere(request.Keyword, p => p.Name.Contains(request.Keyword!))
            .HasWhere(organizationQuery, p => organizationQuery!.Any(o => o.Id == p.OrganizationId))
            .ToPagingAsync(request, p => new GetPositionPagingResponse
            {
                CreatedUserName = p.CreatedUser!.RealName,
                UpdatedUserName = p.UpdatedUser!.RealName,
                OrganizationName = p.Organization.Name
            }, cancellationToken);
        return result;
    }

    /// <summary>
    /// 读取详情
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GetPositionByIdResponse> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var entity = await QueryableNoTracking
            .Where(p => p.Id == id)
            .FirstAsync<GetPositionByIdResponse>(cancellationToken);
        if (entity is null)
        {
            throw FriendlyException.Of("角色不存在");
        }

        return entity;
    }

    /// <summary>
    /// 读取下拉列表
    /// </summary>
    /// <param name="organizationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<SelectViewModel>> GetSelectListAsync(string? organizationId,
        CancellationToken cancellationToken = default)
    {
        if (organizationId == null)
        {
            return new List<SelectViewModel>();
        }

        return await QueryableNoTracking
            .Where(p => p.OrganizationId == organizationId)
            .ToListAsync(p => new SelectViewModel
            {
                Disabled = p.Status == Status.Disabled,
                Label = p.Name,
                Value = p.Id
            }, cancellationToken);
    }
}