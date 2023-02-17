namespace BasicPlatform.AppService.Organizations.Requests;

/// <summary>
/// 读取组织架构树形数据请求类
/// </summary>
public class GetOrganizationTreeDataRequest : GetRequestBase
{
    /// <summary>
    /// 状态
    /// </summary>
    public IList<string>? Status { get; set; }

    /// <summary>
    /// 包含的组织组织架构
    /// </summary>
    public List<string> InOrganization { get; set; } = new();
}