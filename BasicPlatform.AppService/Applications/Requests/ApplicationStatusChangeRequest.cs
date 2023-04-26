namespace BasicPlatform.AppService.Applications.Requests;

/// <summary>
/// 应用状态变更请求类
/// </summary>
public class ApplicationStatusChangeRequest : IdRequest, ITxRequest<string>
{
}