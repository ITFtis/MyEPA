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
    }
}