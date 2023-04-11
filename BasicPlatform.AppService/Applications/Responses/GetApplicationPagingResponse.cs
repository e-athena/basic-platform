using BasicPlatform.AppService.Applications.Models;

namespace BasicPlatform.AppService.Applications.Responses;

/// <summary>
/// 网站应用分页返回值
/// </summary>
[DataPermission(typeof(Application), "网站应用管理模块")]
public class GetApplicationPagingResponse : ApplicationQueryModel
{
}