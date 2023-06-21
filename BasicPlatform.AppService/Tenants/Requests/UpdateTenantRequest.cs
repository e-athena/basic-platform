using BasicPlatform.AppService.Tenants.Models;

namespace BasicPlatform.AppService.Tenants.Requests;

/// <summary>
/// 更新租户请求类
/// </summary>
public class UpdateTenantRequest : TenantModel, ITxRequest<string>
{
}