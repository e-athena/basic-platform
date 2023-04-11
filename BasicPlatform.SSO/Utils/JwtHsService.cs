using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BasicPlatform.SSO.Utils
{
    /// <summary>
    /// JWT对称可逆加密
    /// </summary>
    public class JwtHsService : JwtBaseService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cachelper"></param>
        public JwtHsService(IOptions<AppSettingOptions> options, Cachelper cachelper) : base(options, cachelper)
        {
        }

        /// <summary>
        /// 生成对称加密签名凭证
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        protected override SigningCredentials GetCreds(string clientId)
        {
            var appHsSettings = GetAppInfoByAppKey(clientId);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appHsSettings.ClientSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            return creds;
        }

        /// <summary>
        /// 根据appKey获取应用信息
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        private AppHsSetting GetAppInfoByAppKey(string clientId)
        {
            var appHsSetting = AppSettingOptions
                .Value
                .AppHsSettings
                .First(s => s.ClientId == clientId);
            return appHsSetting;
        }
    }
}