using MyEPA.EPA.Attribute;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MyEPA.Extensions
{
    public static class ExtensionOfCollections
    {
        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        private static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
        public static bool IsEmptyOrNull<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return true; // or throw an exception

            if (source.Any())
                return false;

            return true;
        }
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return new HashSet<T>(); // or throw an exception

            return new HashSet<T>(source);
        }
        
        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        private static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }
        public static bool IsList(Type t)
        {
            return t.IsGenericType &&
                   t.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
        public static List<DataTableViewModel> ToDataTables<T>(this T item) where T : class
        {
            List<DataTableViewModel> dataTables = new List<DataTableViewModel>();
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                if(t == typeof(string))
                {
                    continue;
                }

                if (IsList(t))
                {
                    var value = prop.GetValue(item) as IEnumerable<object>;
                    var tb = ToDataTable(value);

                    string title = prop.Name;

                    DisplayNameAttribute attrs = 
                        prop.GetCustomAttributes(true).ToList()
                        .Where(e=>e is DisplayNameAttribute)
                        .FirstOrDefault() as DisplayNameAttribute;

                    if (attrs != null)
                    {
                        title = attrs.DisplayName;
                    }

                    dataTables.Add(new DataTableViewModel 
                    {
                        DataTable = tb,
                        Title = title
                    });
                }
            }
            return dataTables;
        }
        public static DataTable ToDataTable<T>(this IEnumerable<T> items, List<string> ignoreFields = null)
        {
            var type = items.GetType().GetGenericArguments().Single();
            var tb = new DataTable(type.Name);

            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            List<object> sumValues = new List<object>() {};

            bool isSumAttrExist = false;

            foreach (PropertyInfo prop in props)
            {
                
                if (ignoreFields.IsNotEmpty() && ignoreFields.Contains(prop.Name))
                {
                    continue;
                }
                string columnName = prop.Name;

                var attrs = prop.GetCustomAttributes(true).ToList();

                DisplayNameAttribute attr =
                         attrs.Where(e => e is DisplayNameAttribute)
                         .FirstOrDefault() as DisplayNameAttribute;

                if (attr != null)
                {
                    columnName = attr.DisplayName;
                }
                Type t = GetCoreType(prop.PropertyType);
                if (t.IsEnum || Type.Equals(t, typeof(DateTime)) || Type.Equals(t, typeof(bool)))
                {
                    tb.Columns.Add(columnName, typeof(string));
                }
                else
                {
                    tb.Columns.Add(columnName, t);
                }

                DataTableSumAttribute sumattr =
                         attrs.Where(e => e is DataTableSumAttribute)
                         .FirstOrDefault() as DataTableSumAttribute;

                if (sumattr != null)
                {
                    decimal sum = 0;
                    foreach (var item in items)
                    {
                        sum += Convert.ToDecimal(prop.GetValue(item, null));
                    }
                    if (Type.Equals(t, typeof(int)))
                    {
                        sumValues.Add(Convert.ToInt32(sum));
                    }
                    else if (Type.Equals(t, typeof(decimal)))
                    {
                        sumValues.Add(sum);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    isSumAttrExist = true;
                }
                else
                {
                    //強制讓第一欄的合計資料輸出合計
                    if(sumValues.Any() == false)
                    {
                        sumValues.Add("合計");
                    }
                    else
                    {
                        sumValues.Add(null);
                    }
                }
            }

            if(isSumAttrExist)
            {
                tb.Rows.Add(sumValues.ToArray());
            }

            foreach (T item in items)
            {
                List<object> values = new List<object>();

                for (int i = 0; i < props.Length; i++)
                {
                    PropertyInfo p = props[i];
                    if (ignoreFields.IsNotEmpty() && ignoreFields.Contains(p.Name))
                    {
                        continue;
                    }
                    Type t = GetCoreType(p.PropertyType);
                    if (t.IsEnum)
                    {
                        var value = (Enum)props[i].GetValue(item, null);
                        values.Add(value.GetDescription());
                    }
                    else if(Type.Equals(t, typeof(DateTime)))
                    {
                        var value = (DateTime?)props[i].GetValue(item, null);
                        values.Add(value.ToDateTimeString());
                    }
                    else if(Type.Equals(t, typeof(bool)))
                    {
                        var value = (bool?)props[i].GetValue(item, null);
                        values.Add(value.HasValue ? value.Value ? "是" : "否" : string.Empty);
                    }
                    else if (Type.Equals(t, typeof(string)))
                    {
                        var value = (string)props[i].GetValue(item, null);
                        if(value == null)
                        {
                            value = string.Empty;
                        }
                        values.Add(value.Replace("<br />", "\n"));
                    }
                    else
                    {
                        values.Add(props[i].GetValue(item, null));
                    }
                }

                tb.Rows.Add(values.ToArray());
            }

            return tb;
        }
        /// <summary>
        /// 判斷List是否不為空 (不為null ＆＆　筆數大於０)
        /// </summary>
        public static bool IsNotEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return false; // or throw an exception

            return source.Any();
        }
        /// <summary>
        /// 將單筆 int、long、string 等型別轉成 list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static List<T> ToListCollection<T>(this T inputValue)
        {
            List<T> listResult = new List<T>();
            listResult.Add(inputValue);

            return listResult;
        }

        public static IEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> sources, string propertyName)
        {
            return OrderBy(sources, propertyName, false);
        }

        public static IEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> sources, string propertyName)
        {
            return OrderBy(sources, propertyName, true);
        }

        private static IEnumerable<TSource> OrderBy<TSource>(IEnumerable<TSource> sources, string propertyName, bool isDescending)
        {
            PropertyInfo propertyInfo = typeof(TSource).GetProperty(propertyName);
            if (propertyInfo == null)
            {
                return sources;
            }
            if (isDescending)
            {
                return sources.OrderByDescending(x => propertyInfo.GetValue(x, null));
            }
            else
            {
                return sources.OrderBy(x => propertyInfo.GetValue(x, null));
            }
        }
    }
}