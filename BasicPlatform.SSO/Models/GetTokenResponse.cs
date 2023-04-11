namespace BasicPlatform.SSO.Models
{
    /// <summary>
    /// 获取token响应结果
    /// </summary>
    public class GetTokenResponse
    {
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; } = null!;

        /// <summary>
        /// 刷新token
        /// </summary>
        public string RefreshToken { get; set; }= null!;

        /// <summary>
        /// 过期时间,多少s后
        /// </summary>
        public int Expires { get; set; }

        /// <summary>
        /// 资源域
        /// </summary>
        public string? Scope { get; set; }
    }
}