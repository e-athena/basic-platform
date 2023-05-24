using Athena.Infrastructure.Attributes;
using Athena.Infrastructure.Jwt;
using Athena.Infrastructure.Messaging.Responses;
using Athena.Infrastructure.ViewModels;

namespace App.Infrastructure.Services.Impls;

/// <summary>
/// 角色服务接口默认实现
/// </summary>
[Component]
public class DefaultPositionService : DefaultServiceBase, IPositionService
{
    private const string ApiUrl = "http://localhost:5078";

    public DefaultPositionService(ISecurityContextAccessor accessor) : base(accessor)
    {
    }

    /// <summary>
    /// 读取下拉列表
    /// </summary>
    /// <param name="organizationId"></param>
    /// <returns></returns>
    public async Task<List<SelectViewModel>> GetSelectListAsync(string? organizationId = null)
    {
        var url = $"{ApiUrl}/api/SubApplication/GetPositionSelectList";
        if (!string.IsNullOrEmpty(organizationId))
        {
            url += $"?organizationId={organizationId}";
        }

        var result = await GetRequest(url)
            .GetJsonAsync<ApiResult<List<SelectViewModel>>>();
        return result.Data ?? new List<SelectViewModel>();
    }
}