namespace BasicPlatform.SSO.Utils
{
    /// <summary>
    /// 当前用户信息
    /// </summary>
    public class CurrentUserModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public string Account { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public string Mobile { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public string Role { get; set; } = null!;
    }
}