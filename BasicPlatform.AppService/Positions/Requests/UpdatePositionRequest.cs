using BasicPlatform.AppService.Positions.Models;

namespace BasicPlatform.AppService.Positions.Requests;

/// <summary>
/// 更新职位请求类
/// </summary>
public class UpdatePositionRequest : PositionModel,ITxRequest<string>
{
    /// <summary>
    /// 角色Ids
    /// </summary>
    public IList<string> RoleIds { get; set; } = new List<string>();
}