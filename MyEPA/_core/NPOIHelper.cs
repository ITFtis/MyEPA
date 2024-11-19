using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.OpenXml4Net.OPC.Internal;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace MyEPA
{
    public class NPOIHelper
    {
        /// <summary>
        /// 取得cell欄位資料
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static object getCellValue(ICell cell)
        {
            object cValue = string.Empty;
            switch (cell.CellType)
            {
                case (CellType.Unknown | CellType.Formula | CellType.Blank):
                    cValue = cell.ToString();
                    break;
                case CellType.Numeric:
                    cValue = cell.NumericCellValue;
                    break;
                case CellType.String:
                    cValue = cell.StringCellValue;
                    break;
                case CellType.Boolean:
                    cValue = cell.BooleanCellValue;
                    break;
                case CellType.Error:
                    cValue = cell.ErrorCellValue;
                    break;
                default:
                    cValue = string.Empty;
                    break;
            }
            return cValue;
        }

        /// <summary>
        /// 片段文字顏色
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="cell"></param>
        /// <param name="lists">片段文字</param>
        public static void ReplaceCellStyleF1(XSSFWorkbook workbook, ICell cell,
                                              List<string> lists, XSSFFont font)
        {
            string text = cell.ToString();

            foreach (string str in lists)
            {
                int start = text.IndexOf(str);
                if (start > 0)
                {
                    cell.RichStringCellValue.ApplyFont(start, start + str.Length, font);
                }
            }
        }
    }
}