using BasicPlatform.Infrastructure.Enums;

namespace BasicPlatform.Domain.Models;

/// <summary>
/// 资源
/// </summary>
[Table("authority_resources")]
// ReSharper disable once ClassNeverInstantiated.Global
public class Resource : EntityCore, ICreator
{
    /// <summary>
    /// 资源类型
    /// </summary>
    /// <value></value>
    public ResourceType ResourceType { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedUserId { get; set; }

    public Resource()
    {
    }

    public Resource(ResourceType resourceType, string? userId)
    {
        ResourceType = resourceType;
        CreatedUserId = userId;
    }
}