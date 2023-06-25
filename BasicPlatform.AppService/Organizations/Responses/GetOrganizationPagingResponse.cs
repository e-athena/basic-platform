using BasicPlatform.AppService.Organizations.Models;

namespace BasicPlatform.AppService.Organizations.Responses;

/// <summary>
/// 组织架构分页返回值
/// </summary>
[DataPermission(typeof(Organization), "组织/部门管理模块", AppId = "system")]
public class GetOrganizationPagingResponse : OrganizationQueryModel
{
    /// <summary>
    /// 部门负责人
    /// </summary>
    [TableColumn(Sort = 1, Width = 120)]
    public string? LeaderName { get; set; }
}