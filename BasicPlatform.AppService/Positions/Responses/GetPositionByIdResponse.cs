using BasicPlatform.AppService.Positions.Models;

namespace BasicPlatform.AppService.Positions.Responses;

/// <summary>
/// 读取职位信息
/// </summary>
public class GetPositionByIdResponse : PositionModel
{
    /// <summary>
    /// 角色
    /// </summary>
    public List<string> RoleIds { get; set; } = new();
}