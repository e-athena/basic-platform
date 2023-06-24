using BasicPlatform.AppService.Applications;
using BasicPlatform.AppService.Applications.Models;
using BasicPlatform.AppService.Applications.Requests;
using BasicPlatform.AppService.Applications.Responses;
using BasicPlatform.Domain.Models.Tenants;

namespace BasicPlatform.AppService.FreeSql.Applications;

/// <summary>
/// 网站系统应用查询接口服务实现类
/// </summary>
[Component]
public class ApplicationQueryService : AppQueryServiceBase<Application>, IApplicationQueryService
{
    private readonly ICacheManager _cacheManager;

    public ApplicationQueryService(FreeSqlMultiTenancy freeSql, ISecurityContextAccessor accessor,
        ICacheManager cacheManager) :
        base(freeSql, accessor)
    {
        _cacheManager = cacheManager;
    }

    /// <summary>
    /// 读取密钥
    /// </summary>
    /// <param name="clientId">客户端ID</param>
    /// <returns></returns>
    public string? GetSecret(string clientId)
    {
        // 缓存30分钟
        var cacheKey = $"sso:application:secret:{clientId}";
        return _cacheManager.GetOrCreate(cacheKey, () =>
        {
            return DefaultQueryableNoTracking
                .Where(p => p.ClientId == clientId)
                .ToOne(p => p.ClientSecret);
        }, TimeSpan.FromMinutes(30));
    }

    /// <summary>
    /// 读取列表
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<ApplicationModel>> GetListAsync()
    {
        // 如果是租户环境
        if (IsTenantEnvironment)
        {
            // 读取租户应用
            return await DefaultQueryNoTracking<TenantApplication>()
                .Where(p => p.IsEnabled)
                .Where(p => p.Application.Status == Status.Enabled)
                .ToListAsync(p => new ApplicationModel
                {
                    Id = p.Application.Id,
                    Name = p.Application.Name,
                    ClientId = p.Application.ClientId,
                    ClientSecret = p.Application.ClientSecret,
                    UseDefaultClientSecret = p.Application.UseDefaultClientSecret,
                    FrontendUrl = p.Application.FrontendUrl,
                    ApiUrl = p.Application.ApiUrl,
                    MenuResourceRoute = p.Application.MenuResourceRoute,
                    PermissionResourceRoute = p.Application.PermissionResourceRoute,
                    Remarks = p.Application.Remarks
                });
        }

        return await QueryableNoTracking
            .Where(p => p.Status == Status.Enabled)
            .ToListAsync<ApplicationModel>();
    }

    /// <summary>
    /// 读取下拉列表
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<List<SelectViewModel>> GetSelectListAsync()
    {
        return QueryableNoTracking
            .ToListAsync(p => new SelectViewModel
            {
                Value = p.Id,
                Label = p.Name,
                Disabled = p.Status == Status.Disabled
            });
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
    public async Task<ApplicationModel?> GetByClientIdAsync(string clientId)
    {
        // 如果是租户环境
        if (IsTenantEnvironment)
        {
            // 读取租户应用
            return await DefaultQueryNoTracking<TenantApplication>()
                .Where(p => p.Application.ClientId == clientId)
                .ToOneAsync(p => new ApplicationModel
                {
                    Id = p.Application.Id,
                    Name = p.Application.Name,
                    ClientId = p.Application.ClientId,
                    ClientSecret = p.Application.ClientSecret,
                    UseDefaultClientSecret = p.Application.UseDefaultClientSecret,
                    FrontendUrl = p.Application.FrontendUrl,
                    ApiUrl = p.Application.ApiUrl,
                    MenuResourceRoute = p.Application.MenuResourceRoute,
                    PermissionResourceRoute = p.Application.PermissionResourceRoute,
                    Remarks = p.Application.Remarks
                });
        }

        return await QueryableNoTracking
            .Where(p => p.ClientId == clientId)
            .ToOneAsync<ApplicationModel>()!;
    }
}