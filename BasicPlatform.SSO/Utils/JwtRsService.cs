using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BasicPlatform.SSO.Utils
{
    /// <summary>
    /// JWT非对称加密
    /// </summary>
    public class JwtRsService : JwtBaseService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cachelper"></param>
        public JwtRsService(IOptions<AppSettingOptions> options, Cachelper cachelper) : base(options, cachelper)
        {
        }

        /// <summary>
        /// 生成非对称加密签名凭证
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        protected override SigningCredentials GetCreds(string clientId)
        {
            var appRsSetting = GetAppInfoByAppKey(clientId);
            var rsa = RSA.Create();
            var privateKey = Convert.FromBase64String(appRsSetting.PrivateKey); //这里只需要私钥，不要begin,不要end
            rsa.ImportPkcs8PrivateKey(privateKey, out _);
            var key = new RsaSecurityKey(rsa);
            var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
            return creds;
        }

        /// <summary>
        /// 根据appKey获取应用信息
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        private AppRsSetting GetAppInfoByAppKey(string clientId)
        {
            var appRsSetting = AppSettingOptions
                .Value
                .AppRsSettings
                .First(s => s.ClientId == clientId);
            return appRsSetting;
        }
    }
}