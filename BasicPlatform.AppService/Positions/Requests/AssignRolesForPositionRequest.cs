namespace BasicPlatform.AppService.Positions.Requests;

/// <summary>
/// 给职位分配角色请求类
/// </summary>
public class AssignRolesForPositionRequest : ITxRequest<string>
{
    /// <summary>
    /// 职位Id
    /// </summary>
    public string PositionId { get; set; } = string.Empty;

    /// <summary>
    /// 角色Id列表
    /// </summary>
    public IList<string>? RoleIds { get; set; }
}