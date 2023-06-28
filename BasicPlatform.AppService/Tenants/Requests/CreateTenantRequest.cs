using BasicPlatform.AppService.Tenants.Models;

namespace BasicPlatform.AppService.Tenants.Requests;

/// <summary>
/// 创建租户请求类
/// </summary>
public class CreateTenantRequest : TenantModel, ITxRequest<string>
{
}