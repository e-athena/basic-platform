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
    public TenantQueryService(IFreeSql freeSql) : base(freeSql)
    {
    }

    public Task<Paging<GetTenantPagingResponse>> GetPagingAsync(GetTenantPagingRequest request,
        CancellationToken cancellationToken = default)
    {
        return QueryableNoTracking
            .HasWhere(request.Keyword, p => p.Name.Contains(request.Keyword!))
            .ToPagingAsync(request, p => new GetTenantPagingResponse(), cancellationToken);
    }

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
                    .ToList(x => new TenantApplicationModel
                    {
                        ApplicationName = x.Application.Name
                    })
            }, cancellationToken);
        if (entity is null)
        {
            throw FriendlyException.Of("租户不存在");
        }

        // 读取应用列表
        var applications = await QueryNoTracking<Application>()
            .Where(p => p.Status == Status.Enabled)
            .ToListAsync(cancellationToken);

        var tenantApplications = new List<TenantApplicationModel>();

        foreach (var app in applications)
        {
            var item = entity
                .Applications
                .FirstOrDefault(p => p.ApplicationId == app.Id) ?? new TenantApplicationModel
            {
                TenantId = id,
                ApplicationId = app.Id,
                ApplicationName = app.Name
            };
            item.ConnectionString = string.IsNullOrEmpty(item.ConnectionString)
                ? string.Empty
                : SecurityHelper.Decrypt(item.ConnectionString);
            tenantApplications.Add(item);
        }

        entity.Applications = tenantApplications;
        entity.ConnectionString = SecurityHelper.Decrypt(entity.ConnectionString) ?? string.Empty;
        return entity;
    }
}