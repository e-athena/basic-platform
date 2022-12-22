namespace BasicPlatform.Domain.Models;

/// <summary>
/// 组织架构用户
/// </summary>
[Table("AuthorityOrganizationUsers")]
public class OrganizationUser : ValueObject
{
    /// <summary>
    /// 组织架构ID
    /// </summary>
    [MaxLength(36)]
    public string OrganizationId { get; set; } = null!;

    /// <summary>
    /// 组织架构
    /// </summary>
    public virtual Organization Organization { get; set; } = null!;

    /// <summary>
    /// 用户ID
    /// </summary>
    [MaxLength(36)]
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 用户
    /// </summary>
    public virtual User User { get; set; } = null!;

    public OrganizationUser()
    {
    }

    public OrganizationUser(string organizationId, string userId)
    {
        OrganizationId = organizationId;
        UserId = userId;
    }
}