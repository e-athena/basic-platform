using BasicPlatform.AppService.Tenants;
using BasicPlatform.AppService.Tenants.Models;
using BasicPlatform.AppService.Tenants.Requests;
using BasicPlatform.AppService.Tenants.Responses;
using BasicPlatform.Domain.Models.Tenants;

namespace BasicPlatform.AppService.FreeSql.Tenants;

/// <summary>
/// 租户查询服务接口实现类
/// </summary>
[Component]
public class TenantQueryService : QueryServiceBase<Tenant>, ITenantQueryService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="multiTenancy"></param>
    /// <param name="accessor"></param>
    public TenantQueryService(FreeSqlMultiTenancy multiTenancy, ISecurityContextAccessor accessor) : base(multiTenancy,
        accessor)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Paging<GetTenantPagingResponse>> GetPagingAsync(GetTenantPagingRequest request,
        CancellationToken cancellationToken = default)
    {
        return QueryableNoTracking
            .HasWhere(request.Keyword, p => p.Name.Contains(request.Keyword!))
            .ToPagingAsync(UserId, request, p => new GetTenantPagingResponse(), cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="FriendlyException"></exception>
    public async Task<GetTenantDetailResponse> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var entity = await QueryableNoTracking.Where(p => p.Id == id)
            .FirstAsync(p => new GetTenantDetailResponse
            {
                Resources = QueryNoTracking<TenantResource>()
                    .Where(x => x.TenantId == p.Id)
                    .ToList(x => new ResourceModel
                    {
                        Key = x.ResourceKey,
                        Code = x.ResourceCode
                    }),
                Applications = QueryNoTracking<TenantApplication>()
                    .Where(x => x.TenantId == p.Id)
                    .ToList<TenantApplicationModel>()
            }, cancellationToken);
        if (entity is null)
        {
            throw FriendlyException.Of("租户不存在");
        }

        var clientIds = entity.Applications.Select(p => p.ApplicationClientId).ToList();

        // 读取应用列表
        var applications = await QueryNoTracking<Application>()
            .Where(p => p.Status == Status.Enabled)
            .Where(p => p.Environment == AspNetCoreEnvironment)
            .Where(p => clientIds.Contains(p.ClientId))
            .ToListAsync(cancellationToken);

        var tenantApplications = new List<TenantApplicationModel>();

        foreach (var group in applications.GroupBy(p => p.ClientId))
        {
            var app = group.First();
            var item = entity
                .Applications
                .FirstOrDefault(p => p.ApplicationClientId == app.ClientId) ?? new TenantApplicationModel
            {
                TenantId = id,
                ApplicationName = app.Name,
                IsolationLevel = TenantIsolationLevel.Shared
            };
            item.ConnectionString = string.IsNullOrEmpty(item.ConnectionString)
                ? string.Empty
                : SecurityHelper.Decrypt(item.ConnectionString);
            item.ApplicationName = app.Name;
            tenantApplications.Add(item);
        }

        entity.Applications = tenantApplications;
        entity.ConnectionString = SecurityHelper.Decrypt(entity.ConnectionString) ?? string.Empty;
        return entity;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="FriendlyException"></exception>
    public async Task<GetTenantDetailResponse> GetByCodeAsync(string code,
        CancellationToken cancellationToken = default)
    {
        var entity = await MainQueryableNoTracking.Where(p => p.Code == code)
            .FirstAsync(p => new GetTenantDetailResponse
            {
                Resources = MainQueryNoTracking<TenantResource>()
                    .Where(x => x.TenantId == p.Id)
                    .ToList(x => new ResourceModel
                    {
                        Key = x.ResourceKey,
                        Code = x.ResourceCode
                    }),
                Applications = MainQueryNoTracking<TenantApplication>()
                    .Where(x => x.TenantId == p.Id)
                    .ToList<TenantApplicationModel>()
            }, cancellationToken);
        if (entity is null)
        {
            throw FriendlyException.Of("租户不存在");
        }

        if (entity.Applications.Count > 0)
        {
            var clientIds = entity.Applications.Select(p => p.ApplicationClientId).ToList();
            var apps = await MainQueryNoTracking<Application>()
                .Where(p => p.Environment == AspNetCoreEnvironment)
                .Where(p => clientIds.Contains(p.ClientId))
                .ToListAsync(cancellationToken);
            if (apps.Count > 0)
            {
                foreach (var model in entity.Applications)
                {
                    var item = apps.FirstOrDefault(p => p.ClientId == model.ApplicationClientId);
                    if (item == null)
                    {
                        continue;
                    }

                    model.ApplicationClientId = item.ClientId;
                    model.ApplicationName = item.Name;
                    model.ApplicationApiUrl = item.ApiUrl;
                }
            }
        }

        entity.ConnectionString = SecurityHelper.Decrypt(entity.ConnectionString) ?? string.Empty;
        return entity;
    }

    /// <summary>
    /// 读取租户连接字符串
    /// </summary>
    /// <param name="code"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<string> GetConnectionStringAsync(string code, string appId)
    {
        return MainQueryNoTracking<TenantApplication>()
            .Where(p => p.Tenant.Code == code)
            .Where(p => p.ApplicationClientId == appId)
            .Where(p => p.IsEnabled)
            .ToOneAsync(p => p.ConnectionString!);
    }

    /// <summary>
    /// 读取租户连接字符串
    /// </summary>
    /// <param name="code"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<TenantInfo> GetAsync(string code, string appId)
    {
        return MainQueryNoTracking<TenantApplication>()
            .Where(p => p.Tenant.Code == code)
            .Where(p => p.ApplicationClientId == appId)
            .Where(p => p.IsEnabled)
            .ToOneAsync(p => new TenantInfo
            {
                ConnectionString = p.ConnectionString!,
                DbKey = code,
                IsolationLevel = p.IsolationLevel
            });
    }
}