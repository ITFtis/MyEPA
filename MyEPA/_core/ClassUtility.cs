using MyEPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA
{
    /// <summary>
    /// 物件工具
    /// </summary>
    public static class ClassUtility
    {
        /// <summary>
        /// 閥值資料建置，屬性Copy
        /// </summary>
        /// <param name="source">來源</param>
        /// <param name="dest">目的</param>
        public static void CopyPropertiesTo(this DisinfectorModel source, LogDisinfectorModel dest)
        {
            var sourceProps = typeof(DisinfectorModel).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(LogDisinfectorModel).GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    p.SetValue(dest, sourceProp.GetValue(source, null), null);
                }

            }
        }
    }
}