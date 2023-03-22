using BasicPlatform.AppService.Positions.Models;

namespace BasicPlatform.AppService.Positions.Responses;

/// <summary>
/// 读取职位分页列表响应类
/// </summary>
public class GetPositionPagingResponse : PositionQueryModel
{
    /// <summary>
    /// 组织名称
    /// </summary>
    [TableColumn(Width = 150, Sort = 0)]
    public string? OrganizationName { get; set; }
}