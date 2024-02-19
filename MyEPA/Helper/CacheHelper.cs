using MyEPA.Extensions;
using System;

namespace MyEPA.Helper
{
    public class CacheHelper
    {
        protected static TimeSpan _defaultCacheTime
        {
            get
            {
                return new TimeSpan(0, 2, 0);
            }
        }


        public static T GetCacheOrQuery<T>(string key, Func<T> func, int expiredMinute)
        {
            return GetCacheOrQuery(key, func, new TimeSpan(0, expiredMinute, 0));
        }
        public static T GetCacheOrQueryBySeconds<T>(string key, Func<T> func, int expiredseconds)
        {
            return GetCacheOrQuery(key, func, new TimeSpan(0, 0, expiredseconds));
        }

        public static T GetCacheOrQuery<T>(string key, Func<T> func, TimeSpan? expiredTime = null)
        {
            var cacheService = InstanceCacheService();

            if (cacheService.IsExist(key))
            {
                T cacheValue = GetCacheValue<T>(key);

                return cacheValue;
            }

            T queryResult = func();

            //null的話 就不用cache存了
            if (queryResult == null)
                return queryResult;

            var cloneResult = queryResult.ToGetJsonString().JsonConvertToModel<T>();//todo question why cloneResult
            SetCacheValue(key, cloneResult, expiredTime);

            return queryResult;
        }
        public static long Increment(string key, int expiredTimeSeconds = 900)
        {
            var cacheService = InstanceCacheService();
            long cacheValue = 0;
            if (cacheService.IsExist(key) == false)
            {
                SetCacheValue(key, ++cacheValue, expiredTimeSeconds);

                return cacheValue;
            }
            cacheValue = cacheService.Get<long>(key);
            SetCacheValue(key, ++cacheValue, expiredTimeSeconds);
            return cacheValue;
        }
        public static bool IsExist(string key)
        {
            var cacheService = InstanceCacheService();
            return cacheService.IsExist(key);
        }
        public static T GetCacheValue<T>(string key)
        {
            var cacheService = InstanceCacheService();

            T cacheValue = cacheService.Get<T>(key);
            var cloneResult = cacheValue.ToGetJsonString().JsonConvertToModel<T>();
            return cloneResult;
        }

        public static T GetCacheValueAndRemove<T>(string key)
        {
            var cacheValue = GetCacheValue<T>(key);

            RemoveCacheByKey(key);

            return cacheValue;
        }

        public static bool SetCacheValue<T>(string key, T value,int expiredTimeSeconds)
        {
            return SetCacheValue(key, value, new TimeSpan(0, 0, expiredTimeSeconds));
        }
        public static bool SetCacheValue<T>(string key, T value, TimeSpan? expiredTime)
        {
            expiredTime = GetValidExpireTime(expiredTime);

            var cacheService = InstanceCacheService();
            //記憶體共用問題
            var cloneResult = value.ToGetJsonString().JsonConvertToModel<T>();

            bool isOK = cacheService.Set(key, cloneResult, expiredTime);
            return isOK;
        }

        public static void RemoveCacheByKey(string key)
        {
            var cacheService = InstanceCacheService();

            cacheService.Flush(key);
        }

        private static BaseCache InstanceCacheService()
        {

            //如果之後要更換成 redisCache 應該改一下這邊就好
            return InstanceCacheByConfigSetting();
        }


        private static BaseCache InstanceCacheByConfigSetting()
        {
            return InstanceCacheByType();
        }


        public static BaseCache InstanceCacheByType()
        {
            return new RuntimeCache();
        }


        public static TimeSpan GetValidExpireTime(TimeSpan? expiredTime)
        {
            if (expiredTime.HasValue == false)
                expiredTime = _defaultCacheTime;

            return expiredTime.Value;
        }
    }
}