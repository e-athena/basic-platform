using BasicPlatform.SSO.Utils;

namespace BasicPlatform.SSO.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SessionCodeUser
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiresTime { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public CurrentUserModel CurrentUser { get; set; } = null!;
    }
}