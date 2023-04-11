using BasicPlatform.SSO.Models;
using BasicPlatform.SSO.Utils;

namespace BasicPlatform.SSO.Controllers
{
    /// <summary>
    /// 授权信息获取
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtService"></param>
        public AccountController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        /// <summary>
        /// 根据授权码,获取Token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseModel<GetTokenResponse> GetToken([FromBody] GetTokenRequest request)
        {
            return _jwtService.GetTokenWithRefresh(request.AuthCode);
        }
    }
}