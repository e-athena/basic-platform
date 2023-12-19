using BasicPlatform.AppService.Applications.Models;
using BasicPlatform.Domain.Models.Applications;

namespace BasicPlatform.AppService.Applications.Responses;

/// <summary>
/// 子应用分页返回值
/// </summary>
[DataPermission(typeof(Application), "子应用管理模块", AppId = "system")]
public class GetApplicationPagingResponse : ApplicationQueryModel
{
}