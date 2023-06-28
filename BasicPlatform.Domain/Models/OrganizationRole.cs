using BasicPlatform.Domain.Models.Roles;

namespace BasicPlatform.Domain.Models;

/// <summary>
/// 组织架构角色
/// </summary>
[Table("authority_organization_roles")]
public class OrganizationRole : ValueObject
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
    /// 角色ID
    /// </summary>
    [MaxLength(36)]
    public string RoleId { get; set; } = null!;

    /// <summary>
    /// 角色
    /// </summary>
    public virtual Role Role { get; set; } = null!;


    public OrganizationRole()
    {
    }

    public OrganizationRole(string organizationId, string roleId)
    {
        OrganizationId = organizationId;
        RoleId = roleId;
    }
}