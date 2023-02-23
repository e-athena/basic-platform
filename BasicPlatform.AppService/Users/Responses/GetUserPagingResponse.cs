using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Responses;

/// <summary>
/// 读取用户分页数据响应类
/// </summary>
public class GetUserPagingResponse : UserModel
{
  /// <summary>
  /// 创建人ID
  /// </summary>
  public string? CreatedUserId { get; set; }

  /// <summary>
  /// 创建人
  /// </summary>
  public string? CreatedUserName { get; set; }
}