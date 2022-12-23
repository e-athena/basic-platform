namespace BasicPlatform.Domain.Models;

/// <summary>
/// 模块资源
/// </summary>
[Table("authority_module_resources")]
public class ModuleResource : ValueObject
{
    /// <summary>
    /// 模块ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string ModuleId { get; set; } = null!;

    /// <summary>
    /// 模块
    /// </summary>
    /// <value></value>
    public virtual Module Module { get; set; } = null!;

    /// <summary>
    /// 资源ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string ResourceId { get; set; } = null!;

    /// <summary>
    /// 权限
    /// </summary>
    /// <value></value>
    public virtual Resource Resource { get; set; } = null!;


    public ModuleResource()
    {
    }

    public ModuleResource(string menuId, string resourceId)
    {
        ResourceId = resourceId ?? throw new ArgumentNullException(nameof(resourceId));
        ModuleId = menuId ?? throw new ArgumentNullException(nameof(menuId));
    }
}