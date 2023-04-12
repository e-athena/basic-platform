using BasicPlatform.AppService.Applications.Models;
using BasicPlatform.AppService.Applications.Requests;
using BasicPlatform.AppService.Applications.Responses;

namespace BasicPlatform.AppService.Applications;

/// <summary>
/// 网站系统应用查询接口服务
/// </summary>
public interface IApplicationQueryService
{
    /// <summary>
    /// 读取列表
    /// </summary>
    /// <returns></returns>
    Task<List<ApplicationModel>> GetListAsync();

    /// <summary>
    /// 读取分页列表
    /// </summary>
    /// <param name="request">请求类</param>
    /// <returns></returns>
    Task<Paging<GetApplicationPagingResponse>> GetPagingAsync(GetApplicationPagingRequest request);

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns></returns>
    Task<ApplicationModel?> GetAsync(string id);

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="clientId">客户端ID</param>
    /// <returns></returns>
    Task<ApplicationModel?> GetByClientIdAsync(string clientId);
}