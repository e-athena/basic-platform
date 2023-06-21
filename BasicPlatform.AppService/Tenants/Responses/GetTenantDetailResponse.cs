using BasicPlatform.AppService.Tenants.Models;

namespace BasicPlatform.AppService.Tenants.Responses;

/// <summary>
/// 读取租户信息响应类
/// </summary>
public class GetTenantDetailResponse : TenantQueryModel
{
    /// <summary>
    /// 连接字符串
    /// </summary>
    [TableColumn(Sort = 10)]
    public override string ConnectionString { get; set; } = null!;

    /// <summary>
    /// 资源代码
    /// </summary>
    public List<ResourceModel> Resources { get; set; } = new();

    /// <summary>
    /// 租户应用
    /// </summary>
    public List<TenantApplicationModel> Applications { get; set; } = new();
}