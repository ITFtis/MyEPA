using MyEPA.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace MyEPA.Extensions
{
    public static class ExtensionOfDocX
    {
        public static Table InsertTable<T>(this DocX doc, IEnumerable<T> list, Action<Table> action = null) where T : class
        {
            Table table = list.ConvertToTable();
            if(action != null)
            {
                action(table);
            }
            doc.InsertTable(table);

            return table;
        }
        private static WordTableHelper CreateWordTableHelper(this DocX doc, int row, int cell)
        {
            return new WordTableHelper(doc, row, cell);
        }

        private static Table ConvertToTable<T>(this IEnumerable<T> datas) where T : class
        {
            //僅暫存不儲存檔案
            DocX doc = DocX.Create(string.Empty, DocumentTypes.Template);
            
            PropertyInfo[] prop = datas.GetType().GetProperty("Item").PropertyType.GetProperties();

            WordTableHelper helper = doc.CreateWordTableHelper(datas.Count(), prop.Length);

            //寫入標題
            foreach (var item in prop)
            {
                var attr = item.GetCustomAttribute<DisplayNameAttribute>();
                string displayName = item.Name;
                if (attr != null)
                {
                    displayName = attr.DisplayName;
                }
                helper.SetCellValue(displayName);
            }
            helper.NextRow();
            foreach (var item in datas)
            {
                foreach (var p in prop)
                {
                    string value = p.GetValue(item)?.ToString();
                    helper.SetCellValue(value);
                }
                helper.NextRow();
            }
            return helper.GetTable();
        }

    }
}