using BasicPlatform.AppService.Positions.Requests;
using BasicPlatform.AppService.Positions.Responses;

namespace BasicPlatform.AppService.Positions;

/// <summary>
/// 职位查询服务接口
/// </summary>
public interface IPositionQueryService
{
    /// <summary>
    /// 读取分页信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Paging<GetPositionPagingResponse>> GetPagingAsync(GetPositionPagingRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetPositionByIdResponse> GetAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取下拉列表数据
    /// </summary>
    /// <param name="organizationId">组织架构ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<SelectViewModel>> GetSelectListAsync(string? organizationId,
        CancellationToken cancellationToken = default);
}