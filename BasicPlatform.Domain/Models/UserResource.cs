namespace BasicPlatform.Domain.Models;

/// <summary>
/// 用户资源
/// </summary>
[Table("authority_user_resources")]
public class UserResource : ValueObject
{
    /// <summary>
    /// 用户ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 用户
    /// </summary>
    /// <value></value>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// 资源ID
    /// </summary>
    /// <value></value>
    [MaxLength(36)]
    public string ResourceId { get; set; } = null!;

    /// <summary>
    /// 资源
    /// </summary>
    /// <value></value>
    public virtual Resource Resource { get; set; } = null!;

    public UserResource()
    {
    }

    public UserResource(string userId, string resourceId)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        ResourceId = resourceId ?? throw new ArgumentNullException(nameof(resourceId));
    }
}