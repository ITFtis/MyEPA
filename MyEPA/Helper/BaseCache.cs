using System;

namespace MyEPA.Helper
{
    public abstract class BaseCache
    {
        public abstract T Get<T>(string key);

        public abstract bool Set(string key, object data, TimeSpan? expiredTime = null);

        public abstract void Flush(string key);

        public abstract bool IsExist(string key);
    }
}