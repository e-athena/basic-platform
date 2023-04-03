namespace BasicPlatform.AppService.Positions.Requests;

/// <summary>
/// 读取职位分页请求类
/// </summary>
public class GetPositionPagingRequest : GetPagingRequestBase
{
    /// <summary>
    /// 所属组织Id
    /// </summary>
    public string? OrganizationId { get; set; }
}