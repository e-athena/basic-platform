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
    /// 资源代码
    /// </summary>
    /// <value></value>
    [MaxLength(256)]
    public string ResourceCode { get; set; } = null!;

    /// <summary>
    /// 有效期至
    /// <remarks>为空时永久有效</remarks>
    /// </summary>
    public DateTime? ExpireAt { get; set; }

    public UserResource()
    {
    }

    public UserResource(string userId, string resourceCode, DateTime? expireAt = null)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        ResourceCode = resourceCode ?? throw new ArgumentNullException(nameof(resourceCode));
        ExpireAt = expireAt;
    }
}