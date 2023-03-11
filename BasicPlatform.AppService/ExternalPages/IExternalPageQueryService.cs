using BasicPlatform.AppService.ExternalPages.Requests;
using BasicPlatform.AppService.ExternalPages.Responses;

namespace BasicPlatform.AppService.ExternalPages;

/// <summary>
/// 
/// </summary>
public interface IExternalPageQueryService
{
    /// <summary>
    /// 读取分页列表
    /// </summary>
    /// <param name="request">请求类</param>
    /// <returns></returns>
    Task<Paging<GetExternalPagePagingResponse>> GetPagingAsync(GetExternalPagePagingRequest request);

    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns></returns>
    Task<GetExternalPageByIdResponse?> GetAsync(string id);

    /// <summary>
    /// 读取选择框数据列表
    /// </summary>
    /// <returns></returns>
    Task<List<SelectViewModel>> GetSelectListAsync();

    /// <summary>
    /// 读取树形数据列表
    /// </summary>
    /// <returns></returns>
    Task<List<TreeViewModel>> GetTreeListAsync();

}