using BasicPlatform.AppService.Roles.Models;
using BasicPlatform.AppService.Roles.Requests;
using BasicPlatform.AppService.Roles.Responses;

namespace BasicPlatform.AppService.Roles;

/// <summary>
/// 角色查询服务接口
/// </summary>
public interface IRoleQueryService
{
    /// <summary>
    /// 读取分页信息
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Paging<GetRolePagingResponse>> GetPagingAsync(GetRolePagingRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetRoleByIdResponse> GetAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取下拉列表数据
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<SelectViewModel>> GetSelectListAsync(CancellationToken cancellationToken = default);
}