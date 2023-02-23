using BasicPlatform.AppService.Positions.Models;

namespace BasicPlatform.AppService.Positions.Responses;

/// <summary>
/// 读取职位分页数据响应类
/// </summary>
public class GetPositionPagingResponse : PositionModel
{
  /// <summary>
  /// 创建人
  /// </summary>
  public string? CreatedUserName { get; set; }
}