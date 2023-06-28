using BasicPlatform.AppService.Applications.Models;

namespace BasicPlatform.AppService.Applications.Requests;

/// <summary>
/// 更新网站系统应用请求类
/// </summary>
public class UpdateApplicationRequest : ApplicationModel, ITxRequest<string>
{
}