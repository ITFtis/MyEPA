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
        /// (閾值)消毒設備資料建置，屬性Copy
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


        /// <summary>
        /// (閾值)消毒藥劑資料建置，屬性Copy
        /// </summary>
        /// <param name="source">來源</param>
        /// <param name="dest">目的</param>
        public static void CopyPropertiesTo(this DisinfectantModel source, LogDisinfectantModel dest)
        {
            var sourceProps = typeof(DisinfectantModel).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(LogDisinfectantModel).GetProperties()
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