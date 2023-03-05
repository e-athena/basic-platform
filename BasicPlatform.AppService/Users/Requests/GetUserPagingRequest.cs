namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 用户读取分页数据请求类
/// </summary>
public class GetUserPagingRequest : GetPagingRequestBase
{
    /// <summary>
    /// 组织架构Id
    /// </summary>
    public string? OrganizationId { get; set; }

    /// <summary>
    /// 角色Id
    /// </summary>
    public string? RoleId { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public IList<Status>? Status { get; set; }

    /// <summary>
    /// 排序值
    /// </summary>
    public override string? Sorter { get; set; } = "a.CreatedOn DESC";
}