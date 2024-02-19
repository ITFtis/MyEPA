using MyEPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Extensions
{
    public static class ExtensionOfDictionary
    {
        /// <summary>
        /// 若找不到資料回傳預設(大部分是null)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetValue<T, S>(this Dictionary<S, T> dic, S key, T DefaultValue = default(T))
        {
            if (dic != null && key != null && dic.ContainsKey(key) && dic[key] != null)
            {
                return dic[key];
            }
            return DefaultValue;
        }

        public static MultiKeyDictionary<K1, K2, V> ToMultiKeyDictionary<T, K1, K2, V>(this IEnumerable<T> list, Func<T, K1> key1, Func<T, K2> key2, Func<T, V> elementSelector)
        {
            MultiKeyDictionary<K1, K2, V> result = new MultiKeyDictionary<K1, K2, V>();
            foreach (T item in list)
            {
                result.Add(key1(item), key2(item), elementSelector(item));
            }
            return result;
        }

        public static V GetValue<K1, K2, V>(this MultiKeyDictionary<K1, K2, V> dic, K1 Key1, K2 Key2, V DefaultValue = default(V))
        {
            if (dic != null && Key1 != null && Key2 != null && dic.ContainsKey(Key1, Key2))
            {
                return dic[Key1, Key2];
            }
            return DefaultValue;
        }
        public static Dictionary<K2, V> GetValue<K1, K2, V>(this MultiKeyDictionary<K1, K2, V> dic, K1 Key1, Dictionary<K2, V> DefaultValue = default(Dictionary<K2, V>))
        {
            if (dic != null && Key1 != null && dic.ContainsKey(Key1))
            {
                return dic[Key1];
            }
            return DefaultValue;
        }
    }
}