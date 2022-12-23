namespace BasicPlatform.AppService.Users.Requests;

/// <summary>
/// 用户状态变更请求类
/// </summary>
public class UserStatusChangeRequest : IdRequest, IRequest<string>, ITransactionRequest
{
}