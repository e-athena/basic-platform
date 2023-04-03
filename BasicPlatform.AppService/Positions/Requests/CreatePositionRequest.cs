using BasicPlatform.AppService.Positions.Models;

namespace BasicPlatform.AppService.Positions.Requests;

/// <summary>
/// 创建职位请求类
/// </summary>
public class CreatePositionRequest : PositionModel, ITxRequest<string>
{
}