using MyEPA.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace MyEPA
{
    public static class ExtensionsOfEnum
    {
        public static List<SelectListItem> ConvertToSelectListItems<T>(this IEnumerable<T> types, T? selectType = null) 
            where T : struct, Enum
        {
            return types.Select(e =>
            {
                return new SelectListItem
                {
                    Text = e.GetDescription(),
                    Value = e.ToIntegerString(),
                    Selected = selectType.HasValue && e.Equals(selectType.Value)
                };
            }).ToList();
        }
        public static List<SelectListItem> ConvertToGroupSelectListItems<T>(this IEnumerable<T> types, T? selectType = null)
            where T : struct, Enum
        {
            Dictionary<string, SelectListGroup> groups = new Dictionary<string, SelectListGroup>();

            return types.Select(e =>
            {
                string name = e.GetContactManualGroup();

                SelectListGroup group = null;

                if (groups.ContainsKey(name))
                {
                    group = groups[name];
                }
                else
                {
                    group = new SelectListGroup()
                    {
                        Name = name
                    };
                    groups.Add(name, group);
                }

                return new SelectListItem
                {
                    Group = group,
                    Text = e.GetDescription(),
                    Value = e.ToIntegerString(),
                    Selected = e.Equals(selectType)
                };
            }).ToList();
        }
        public static int ToInteger(this Enum value)
        {
            return value.GetHashCode();
        }
        public static string ToIntegerString(this Enum value)
        {
            return value.GetHashCode().ToString();
        }
        /// <summary>
        /// 取得 Enum Attribute Description
        /// </summary>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumVal)
        {
            if (enumVal == null || enumVal.IsDefinedValue() == false)
            {
                return string.Empty;
            }
            var attr = enumVal.GetAttributeFromEnum<DescriptionAttribute>();
            if(attr == null)
            {
                return enumVal.ToString();
            }
            return attr.Description;
        }
        public static string GetContactManualGroup(this Enum enumVal)
        {
            if (enumVal == null || enumVal.IsDefinedValue() == false)
            {
                return string.Empty;
            }
            return enumVal.GetAttributeFromEnum<ContactManualGroupAttribute>().GroupName;
        }
        /// <summary>
        /// 是否為定義好的enum編號
        /// </summary>
        public static bool IsDefinedValue(this System.Enum enumVal)
        {
            var type = enumVal.GetType();
            return System.Enum.IsDefined(type, enumVal);
        }
        /// <summary>
        /// 取得Enums的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        public static T GetAttributeFromEnum<T>(this System.Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
        /// <summary>
        /// 取得每一筆enum的 英文:數值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetEnumAllValue<T>() where T : IComparable, IFormattable, IConvertible
        {
            List<T> result = new List<T>();

            foreach (T enumValue in System.Enum.GetValues(typeof(T)))
            {
                result.Add(enumValue);
            }

            return result;
        }

        public static T GetValueFromDescription<T>(this string description) where T : IComparable, IFormattable, IConvertible

        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
        }
    }
}