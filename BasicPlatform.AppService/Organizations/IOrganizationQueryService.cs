using BasicPlatform.AppService.Organizations.Requests;
using BasicPlatform.AppService.Organizations.Responses;

namespace BasicPlatform.AppService.Organizations;

/// <summary>
/// 
/// </summary>
public interface IOrganizationQueryService
{
    /// <summary>
    /// 读取分页列表
    /// </summary>
    /// <param name="request">请求类</param>
    /// <returns></returns>
    Task<Page<GetOrganizationPagesResponse>> GetAsync(GetOrganizationPagesRequest request);
    
    /// <summary>
    /// 读取信息
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns></returns>
    Task<GetOrganizationByIdResponse?> GetAsync(string id);

    /// <summary>
    /// 读取树形数据
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<List<GetOrganizationTreeDataResponse>> GetTreeDataAsync(GetOrganizationTreeDataRequest request);

    /// <summary>
    /// 读取树形数据列表
    /// </summary>
    /// <returns></returns>
    Task<List<TreeViewModel>> GetTreeDataAsync();

    /// <summary>
    /// 读取树形选择框数据列表
    /// </summary>
    /// <returns></returns>
    Task<List<TreeSelectViewModel>> GetTreeSelectDataAsync();

    /// <summary>
    /// 读取树形选择框数据列表
    /// </summary>
    /// <returns></returns>
    Task<List<TreeSelectViewModel>> GetTreeSelectDataForSelfAsync();

    /// <summary>
    /// 获取组织架构级联人员信息
    /// </summary>
    /// <returns></returns>
    Task<List<CascaderViewModel>> GetCascaderDataAsync();
}