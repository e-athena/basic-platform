using Microsoft.Extensions.Caching.Memory;

namespace BasicPlatform.SSO.Utils
{
    /// <summary>
    /// 缓存帮助类-真正项目这里用redis
    /// </summary>
    public class Cachelper
    {
        //内存缓存
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryCache"></param>
        public Cachelper(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool StringSet<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (expiry == null)
            {
                _memoryCache.Set<T>(key, value);
            }
            else
            {
                _memoryCache.Set<T>(key, value, (TimeSpan) expiry);
            }

            return true;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T? StringGet<T>(string key)
        {
            var result = _memoryCache.Get<T>(key);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void DeleteKey(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}