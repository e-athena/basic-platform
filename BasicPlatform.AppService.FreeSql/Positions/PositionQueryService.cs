using BasicPlatform.AppService.Positions;
using BasicPlatform.AppService.Positions.Requests;
using BasicPlatform.AppService.Positions.Responses;

namespace BasicPlatform.AppService.FreeSql.Positions;

/// <summary>
/// 职位查询服务接口实现类
/// </summary>
[Component]
public class PositionQueryService : DataPermissionQueryServiceBase<Position>, IPositionQueryService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="freeSql"></param>
    /// <param name="accessor"></param>
    public PositionQueryService(
        IFreeSql freeSql,
        ISecurityContextAccessor accessor) : base(freeSql, accessor)
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
            organizationQuery = QuerySkipPermission<Organization>()
                .As("o")
                // 当前组织架构及下级组织架构
                .Where(p => p.ParentPath.Contains(request.OrganizationId!) || p.Id == request.OrganizationId);
        }

        var result = await QueryableNoTracking
            .HasWhere(request.Keyword, p => p.Name.Contains(request.Keyword!))
            .HasWhere(organizationQuery, p => organizationQuery!.Any(o => o.Id == p.OrganizationId))
            .ToPagingAsync(UserId, request, p => new GetPositionPagingResponse
            {
                CreatedUserName = p.CreatedUser!.RealName,
                UpdatedUserName = p.LastUpdatedUser!.RealName,
                OrganizationName = p.Organization!.Name
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
        var result = await QueryableNoTracking
            .Where(p => p.Id == id)
            .FirstAsync(p => new GetPositionByIdResponse
            {
                OrganizationPath = p.Organization!.ParentPath
            }, cancellationToken);
        if (result is null)
        {
            throw FriendlyException.Of("职位不存在");
        }

        result.OrganizationPath = !string.IsNullOrEmpty(result.OrganizationPath)
            ? $"{result.OrganizationPath},{result.OrganizationId}"
            : result.OrganizationId;

        return result;
    }

    /// <summary>
    /// 读取下拉列表
    /// </summary>
    /// <param name="organizationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<SelectViewModel>> GetSelectListAsync(
        string? organizationId,
        CancellationToken cancellationToken = default
    )
    {
        return await QueryableSkipPermission
            .Where(p => p.OrganizationId == organizationId || p.OrganizationId == null)
            .ToListAsync(p => new SelectViewModel
            {
                Disabled = p.Status == Status.Disabled,
                Label = p.Name,
                Value = p.Id
            }, cancellationToken);
    }
}