namespace BasicPlatform.SSO.Models
{
    /// <summary>
    /// 响应结果
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// 响应状态码
        /// </summary>
        public ResponseCode Code { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string? Message { get; set; }
    }

    /// <summary>
    /// 有对象的响应结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseModel<T> : ResponseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public T? Data { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ResponseCode
    {
        /// <summary>
        /// 
        /// </summary>
        Success = 0,

        /// <summary>
        /// 
        /// </summary>
        Fail = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ResponseExtend
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResponseModel SetSuccess(this ResponseModel response, string message = "")
        {
            var result = new ResponseModel
            {
                Code = ResponseCode.Success,
                Message = message == string.Empty ? "操作成功" : message
            };
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ResponseModel<T> SetSuccess<T>(this ResponseModel<T> response, T? data = default,
            string message = "")
        {
            response.Code = ResponseCode.Success;
            response.Message = message == string.Empty ? "操作成功" : message;
            response.Data = data;
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResponseModel SetFail(this ResponseModel response, string message = "")
        {
            response.Code = ResponseCode.Fail;
            response.Message = message == string.Empty ? "操作失败" : message;
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ResponseModel<T> SetFail<T>(this ResponseModel<T> response, string message = "",
            T? data = default)
        {
            response.Code = ResponseCode.Fail;
            response.Message = message == string.Empty ? "操作成功" : message;
            response.Data = data;
            return response;
        }
    }
}