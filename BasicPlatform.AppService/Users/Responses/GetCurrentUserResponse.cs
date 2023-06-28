using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Responses;

/// <summary>
/// 读取当前用户响应类
/// </summary>
public class GetCurrentUserResponse : UserQueryModel
{
    /// <summary>
    /// 资源编码列表
    /// </summary>
    public List<string> ResourceCodes { get; set; } = new();

    /// <summary>
    /// 组织名称
    /// </summary>
    public string? OrganizationName { get; set; }

    /// <summary>
    /// 职位名称
    /// </summary>
    public string? PositionName { get; set; }
    
    /// <summary>
    /// 组织架构路径
    /// </summary>
    public string OrganizationPath { get; set; } = null!;

    /// <summary>
    /// 角色
    /// </summary>
    public List<string> RoleIds { get; set; } = new();
}