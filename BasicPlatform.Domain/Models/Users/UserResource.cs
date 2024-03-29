using BasicPlatform.Domain.Models.Applications;

namespace BasicPlatform.Domain.Models.Users;

/// <summary>
/// 用户资源
/// </summary>
[Table("authority_user_resources")]
public class UserResource : ValueObject
{
    /// <summary>
    /// 应用ID
    /// </summary>
    [MaxLength(36)]
    public string? AppId { get; set; }

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
    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// 资源Key
    /// </summary>
    [MaxLength(128)]
    public string ResourceKey { get; set; } = null!;

    /// <summary>
    /// 资源代码
    /// <remarks>多个用逗号分割</remarks>
    /// </summary>
    /// <value></value>
    [MaxLength(256)]
    public string ResourceCode { get; set; } = null!;

    /// <summary>
    /// 资源代码列表
    /// </summary>
    public IList<string> ResourceCodes =>
        string.IsNullOrEmpty(ResourceCode) ? new List<string>() : ResourceCode.Split(",");

    /// <summary>
    /// 有效期至
    /// <remarks>为空时永久有效</remarks>
    /// </summary>
    public DateTime? ExpireAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public UserResource()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="userId"></param>
    /// <param name="resourceKey"></param>
    /// <param name="resourceCode"></param>
    /// <param name="expireAt"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public UserResource(
        string? appId,
        string userId, string resourceKey, string resourceCode,
        DateTime? expireAt = null)
    {
        AppId = appId;
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        ResourceKey = resourceKey ?? throw new ArgumentNullException(nameof(resourceKey));
        ResourceCode = resourceCode ?? throw new ArgumentNullException(nameof(resourceCode));
        ExpireAt = expireAt;
    }
}