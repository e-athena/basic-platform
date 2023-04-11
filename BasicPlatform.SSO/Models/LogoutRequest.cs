namespace BasicPlatform.SSO.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LogoutRequest
    {
        /// <summary>
        /// 会话code
        /// </summary>
        public string SessionCode { get; set; } = null!;
    }
}