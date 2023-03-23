namespace BasicPlatform.AppService.Roles.Requests;

/// <summary>
/// 读取角色分页请求类
/// </summary>
public class GetRolePagingRequest : GetPagingRequestBase
{
    /// <summary>
    /// 数据访问范围
    /// </summary>
    public IList<RoleDataScope>? DataScope { get; set; }
}