using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BasicPlatform.SSO.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BasicPlatform.SSO.Utils
{
    /// <summary>
    /// jwt服务
    /// </summary>
    public abstract class JwtBaseService : IJwtService
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly IOptions<AppSettingOptions> AppSettingOptions;

        /// <summary>
        /// 
        /// </summary>
        protected readonly Cachelper Cachelper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSettingOptions"></param>
        /// <param name="cachelper"></param>
        public JwtBaseService(IOptions<AppSettingOptions> appSettingOptions, Cachelper cachelper)
        {
            AppSettingOptions = appSettingOptions;
            Cachelper = cachelper;
        }

        /// <summary>
        /// 获取授权码
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ResponseModel<string> GetCode(string clientId, string userName, string password)
        {
            var result = new ResponseModel<string>();

            var appHsSetting = AppSettingOptions.Value.AppHsSettings
                .FirstOrDefault(s => s.ClientId == clientId);
            if (appHsSetting == null)
            {
                result.SetFail("应用不存在");
                return result;
            }

            //真正项目这里查询数据库比较
            if (!(userName == "admin" && password == "123456"))
            {
                result.SetFail("用户名或密码不正确");
                return result;
            }

            //用户信息
            var currentUserModel = new CurrentUserModel
            {
                Id = 101,
                Account = "admin",
                Name = "张三",
                Mobile = "13800138000",
                Role = "SuperAdmin"
            };

            // 生成授权码
            var code = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            var key = $"AuthCode:{code}";
            var appCacheKey = $"AuthCodeClientId:{code}";
            // 缓存授权码
            Cachelper.StringSet(key, currentUserModel, TimeSpan.FromMinutes(10));
            // 缓存授权码是哪个应用的
            Cachelper.StringSet(appCacheKey, appHsSetting.ClientId, TimeSpan.FromMinutes(10));
            // 创建全局会话
            var sessionCode = $"SessionCode:{code}";
            var sessionCodeUser = new SessionCodeUser
            {
                ExpiresTime = DateTime.Now.AddHours(1),
                CurrentUser = currentUserModel
            };
            Cachelper.StringSet(sessionCode, currentUserModel, TimeSpan.FromDays(1));
            //全局会话过期时间
            var sessionExpiryKey = $"SessionExpiryKey:{code}";
            var sessionExpiryTime = DateTime.Now.AddDays(1);
            Cachelper.StringSet(sessionExpiryKey, sessionExpiryTime, TimeSpan.FromDays(1));
            Console.WriteLine($"登录成功，全局会话code:{code}");
            //缓存授权码取token时最长的有效时间
            Cachelper.StringSet($"AuthCodeSessionTime:{code}", sessionExpiryTime, TimeSpan.FromDays(1));

            result.SetSuccess(code);
            return result;
        }

        /// <summary>
        /// 根据会话code获取授权码
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="sessionCode"></param>
        /// <returns></returns>
        public ResponseModel<string> GetCodeBySessionCode(string clientId, string sessionCode)
        {
            var result = new ResponseModel<string>();
            var appHsSetting = AppSettingOptions.Value.AppHsSettings.FirstOrDefault(s => s.ClientId == clientId);
            if (appHsSetting == null)
            {
                result.SetFail("应用不存在");
                return result;
            }

            var codeKey = $"SessionCode:{sessionCode}";
            var currentUserModel = Cachelper.StringGet<CurrentUserModel>(codeKey);
            if (currentUserModel == null)
            {
                return result.SetFail("会话不存在或已过期", string.Empty);
            }

            //生成授权码
            var code = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            var key = $"AuthCode:{code}";
            var appCacheKey = $"AuthCodeClientId:{code}";
            //缓存授权码
            Cachelper.StringSet(key, currentUserModel, TimeSpan.FromMinutes(10));
            //缓存授权码是哪个应用的
            Cachelper.StringSet(appCacheKey, appHsSetting.ClientId, TimeSpan.FromMinutes(10));

            //缓存授权码取token时最长的有效时间
            var expireTime = Cachelper.StringGet<DateTime>($"SessionExpiryKey:{sessionCode}");
            Cachelper.StringSet($"AuthCodeSessionTime:{code}", expireTime, expireTime - DateTime.Now);

            result.SetSuccess(code);
            return result;
        }

        /// <summary>
        /// 根据刷新Token获取Token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public string GetTokenByRefresh(string refreshToken, string clientId)
        {
            //刷新Token是否在缓存
            var currentUserModel = Cachelper.StringGet<CurrentUserModel>($"RefreshToken:{refreshToken}");
            if (currentUserModel == null)
            {
                return string.Empty;
            }

            //刷新token过期时间
            var refreshTokenExpiry = Cachelper.StringGet<DateTime>($"RefreshTokenExpiry:{refreshToken}");
            //token默认时间为600s
            double tokenExpiry = 600;
            //如果刷新token的过期时间不到600s了，token过期时间为刷新token的过期时间
            if (refreshTokenExpiry > DateTime.Now && refreshTokenExpiry < DateTime.Now.AddSeconds(600))
            {
                tokenExpiry = (refreshTokenExpiry - DateTime.Now).TotalSeconds;
            }

            //从新生成Token
            var token = IssueToken(currentUserModel, clientId, tokenExpiry);
            return token;
        }

        /// <summary>
        /// 根据授权码,获取Token
        /// </summary>
        /// <param name="authCode"></param>
        /// <returns></returns>
        public ResponseModel<GetTokenResponse> GetTokenWithRefresh(string authCode)
        {
            var result = new ResponseModel<GetTokenResponse>();

            var key = $"AuthCode:{authCode}";
            var clientIdCacheKey = $"AuthCodeClientId:{authCode}";
            var authCodeSessionTimeKey = $"AuthCodeSessionTime:{authCode}";

            //根据授权码获取用户信息
            var currentUserModel = Cachelper.StringGet<CurrentUserModel>(key);
            if (currentUserModel == null)
            {
                throw new Exception("授权码无效");
            }

            //清除authCode，只能用一次
            Cachelper.DeleteKey(key);

            //获取应用配置
            var clientId = Cachelper.StringGet<string>(clientIdCacheKey);
            //刷新token过期时间
            var sessionExpiryTime = Cachelper.StringGet<DateTime>(authCodeSessionTimeKey);
            var tokenExpiryTime = DateTime.Now.AddMinutes(10); //token过期时间10分钟
            //如果刷新token有过期期比token默认时间短，把token过期时间设成和刷新token一样
            if (sessionExpiryTime > DateTime.Now && sessionExpiryTime < tokenExpiryTime)
            {
                tokenExpiryTime = sessionExpiryTime;
            }

            //获取访问token
            if (clientId == null)
            {
                return result;
            }

            // 签发Token
            var token = IssueToken(currentUserModel, clientId, (sessionExpiryTime - DateTime.Now).TotalSeconds);

            TimeSpan refreshTokenExpiry;
            if (sessionExpiryTime != default)
            {
                refreshTokenExpiry = sessionExpiryTime - DateTime.Now;
            }
            else
            {
                refreshTokenExpiry = TimeSpan.FromSeconds(60 * 60 * 24); // 默认24小时
            }

            // 获取刷新token
            var refreshToken = IssueToken(currentUserModel, clientId, refreshTokenExpiry.TotalSeconds);
            // 缓存刷新token
            Cachelper.StringSet($"RefreshToken:{refreshToken}", currentUserModel, refreshTokenExpiry);
            // 缓存刷新token过期时间
            Cachelper.StringSet($"RefreshTokenExpiry:{refreshToken}",
                DateTime.Now.AddSeconds(refreshTokenExpiry.TotalSeconds), refreshTokenExpiry);
            result.SetSuccess(new GetTokenResponse() {Token = token, RefreshToken = refreshToken, Expires = 60 * 10});
            Console.WriteLine(
                $"client_id:{clientId}获取token,有效期:{sessionExpiryTime:yyyy-MM-dd HH:mm:ss},token:{token}");
            return result;
        }

        #region private

        /// <summary>
        /// 签发token
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="clientId"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        private string IssueToken(CurrentUserModel userModel, string clientId, double second = 600)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userModel.Name),
                new Claim("Account", userModel.Account),
                new Claim("Id", userModel.Id.ToString()),
                new Claim("Mobile", userModel.Mobile),
                new Claim(ClaimTypes.Role, userModel.Role),
            };
            //var appHSSetting = getAppInfoByAppKey(clientId);
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appHSSetting.clientSecret));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var creds = GetCreds(clientId);

            // Claims (Payload)
            // Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段:
            // iss: The issuer of the token，签发主体，谁给的
            // sub: The subject of the token，token 主题
            // aud: 接收对象，给谁的
            // exp: Expiration Time。 token 过期时间，Unix 时间戳格式
            // iat: Issued At。 token 创建时间， Unix 时间戳格式
            // jti: JWT ID。针对当前 token 的唯一标识
            //     除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
            var token = new JwtSecurityToken(
                issuer: "SSOCenter", //谁给的
                audience: clientId, //给谁的
                claims: claims,
                expires: DateTime.Now.AddSeconds(second), // token有效期
                notBefore: null, //立即生效  DateTime.Now.AddMilliseconds(30),//30s后有效
                signingCredentials: creds);
            var returnToken = new JwtSecurityTokenHandler().WriteToken(token);
            return returnToken;
        }

        /// <summary>
        /// 根据appKey获取应用信息
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        private AppHsSetting GetAppInfoByAppKey(string clientId)
        {
            var appHsSetting = AppSettingOptions.Value.AppHsSettings
                .First(s => s.ClientId == clientId);
            return appHsSetting;
        }

        /// <summary>
        /// 获取加密方式
        /// </summary>
        /// <returns></returns>
        protected abstract SigningCredentials GetCreds(string clientId);

        #endregion
    }
}