namespace BasicPlatform.SSO.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GetCoderRequest
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        public string ClientId { get; set; } = null!;

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = null!;
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetCodeBySessionCodeRequest
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        public string ClientId { get; set; } = null!;

        /// <summary>
        /// 会话code
        /// </summary>
        public string SessionCode { get; set; }= null!;
    }
}