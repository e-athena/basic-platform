namespace BasicPlatform.AppService.Positions.Requests;

/// <summary>
/// 职位状态变更请求类
/// </summary>
public class PositionStatusChangeRequest : IdRequest, ITxRequest<string>
{
}