using BasicPlatform.SSO.Models;

namespace BasicPlatform.SSO.Utils
{
    /// <summary>
    /// JWT服务接口
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// 获取授权码
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        ResponseModel<string> GetCode(string clientId, string userName, string password);

        /// <summary>
        /// 根据会话Code获取授权码
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="sessionCode"></param>
        /// <returns></returns>
        ResponseModel<string> GetCodeBySessionCode(string clientId, string sessionCode);

        /// <summary>
        /// 获取Token+RefreshToken
        /// </summary>
        /// <param name="authCode"></param>
        /// <returns>Token+RefreshToken</returns>
        ResponseModel<GetTokenResponse> GetTokenWithRefresh(string authCode);

        /// <summary>
        /// 基于refreshToken获取Token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        string GetTokenByRefresh(string refreshToken, string clientId);
    }
}