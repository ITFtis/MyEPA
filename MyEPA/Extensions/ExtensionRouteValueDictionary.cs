using System;
using System.Collections;
using System.Reflection;
using System.Web.Routing;

namespace MyEPA.Extensions
{
    public static class ExtensionRouteValueDictionary
    {
        public static RouteValueDictionary ToRouteValueDictionary<T>(this T input)
        {
            RouteValueDictionary routeValue = new RouteValueDictionary();

            foreach (PropertyInfo property in input.GetType().GetProperties())
            {
                var value = property.GetValue(input);
                if(value == null)
                {
                    continue;
                }
                //數值
                if (property.PropertyType.IsValueType || value is string)
                {
                    routeValue.Add(property.Name, value);
                }
                else if (value is IList && value.GetType().IsGenericType)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    var routeValues = ToRouteValueDictionary(value);
                    routeValue.Add(routeValues);
                }
            }

            return routeValue;
        }
        public static void Add(this RouteValueDictionary routeValue, RouteValueDictionary input)
        {
            foreach (var item in input)
            {
                if (routeValue.ContainsKey(item.Key))
                {
                    routeValue[item.Key] = item.Value;
                }
                else
                {
                    routeValue.Add(item.Key, item.Value);
                }

            }
        }
        public static RouteValueDictionary AddModel<T>(this RouteValueDictionary routeValue, T data)
        {
            foreach (PropertyInfo property in data.GetType().GetProperties())
            {
                routeValue.Add(property.Name, property.GetValue(data));
            }

            return routeValue;
        }
    }
}