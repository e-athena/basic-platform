using BasicPlatform.AppService.ExternalPages.Models;

namespace BasicPlatform.AppService.ExternalPages.Requests;

/// <summary>
/// 创建请求类
/// </summary>
public class CreateExternalPageRequest : ExternalPageModel, ITxRequest<string>
{
    /// <summary>
    /// 是否公开页面
    /// </summary>
    public bool IsPublic { get; set; }
}