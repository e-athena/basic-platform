namespace BasicPlatform.AppService.Tenants.Requests;

/// <summary>
/// 初始化租户请求类
/// </summary>
public class InitTenantRequest : TxTraceRequest<string>
{
    /// <summary>
    /// 租户Code
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// 操作人
    /// </summary>
    public string? UserId { get; set; }
}