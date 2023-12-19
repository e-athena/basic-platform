using BasicPlatform.AppService.WebSettings.Models;

namespace BasicPlatform.AppService.WebSettings.Requests;

/// <summary>
/// 保存网站设置请求类
/// </summary>
public class SaveWebSettingRequest : WebSettingModel, ITxRequest<string>
{
}