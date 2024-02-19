using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MyEPA.Extensions
{
    public static class ExtensionOfStream
    {
        public static byte[] ToByte(this Stream stream)
        {
            byte[] bytes = new byte[stream.Length];

            stream.Read(bytes, 0, bytes.Length);

            // 設定當前流的位置為流的開始 

            stream.Seek(0, SeekOrigin.Begin);

            return bytes;
        }
    }
    public static class ExtensionOfObject
    {
        public static string ToJson<T>(this T model)
        {
            if (model == null)
                return null;

            return JsonConvert.SerializeObject(model);
        }
        public static string GetDisplayName(this PropertyInfo property)
        {
            var attribute = property.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault();
            if(attribute != null)
            {
                return attribute.DisplayName;
            }
            return string.Empty;
        }
        public static T JsonConvertToModel<T>(this string jsonText)
        {
            if (string.IsNullOrWhiteSpace(jsonText))
                return default(T);

            return JsonConvert.DeserializeObject<T>(jsonText);
        }
        public static List<T2> ConvertToModel<T1, T2>(this List<T1> list, Action<T1, T2> fun = null) where T2 : new()
        {
            return list.Select(e => e.ConvertToModel(fun)).ToList();
        }
        public static IEnumerable<T2> ConvertToModel<T1, T2>(this IEnumerable<T1> list, Action<T1, T2> fun = null) where T2 : new()
        {
            return list.Select(e => e.ConvertToModel(fun));
        }
        public static T2 ConvertToModel<T1, T2>(this T1 input, Action<T1,T2> fun = null) where T2 : new()
        {
            if(input == null)
            {
                return default(T2);
            }
            T2 result = new T2();
            foreach (var item in typeof(T2).GetProperties())
            {
                var p = typeof(T1).GetProperty(item.Name);
                if (p == null)
                {
                    continue;
                }
                var value = p.GetValue(input);
                item.SetValue(result, value);
            }
            fun?.Invoke(input, result);
            return result;
        }

        public static void CopyTo<S, T>(this S input,T result) where T : new()
        {
            foreach (var item in typeof(T).GetProperties())
            {
                var value = typeof(S).GetProperty(item.Name).GetValue(input);
                item.SetValue(result, value);
            }
        }
        public static T GetValueOrDefaultValue<S, T>(this S obj, Func<S, T> SelectFunc, T DefaultValue = default(T))
        {
            if (obj == null || SelectFunc(obj) == null)
            {
                return DefaultValue;
            }
            return SelectFunc(obj);
        }
        public static string ToGetJsonString(this object obj, ReferenceLoopHandling loopHanding)
        {
            if (obj == null)
                return null;

            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = loopHanding
            });
        }
        public static string ToGetJsonString(this object obj)
        {
            if (obj == null)
                return null;

            return JsonConvert.SerializeObject(obj);
        }
    }
}