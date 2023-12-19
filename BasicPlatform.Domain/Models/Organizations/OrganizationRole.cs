using BasicPlatform.Domain.Models.Roles;

namespace BasicPlatform.Domain.Models.Organizations;

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
    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public virtual Organization Organization { get; set; } = null!;

    /// <summary>
    /// 角色ID
    /// </summary>
    [MaxLength(36)]
    public string RoleId { get; set; } = null!;

    /// <summary>
    /// 角色
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public virtual Role Role { get; set; } = null!;


    /// <summary>
    /// 
    /// </summary>
    public OrganizationRole()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationId"></param>
    /// <param name="roleId"></param>
    public OrganizationRole(string organizationId, string roleId)
    {
        OrganizationId = organizationId;
        RoleId = roleId;
    }
}