namespace BasicPlatform.SSO.Models
{
    /// <summary>
    /// 获取Token请求类
    /// </summary>
    public class GetTokenRequest
    {
        /// <summary>
        /// 授权code
        /// </summary>
        public string AuthCode { get; set; } = null!;
    }
}