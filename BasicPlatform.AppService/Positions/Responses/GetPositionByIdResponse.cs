using BasicPlatform.AppService.Positions.Models;

namespace BasicPlatform.AppService.Positions.Responses;

/// <summary>
/// 读取角色信息
/// </summary>
public class GetPositionByIdResponse : PositionQueryModel
{
    /// <summary>
    /// 组织/部门路径
    /// </summary>
    public string? OrganizationPath { get; set; }
}