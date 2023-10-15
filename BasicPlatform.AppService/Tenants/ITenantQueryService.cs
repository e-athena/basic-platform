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

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="code">编码</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetTenantDetailResponse> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取连接字符串
    /// </summary>
    /// <param name="code"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    Task<string> GetConnectionStringAsync(string code, string appId);

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="code"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    Task<TenantInfo> GetAsync(string code, string appId);
}