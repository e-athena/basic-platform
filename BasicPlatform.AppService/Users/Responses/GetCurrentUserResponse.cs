using BasicPlatform.AppService.Users.Models;

namespace BasicPlatform.AppService.Users.Responses;

/// <summary>
/// 读取当前用户响应类
/// </summary>
public class GetCurrentUserResponse : UserModel
{
    /// <summary>
    /// 资源编码列表
    /// </summary>
    public List<string> ResourceCodes { get; set; } = new();
}