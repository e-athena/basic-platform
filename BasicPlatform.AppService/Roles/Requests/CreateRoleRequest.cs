namespace BasicPlatform.AppService.Roles.Requests;

/// <summary>
/// 创建角色请求类
/// </summary>
public class CreateRoleRequest : ITxRequest<string>
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(1024)]
    public string? Remarks { get; set; }

    /// <summary>
    /// 资源代码列表
    /// </summary>
    public IList<string>? ResourceCodes { get; set; }
}