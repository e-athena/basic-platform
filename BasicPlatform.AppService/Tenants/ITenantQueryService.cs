using BasicPlatform.AppService.Tenants.Models;
using BasicPlatform.AppService.Tenants.Requests;
using BasicPlatform.AppService.Tenants.Responses;

namespace BasicPlatform.AppService.Tenants;

/// <summary>
/// 租户查询服务
/// </summary>
public interface ITenantQueryService
{
    /// <summary>
    /// 读取分页信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Paging<GetTenantPagingResponse>> GetPagingAsync(GetTenantPagingRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetTenantDetailResponse> GetAsync(string id, CancellationToken cancellationToken = default);
}