namespace BasicPlatform.AppService.Tenants.Requests;

/// <summary>
/// 变更租户状态请求类
/// </summary>
public class ChangeTenantStatusRequest : IdRequest, ITxRequest<string>
{
}