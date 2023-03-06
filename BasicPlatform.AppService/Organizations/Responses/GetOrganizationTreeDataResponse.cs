using BasicPlatform.AppService.Organizations.Models;

namespace BasicPlatform.AppService.Organizations.Responses;

/// <summary>
/// 读取组织架构树形数据响应类
/// </summary>
public class GetOrganizationTreeDataResponse : OrganizationModel
{
    /// <summary>
    /// 子项
    /// </summary>
    public IList<GetOrganizationTreeDataResponse>? Children { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedUserName { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedUserName { get; set; }
}