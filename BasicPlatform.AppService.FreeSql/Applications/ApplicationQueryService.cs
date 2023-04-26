using BasicPlatform.AppService.Applications;
using BasicPlatform.AppService.Applications.Models;
using BasicPlatform.AppService.Applications.Requests;
using BasicPlatform.AppService.Applications.Responses;

namespace BasicPlatform.AppService.FreeSql.Applications;

/// <summary>
/// 网站系统应用查询接口服务实现类
/// </summary>
[Component]
public class ApplicationQueryService : AppQueryServiceBase<Application>, IApplicationQueryService
{
    public ApplicationQueryService(IFreeSql freeSql, ISecurityContextAccessor accessor) : base(freeSql, accessor)
    {
    }

    /// <summary>
    /// 读取列表
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<ApplicationModel>> GetListAsync()
    {
        return await QueryableNoTracking
            .ToListAsync<ApplicationModel>();
    }

    /// <summary>
    /// 读取分页数据
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Paging<GetApplicationPagingResponse>> GetPagingAsync(GetApplicationPagingRequest request)
    {
        var result = await QueryableNoTracking
            .HasWhere(request.Keyword, p => p.Name.Contains(request.Keyword!))
            .ToPagingAsync(request, p => new GetApplicationPagingResponse
            {
                CreatedUserName = p.CreatedUser!.RealName,
                UpdatedUserName = p.LastUpdatedUser!.RealName,
            });
        return result;
    }

    /// <summary>
    /// 读取详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ApplicationModel?> GetAsync(string id)
    {
        return QueryableNoTracking
            .Where(p => p.Id == id)
            .ToOneAsync<ApplicationModel>()!;
    }

    /// <summary>
    /// 读取详情
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<ApplicationModel?> GetByClientIdAsync(string clientId)
    {
        return QueryableNoTracking
            .Where(p => p.ClientId == clientId)
            .ToOneAsync<ApplicationModel>()!;
    }
}