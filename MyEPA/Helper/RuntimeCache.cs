using System;
using System.Web;
using System.Web.Caching;

namespace MyEPA.Helper
{
    internal class RuntimeCache : BaseCache
    {

        /// <summary>
        /// 取得Cache中的物件
        /// </summary>
        public override T Get<T>(string key)
        {
            try
            {

                return (T)HttpRuntime.Cache[key];
            }
            catch
            {
                return default(T);
            }
        }
        public override bool IsExist(string key)
        {
            return HttpRuntime.Cache[key] != null;
        }

        public override bool Set(string key, object value, TimeSpan? expiredTime = null)
        {
            HttpRuntime.Cache.Remove(key);
            HttpRuntime.Cache.Add(
                key,
                value,
                null,
                DateTime.UtcNow.Add(expiredTime.Value),
                Cache.NoSlidingExpiration,
                CacheItemPriority.Default,
                null);

            return true;
        }


        /// <summary>
        /// 清除鍵值為key的特定Cache
        /// </summary>
        public override void Flush(string key)
        {
            try
            {
                HttpRuntime.Cache.Remove(key);
            }
            catch
            {

            }
        }
    }
}