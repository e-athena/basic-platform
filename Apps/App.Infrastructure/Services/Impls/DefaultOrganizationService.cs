using Athena.Infrastructure.ViewModels;

namespace App.Infrastructure.Services.Impls;

/// <summary>
/// 组织架构服务接口默认实现
/// </summary>
[Component]
public class DefaultOrganizationService : DefaultServiceBase, IOrganizationService
{
    public DefaultOrganizationService(ISecurityContextAccessor accessor) : base(accessor)
    {
    }

    /// <summary>
    /// 读取级联列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<CascaderViewModel>> GetCascaderListAsync(string? parentId = null)
    {
        var url = "/api/SubApplication/GetOrganizationCascaderList";
        if (!string.IsNullOrEmpty(parentId))
        {
            url += $"?organizationId={parentId}";
        }

        var result = await GetRequest(url)
            .GetJsonAsync<ApiResult<List<CascaderViewModel>>>();
        return result.Data ?? new List<CascaderViewModel>();
    }

    /// <summary>
    /// 读取树形列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<TreeViewModel>> GetTreeListAsync(string? parentId = null)
    {
        var url = "/api/SubApplication/GetOrganizationTreeList";
        if (!string.IsNullOrEmpty(parentId))
        {
            url += $"?organizationId={parentId}";
        }

        var result = await GetRequest(url)
            .GetJsonAsync<ApiResult<List<TreeViewModel>>>();
        return result.Data ?? new List<TreeViewModel>();
    }

    /// <summary>
    /// 读取下拉列表
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public async Task<List<SelectViewModel>> GetSelectListAsync(string? parentId = null)
    {
        var url = "/api/SubApplication/GetOrganizationSelectList";
        if (!string.IsNullOrEmpty(parentId))
        {
            url += $"?organizationId={parentId}";
        }

        var result = await GetRequest(url)
            .GetJsonAsync<ApiResult<List<SelectViewModel>>>();
        return result.Data ?? new List<SelectViewModel>();
    }

    /// <summary>
    /// 读取Id
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    public async Task<string?> GetIdByNameAsync(string name)
    {
        const string url = "/api/SubApplication/GetOrganizationIdByName";
        var result = await GetRequest(url)
            .SetQueryParam("organizationName", name)
            .GetJsonAsync<ApiResult<string>>();
        return result.Data;
    }
}