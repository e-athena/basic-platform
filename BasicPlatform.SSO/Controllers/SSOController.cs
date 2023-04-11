using System.Text;
using BasicPlatform.SSO.Models;
using BasicPlatform.SSO.Utils;

namespace BasicPlatform.SSO.Controllers
{
    /// <summary>
    /// 登录页面
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class SsoController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Cachelper _cachelper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtService"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="cachelper"></param>
        public SsoController(IJwtService jwtService, IHttpClientFactory httpClientFactory, Cachelper cachelper)
        {
            _jwtService = jwtService;
            _httpClientFactory = httpClientFactory;
            _cachelper = cachelper;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login([FromQuery] string clientId, string redirectUrl)
        {
            const string userName = "admin";
            const string password = "123456";
            var res = _jwtService.GetCode(clientId, userName, password);
            // 检查redirectUrl是否有?号，如果用则将?号后的参数拼接
            if (redirectUrl.Contains('?'))
            {
                var url = redirectUrl.Split("?")[0];
                var param = redirectUrl.Split("?")[1];
                return Redirect($"{url}?authCode={res.Data}&sessionCode={res.Data}&source=SSO&{param}");
            }

            return Redirect($"{redirectUrl}?authCode={res.Data}&sessionCode={res.Data}&source=SSO");
        }

        /// <summary>
        /// 获取授权码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseModel<string> GetCode([FromBody] GetCoderRequest request)
        {
            return _jwtService.GetCode(request.ClientId, request.UserName, request.Password);
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

        /// <summary>
        /// 根据会话code获取授权码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseModel<string> GetCodeBySessionCode([FromBody] GetCodeBySessionCodeRequest request)
        {
            return _jwtService.GetCodeBySessionCode(request.ClientId, request.SessionCode);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseModel> Logout([FromBody] LogoutRequest request)
        {
            //删除全局会话
            var sessionKey = $"SessionCode:{request.SessionCode}";
            _cachelper.DeleteKey(sessionKey);
            var client = _httpClientFactory.CreateClient();
            var param = new {sessionCode = request.SessionCode};
            var jsonData = System.Text.Json.JsonSerializer.Serialize(param);
            var paramContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            //这里实战中是用数据库或缓存取
            var urls = new List<string>()
            {
                "https://localhost:7001/Account/LogOut",
                "https://localhost:7002/Account/LogOut"
            };
            //这里可以异步mq处理，不阻塞返回
            foreach (var url in urls)
            {
                //web1退出登录
                var logOutResponse = await client.PostAsync(url, paramContent);
                var resultStr = await logOutResponse.Content.ReadAsStringAsync();
                var response = System.Text.Json.JsonSerializer.Deserialize<ResponseModel>(resultStr);
                Console.WriteLine(response is {Code: 0} // 成功
                    ? $"url:{url},会话Id:{request.SessionCode},退出登录成功"
                    : $"url:{url},会话Id:{request.SessionCode},退出登录失败");
            }

            return new ResponseModel().SetSuccess();
        }
    }
}