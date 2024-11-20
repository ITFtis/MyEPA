
using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Models.QueryModel;
using MyEPA.Repositories;
using MyEPA.Services;
using MyEPA.ViewModels;
using NPOI.HSSF.UserModel;
using NPOI.OpenXml4Net.OPC.Internal;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Xceed.Pdf;
using Xceed.Words.NET;

namespace MyEPA
{
    public class EasyReport
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 簡報產製1_緊急應變統計表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string Export1_Urgent()
        {
            string result = "";

            try
            {
                //1.車輛
                var tmp1Car = GetCarsTotal();
                var carTypes = GetCarsType();

                //2.1 消毒設備            
                var disinfectorDatas = GetDisinfector();
                var tmp2Disinfector = disinfectorDatas;

                //匯出Excel
                //我要下載的檔案位置
                string filefolder = HttpContext.Current.Server.MapPath("~/FileDatas/Template/");
                string fileName = "(範本)簡報產製1_緊急應變統計表.xlsx";
                string path = filefolder + fileName;

                //取得檔案名稱
                string filename = System.IO.Path.GetFileName(path);

                StringBuilder sb = new StringBuilder();
                string toFolder = HttpContext.Current.Server.MapPath("~/FileDatas/Temp/");
                string toFileName = fileName + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                string toPath = toFolder + toFileName.Replace("(範本)", "");

                using (FileStream stream = System.IO.File.OpenRead(path))
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0);
                    workbook.SetSheetName(workbook.GetSheetIndex(sheet), "緊急應變統計表");

                    //dic1 對應
                    Dictionary<string, string> dic1 = new Dictionary<string, string>()
                {
                        {"A01",  tmp1Car.Where(a => a.Type == "A01").FirstOrDefault().Count.ToString()},
                        {"A02",  tmp1Car.Where(a => a.Type == "A02").FirstOrDefault().Count.ToString()},
                        {"A03",  tmp1Car.Where(a => a.Type == "A03").FirstOrDefault().Count.ToString()},
                        {"B01",  tmp1Car.Where(a => a.Type == "B01").FirstOrDefault().Count.ToString()},
                        {"B02",  tmp1Car.Where(a => a.Type == "B02").FirstOrDefault().Count.ToString()},
                        {"B03",  tmp1Car.Where(a => a.Type == "B03").FirstOrDefault().Count.ToString()},
                        {"B04",  tmp1Car.Where(a => a.Type == "B04").FirstOrDefault().Count.ToString()},
                        {"B05",  tmp1Car.Where(a => a.Type == "B05").FirstOrDefault().Count.ToString()},
                        {"B06",  tmp1Car.Where(a => a.Type == "B06").FirstOrDefault().Count.ToString()},
                        {"B07",  tmp1Car.Where(a => a.Type == "B07").FirstOrDefault().Count.ToString()},
                        {"B08",  tmp1Car.Where(a => a.Type == "B08").FirstOrDefault().Count.ToString()},
                        {"B09",  tmp1Car.Where(a => a.Type == "B09").FirstOrDefault().Count.ToString()},
                        {"B10",  tmp1Car.Where(a => a.Type == "B10").FirstOrDefault().Count.ToString()},
                        {"TotalType",  carTypes.Count.ToString()},
                        {"TotalCount",  tmp1Car.Sum(a => a.Count).ToString()},
                };

                    //dic2 對應
                    Dictionary<string, string> dic2 = new Dictionary<string, string>();

                    //消毒設備
                    int d21_count = tmp2Disinfector.Sum(a => a.SprayerCount + a.DisinfectorCount + a.HotSmokeSachineCount
                                                    + a.PressureWasherCount + a.SprayerCAR + a.SprayeSrHI
                                                    + a.SprayeSrLO + a.SMOK + a.OtherCount);
                    dic2.Add("TotalDisinfectorCount", d21_count.ToString());

                    //2.2 消毒藥劑 => 環境消毒
                    var d22_disinfectants = GetDisinfectantEnumEnvironment();
                    dic2.Add("EnvironmentSolidAmount", d22_disinfectants.Where(a => a.DrugState == "固體").Sum(a => a.Amount).ToString());
                    dic2.Add("EnvironmentLiquidAmount", d22_disinfectants.Where(a => a.DrugState != "固體").Sum(a => a.Amount).ToString());

                    //2.3 => 登革熱
                    var d23_disinfectants = GetDisinfectantEnumDengue();
                    dic2.Add("DengueSolidAmount", d23_disinfectants.Where(a => a.DrugState == "固體").Sum(a => a.Amount).ToString());
                    dic2.Add("DengueLiquidAmount", d23_disinfectants.Where(a => a.DrugState != "固體").Sum(a => a.Amount).ToString());
                    //dic2.Add("DengueEmulsionAmount", d23_disinfectants.Where(a => a.DrugState == "乳劑").Sum(a => a.Amount).ToString());

                    //遍歷每一列中的每一個Cell
                    for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        IRow row = sheet.GetRow(rowIndex);

                        if (row != null)
                        {
                            ICellStyle cellstyle = workbook.CreateCellStyle();

                            for (int cellIndex = 0; cellIndex < row.LastCellNum; cellIndex++)
                            {
                                var cell = row.GetCell(cellIndex);

                                if (cell != null)
                                {
                                    // Get the cell value as a string
                                    string cellValue = cell.ToString();

                                    //第1區塊
                                    foreach (var texts in dic1)
                                    {
                                        try
                                        {
                                            var repStr = "[1_$" + texts.Key + "$]";
                                            if (cellValue.Contains(repStr))
                                            {
                                                var text = cellValue.Replace(repStr, texts.Value);  // 替换段落中的文字

                                                //Cell 多個Replace
                                                if (texts.Key == "TotalType" || texts.Key == "TotalCount")
                                                {
                                                    var str = cellValue.Replace("[1_$TotalType$]", dic1["TotalType"])
                                                                        .Replace("[1_$TotalCount$]", dic1["TotalCount"]);
                                                    cell.SetCellValue(str);
                                                    continue;
                                                }

                                                //Cell 單一Replace
                                                int tryInt;//測試數字
                                                if (int.TryParse(text, out tryInt))
                                                {
                                                    cell.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
                                                    cell.CellStyle.Alignment = HorizontalAlignment.Right;
                                                    cell.SetCellValue(tryInt);
                                                }
                                                else
                                                {
                                                    cell.SetCellValue(text);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // 不处理
                                            continue;
                                        }
                                    }

                                    //第2區塊
                                    foreach (var texts in dic2)
                                    {
                                        try
                                        {
                                            var repStr = "[2_$" + texts.Key + "$]";
                                            if (cellValue.Contains(repStr))
                                            {
                                                var text = cellValue.Replace(repStr, texts.Value);  // 替换段落中的文字

                                                //Cell 單一Replace
                                                int tryInt;//測試數字
                                                if (int.TryParse(text, out tryInt))
                                                {
                                                    cell.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
                                                    cell.CellStyle.Alignment = HorizontalAlignment.Right;
                                                    cell.SetCellValue(tryInt);
                                                }
                                                else
                                                {
                                                    cell.SetCellValue(text);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // 不处理
                                            continue;
                                        }
                                    }
                                }
                            }
                            Console.WriteLine(); // Move to the next row
                        }
                    }

                    if (!Directory.Exists(toFolder))
                    {
                        Directory.CreateDirectory(toFolder);
                    }

                    FileStream xlsFile = new FileStream(toPath, FileMode.Create, FileAccess.Write);
                    workbook.Write(xlsFile);
                    xlsFile.Close();
                    workbook.Close();

                    result = toPath;
                }
            }
            catch (Exception ex)
            {
                logger.Error("執行錯誤：簡報產製1_緊急應變統計表");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        /// <summary>
        /// 簡報產製2_台灣地圖
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string Export2_TaiwanMap()
        {
            string result = "";

            try
            {
                //匯出Excel
                //我要下載的檔案位置
                string filefolder = HttpContext.Current.Server.MapPath("~/FileDatas/Template/");
                string fileName = "(範本)簡報產製2_台灣地圖.xlsx";
                string path = filefolder + fileName;

                //取得檔案名稱
                string filename = System.IO.Path.GetFileName(path);

                StringBuilder sb = new StringBuilder();
                string toFolder = HttpContext.Current.Server.MapPath("~/FileDatas/Temp/");
                string toFileName = fileName + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                string toPath = toFolder + toFileName.Replace("(範本)", "");

                using (FileStream stream = System.IO.File.OpenRead(path))
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0);
                    workbook.SetSheetName(workbook.GetSheetIndex(sheet), "台灣地圖");


                    //oooooooooooooooooooo
                    //內容處理
                    //--總計[$ValEnvS$][$ValEnvL$][$ValDengueS$][$VaDenguelL$]
                    //--1."北基宜地區：[$OrA1$] [$AntA1S$] [$OrA1L$]
                    //--2."桃竹苗地區：[$OrA2$] [$AntA2S$] [$OrA2L$]
                    //--3."中彰投地區：[$OrA3$] [$AntA3S$] [$OrA3L$]
                    //--4."雲嘉南地區：[$OrA4$] [$AntA4S$] [$OrA4L$]
                    //--5."高屏地區：[$OrA5$] [$AntA5S$] [$OrA5L$]
                    //--6."花東地區：[$OrA6$] [$AntA6S$] [$OrA6L$]
                    //--7."澎金馬地區：[$OrA7$] [$AntA7S$] [$OrA7L$]

                    var typeCitys = Code.GetTWTypeCity();

                    //消毒設備
                    var disinfectorDatas = GetDisinfector();

                    //2.2 消毒藥劑 => 環境消毒
                    var d22_disinfectants = GetDisinfectantEnumEnvironment();

                    //2.3 => 登革熱
                    var d23_disinfectants = GetDisinfectantEnumDengue();

                    //一、環境消毒藥劑跟登革熱藥劑分開
                    //遍歷每一列中的每一個Cell
                    for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        IRow row = sheet.GetRow(rowIndex);
                        if (row == null)
                            continue;

                        for (int cellIndex = 0; cellIndex < row.LastCellNum; cellIndex++)
                        {

                            var cell = row.GetCell(cellIndex);

                            if (cell == null)
                                continue;

                            // Get the cell value as a string
                            string cellValue = cell.ToString();

                            //片段文字顏色
                            List<string> list = new List<string>();

                            //環境消毒設備 [$ValOr$]
                            string repOr = "[$ValOr$]";
                            double ValOr = disinfectorDatas.Sum(a => a.SprayerCount + a.DisinfectorCount + a.HotSmokeSachineCount
                                                + a.PressureWasherCount + a.SprayerCAR + a.SprayeSrHI
                                                + a.SprayeSrLO + a.SMOK + a.OtherCount);
                            if (cellValue.IndexOf(repOr) > -1)
                            {
                                string strSum = ValOr.ToString();
                                var text = cellValue.Replace(repOr, strSum);  // 替换段落中的文字
                                cell.SetCellValue(text);

                                cellValue = cell.ToString();
                                list.Add(strSum);
                            }

                            //1.1 (液態)環境消毒藥劑 [$ValEnvL$]
                            string rep1 = "[$ValEnvL$]";
                            double ValEnvL = double.Parse(d22_disinfectants.Where(a => a.DrugState != "固體").Sum(a => a.Amount).ToString());
                            if (cellValue.IndexOf(rep1) > -1)
                            {
                                string strSum = ValEnvL.ToString();
                                var text = cellValue.Replace(rep1, strSum);  // 替换段落中的文字
                                cell.SetCellValue(text);

                                cellValue = cell.ToString();
                                list.Add(strSum);
                            }

                            //1.2 (固環)環境消毒藥劑 [$ValEnvS$]
                            string rep2 = "[$ValEnvS$]";
                            double ValEnvS = double.Parse(d22_disinfectants.Where(a => a.DrugState == "固體").Sum(a => a.Amount).ToString());
                            if (cellValue.IndexOf(rep2) > -1)
                            {
                                string strSum = ValEnvS.ToString();
                                var text = cellValue.Replace(rep2, strSum);  // 替换段落中的文字
                                cell.SetCellValue(text);

                                cellValue = cell.ToString();
                                list.Add(strSum);
                            }

                            //2.1 (液態)登革熱藥劑 [$ValDengueL$]
                            string rep3 = "[$ValDengueL$]";
                            double ValDengueL = double.Parse(d23_disinfectants.Where(a => a.DrugState != "固體").Sum(a => a.Amount).ToString());
                            if (cellValue.IndexOf(rep3) > -1)
                            {
                                string strSum = ValDengueL.ToString();
                                var text = cellValue.Replace(rep3, strSum);  // 替换段落中的文字
                                cell.SetCellValue(text);

                                cellValue = cell.ToString();
                                list.Add(strSum);
                            }

                            //2.2 (固態)登革熱藥劑 [$ValDengueS$]
                            string rep4 = "[$ValDengueS$]";
                            double ValDengueS = double.Parse(d23_disinfectants.Where(a => a.DrugState == "固體").Sum(a => a.Amount).ToString());
                            if (cellValue.IndexOf(rep4) > -1)
                            {
                                string strSum = ValDengueS.ToString();
                                var text = cellValue.Replace(rep4, strSum);  // 替换段落中的文字
                                cell.SetCellValue(text);

                                cellValue = cell.ToString();
                                list.Add(strSum);
                            }

                            //片段文字顏色(CellStyle)
                            XSSFFont font = (XSSFFont)workbook.CreateFont();
                            font.Color = IndexedColors.Red.Index;
                            NPOIHelper.ReplaceCellStyleF1(workbook, cell, list, font);

                            if (list.Count > 0)
                            {
                                //跳出迴圈，只有一個欄位取代
                                rowIndex = sheet.LastRowNum;
                                break;
                            }
                        }
                    }

                    //二、環境消毒藥劑跟登革熱藥劑合併
                    //北基宜地區...
                    foreach (var type in typeCitys)
                    {
                        List<int> tCitys = type.Value.Split(',').Select(int.Parse).ToList();
                        var tmp2Disinfector = disinfectorDatas.Where(a => tCitys.Contains(a.CityId));

                        //消毒設備
                        int orAsum = tmp2Disinfector.Sum(a => a.SprayerCount + a.DisinfectorCount + a.HotSmokeSachineCount
                                                + a.PressureWasherCount + a.SprayerCAR + a.SprayeSrHI
                                                + a.SprayeSrLO + a.SMOK + a.OtherCount);

                        //(固體)環境消毒 + 登革熱
                        double antSumS = double.Parse(d22_disinfectants.Where(a => tCitys.Contains(a.CityId)).Where(a => a.DrugState == "固體").Sum(a => a.Amount).ToString())
                                        + double.Parse(d23_disinfectants.Where(a => tCitys.Contains(a.CityId)).Where(a => a.DrugState == "固體").Sum(a => a.Amount).ToString());

                        //(液體)環境消毒 + 登革熱
                        double antSumL = double.Parse(d22_disinfectants.Where(a => tCitys.Contains(a.CityId)).Where(a => a.DrugState != "固體").Sum(a => a.Amount).ToString())
                                        + double.Parse(d23_disinfectants.Where(a => tCitys.Contains(a.CityId)).Where(a => a.DrugState != "固體").Sum(a => a.Amount).ToString());

                        //遍歷每一列中的每一個Cell
                        for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
                        {
                            IRow row = sheet.GetRow(rowIndex);
                            if (row == null)
                                continue;

                            for (int cellIndex = 0; cellIndex < row.LastCellNum; cellIndex++)
                            {

                                var cell = row.GetCell(cellIndex);

                                if (cell == null)
                                    continue;

                                // Get the cell value as a string
                                string cellValue = cell.ToString();

                                //片段文字顏色
                                List<string> list = new List<string>();

                                //1.消毒設備 [$OrA7$]
                                var repStr = "[$OrA" + type.Key.ToString() + "$]";
                                if (cellValue.IndexOf(repStr) > -1)
                                {
                                    string strSum = orAsum.ToString();
                                    var text = cellValue.Replace(repStr, strSum);  // 替换段落中的文字
                                    cell.SetCellValue(text);

                                    cellValue = cell.ToString();
                                    list.Add(strSum);
                                }

                                //2.1(固體)環境消毒藥劑跟登革熱藥劑 [$AntA1S$]
                                var repStr2 = "[$AntA" + type.Key.ToString() + "S$]";
                                if (cellValue.IndexOf(repStr2) > -1)
                                {
                                    string strSum = antSumS.ToString();
                                    var text = cellValue.Replace(repStr2, strSum);  // 替换段落中的文字
                                    cell.SetCellValue(text);

                                    cellValue = cell.ToString();
                                    list.Add(strSum);
                                }

                                //2.2(液體)環境消毒藥劑跟登革熱藥劑 [$AntA1S$]
                                var repStr3 = "[$AntA" + type.Key.ToString() + "L$]";
                                if (cellValue.IndexOf(repStr3) > -1)
                                {
                                    string strSum = antSumL.ToString();
                                    var text = cellValue.Replace(repStr3, strSum);  // 替换段落中的文字
                                    cell.SetCellValue(text);

                                    cellValue = cell.ToString();
                                    list.Add(strSum);
                                }

                                //片段文字顏色(CellStyle)
                                XSSFFont font = (XSSFFont)workbook.CreateFont();
                                font.Color = IndexedColors.Red.Index;
                                NPOIHelper.ReplaceCellStyleF1(workbook, cell, list, font);

                            }
                        }
                    }


                    FileStream xlsFile = new FileStream(toPath, FileMode.Create, FileAccess.Write);
                    workbook.Write(xlsFile);
                    xlsFile.Close();
                    workbook.Close();

                    result = toPath;
                }
            }
            catch (Exception ex)
            {
                logger.Error("執行錯誤：簡報產製2_台灣地圖");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        /// <summary>
        /// 簡報產製3_飲用水質
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string Export3_TestWater(int diasterId)
        {
            string result = "";

            try
            {
                //匯出Excel
                //我要下載的檔案位置
                string filefolder = HttpContext.Current.Server.MapPath("~/FileDatas/Template/");
                string fileName = "(範本)簡報產製3_飲用水質.xlsx";
                string path = filefolder + fileName;

                //取得檔案名稱
                string filename = System.IO.Path.GetFileName(path);

                StringBuilder sb = new StringBuilder();
                string toFolder = HttpContext.Current.Server.MapPath("~/FileDatas/Temp/");
                string toFileName = fileName + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                string toPath = toFolder + toFileName.Replace("(範本)", "");

                using (FileStream stream = System.IO.File.OpenRead(path))
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0);
                    workbook.SetSheetName(workbook.GetSheetIndex(sheet), "飲用水質");


                    //oooooooooooooooooooo
                    //內容處理
                    var waters = GetWaterTotal(diasterId);

                    //遍歷每一列中的每一個Cell
                    for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        IRow row = sheet.GetRow(rowIndex);
                        if (row == null)
                            continue;

                        var cell = row.GetCell(0);
                        if (cell == null)
                            continue;

                        // Get the cell value as a string
                        string cellValue = cell.ToString();

                        //當下列的行高數量
                        int heightNum = 1;

                        //第1欄：縣市
                        var f = waters.Where(a => a.City == cellValue).FirstOrDefault();
                        if (f == null)
                            continue;

                        //第2欄：抽驗件數
                        cell = row.GetCell(1);
                        cell.SetCellValue(f.Count);

                        //第3欄：合格件數

                        //第4欄：不合格件數
                        cell = row.GetCell(3);
                        cell.SetCellValue(f.DisqualifiedCount);
                        heightNum = f.DisqualifiedCount;

                        //第5欄：檢驗中件數

                        //第6欄：說明(不合格淨水場、項目)
                        cell = row.GetCell(5);
                        cell.SetCellValue(f.DisqualifiedAddress);

                        //設定行高
                        if (heightNum > 0)
                        {
                            row.HeightInPoints = float.Parse((22.5 + (18 * (heightNum - 1))).ToString());
                        }
                    }

                    FileStream xlsFile = new FileStream(toPath, FileMode.Create, FileAccess.Write);
                    workbook.Write(xlsFile);
                    xlsFile.Close();
                    workbook.Close();

                    result = toPath;
                }
            }
            catch (Exception ex)
            {
                logger.Error("執行錯誤：簡報產製3_飲用水質");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        /// <summary>
        /// 簡報產製4_環境清潔車輛
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string Export4_Cars()
        {
            string result = "";

            try
            {
                //匯出Excel
                //我要下載的檔案位置
                string filefolder = HttpContext.Current.Server.MapPath("~/FileDatas/Template/");
                string fileName = "(範本)簡報產製4_環境清潔車輛.xlsx";
                string path = filefolder + fileName;

                //取得檔案名稱
                string filename = System.IO.Path.GetFileName(path);

                StringBuilder sb = new StringBuilder();
                string toFolder = HttpContext.Current.Server.MapPath("~/FileDatas/Temp/");
                string toFileName = fileName + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                string toPath = toFolder + toFileName.Replace("(範本)", "");

                using (FileStream stream = System.IO.File.OpenRead(path))
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0);
                    workbook.SetSheetName(workbook.GetSheetIndex(sheet), "環境清潔車輛");


                    //oooooooooooooooooooo
                    //內容處理
                    CityService CityService = new CityService();
                    var citys = CityService.GetAll();

                    //Header 縣市對應欄位(index)
                    Dictionary<string, int> dicIndexs = new Dictionary<string, int>();

                    IRow hrow = sheet.GetRow(1);
                    int index = -1;
                    foreach (ICell cell in hrow.Cells)
                    {
                        index++;
                        string city = cell.StringCellValue;
                        if (citys.Any(a => a.City == city))
                        {
                            dicIndexs.Add(city, index);
                        }
                    }

                    //資料列
                    for (int i = 2; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        
                        //第一個欄位
                        string h0_string = row.Cells[0].StringCellValue;
                        List<string> list = h0_string.Split('_').ToList();
                        if (list.Count > 1)
                        {
                            //類型
                            string type = list[0];
                            //車輛名稱
                            string name = list[1];

                            //修改第一欄名稱
                            row.Cells[0].SetCellValue(name);

                            var cars = GetCarsByCity().Where(a => a.VehicleType == type);
                            foreach (var dic in dicIndexs)
                            {
                                ICell cell = row.GetCell(dic.Value);
                                var datas = cars.Where(a => a.CityName == dic.Key);

                                double sum = double.Parse(datas.Sum(a => a.Count).ToString());

                                cell.SetCellValue(sum);
                                cell.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,#0");
                            }
                        }

                        //執行公式
                        if (h0_string == "合計")
                        {
                            //修改第一欄名稱
                            row.Cells[0].SetCellValue("合計");

                            foreach (var dic in dicIndexs)
                            {
                                ICell cell = row.GetCell(dic.Value);
                                workbook.GetCreationHelper().CreateFormulaEvaluator().EvaluateFormulaCell(cell);
                            }
                        }
                    }

                    FileStream xlsFile = new FileStream(toPath, FileMode.Create, FileAccess.Write);
                    workbook.Write(xlsFile);
                    xlsFile.Close();
                    workbook.Close();

                    result = toPath;
                }
            }
            catch (Exception ex)
            {
                logger.Error("執行錯誤：簡報產製4_環境清潔車輛");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        /// <summary>
        /// 簡報產製5_環境消毒物資
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string Export5_EnvDisinSupplies()
        {
            string result = "";

            try
            {
                //匯出Excel
                //我要下載的檔案位置
                string filefolder = HttpContext.Current.Server.MapPath("~/FileDatas/Template/");
                string fileName = "(範本)簡報產製5_環境消毒物資.xlsx";
                string path = filefolder + fileName;

                //取得檔案名稱
                string filename = System.IO.Path.GetFileName(path);

                StringBuilder sb = new StringBuilder();
                string toFolder = HttpContext.Current.Server.MapPath("~/FileDatas/Temp/");
                string toFileName = fileName + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                string toPath = toFolder + toFileName.Replace("(範本)", "");

                using (FileStream stream = System.IO.File.OpenRead(path))
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0);
                    workbook.SetSheetName(workbook.GetSheetIndex(sheet), "環境消毒物資");


                    //oooooooooooooooooooo
                    //內容處理
                    CityService CityService = new CityService();
                    var citys = CityService.GetAll();

                    //Header 縣市對應欄位(index)
                    Dictionary<string, int> dicIndexs = new Dictionary<string, int>();

                    IRow hrow = sheet.GetRow(1);
                    int index = -1;
                    foreach(ICell cell in hrow.Cells)
                    {
                        index++;
                        string city = cell.StringCellValue;
                        if (citys.Any(a => a.City == city))
                        {
                            dicIndexs.Add(city, index);
                        }
                    }

                    //資料列
                    IRow row;

                    //設備(臺) 2
                    row = sheet.GetRow(2);

                    //消毒設備
                    var disinfectorDatas = GetDisinfector();

                    foreach (var dic in dicIndexs)
                    {
                        ICell cell = row.GetCell(dic.Value);
                        var tmp2Disinfector = disinfectorDatas.Where(a => a.City == dic.Key);

                        int sum = tmp2Disinfector.Sum(a => a.SprayerCount + a.DisinfectorCount + a.HotSmokeSachineCount
                                                + a.PressureWasherCount + a.SprayerCAR + a.SprayeSrHI
                                                + a.SprayeSrLO + a.SMOK + a.OtherCount);

                        cell.SetCellValue(sum);
                    }

                    //消毒藥劑 - 液態 3
                    var disinfectants = GetDisinfectantEnumEnvironment();

                    row = sheet.GetRow(3);
                    foreach (var dic in dicIndexs)
                    {
                        ICell cell = row.GetCell(dic.Value);
                        var tmp2disinfectants = disinfectants.Where(a => a.DrugState != "固體").Where(a => a.City == dic.Key);

                        double sum = double.Parse(tmp2disinfectants.Sum(a => a.Amount).ToString());

                        cell.SetCellValue(sum);
                        cell.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,#0");
                    }

                    //消毒藥劑 - 固態 4
                    row = sheet.GetRow(4);
                    foreach (var dic in dicIndexs)
                    {
                        ICell cell = row.GetCell(dic.Value);
                        var tmp2disinfectants = disinfectants.Where(a => a.DrugState == "固體").Where(a => a.City == dic.Key);

                        double sum = double.Parse(tmp2disinfectants.Sum(a => a.Amount).ToString());

                        cell.SetCellValue(sum);
                        cell.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,#0");
                    }

                    //登革熱藥劑 - 液態(公升) 5
                    var dengues = GetDisinfectantEnumDengue();

                    row = sheet.GetRow(5);
                    foreach (var dic in dicIndexs)
                    {
                        ICell cell = row.GetCell(dic.Value);
                        var tmp2dengues = dengues.Where(a => a.DrugState != "固體").Where(a => a.City == dic.Key);

                        double sum = double.Parse(tmp2dengues.Sum(a => a.Amount).ToString());

                        cell.SetCellValue(sum);
                        cell.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,#0");
                    }

                    //登革熱藥劑 - 固態(公斤) 6
                    row = sheet.GetRow(6);
                    foreach (var dic in dicIndexs)
                    {
                        ICell cell = row.GetCell(dic.Value);
                        var tmp2dengues = dengues.Where(a => a.DrugState == "固體").Where(a => a.City == dic.Key);

                        double sum = double.Parse(tmp2dengues.Sum(a => a.Amount).ToString());

                        cell.SetCellValue(sum);
                        cell.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,#0");
                    }

                    FileStream xlsFile = new FileStream(toPath, FileMode.Create, FileAccess.Write);
                    workbook.Write(xlsFile);
                    xlsFile.Close();
                    workbook.Close();

                    result = toPath;
                }
            }
            catch (Exception ex)
            {
                logger.Error("執行錯誤：簡報產製5_環境消毒物資");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        /// <summary>
        /// 車輛(環境清潔車輛)全部 加總統計
        /// </summary>
        /// <returns></returns>
        public static List<CarsModel> GetCarsTotal()
        {
            VehicleTypeRepository VehicleTypeRepository = new VehicleTypeRepository();

            var carTypes = VehicleTypeRepository.GetList();
            var carDatas = GetCarsByCity();
            var Totals = carTypes.GroupJoin(carDatas, a => a.Name, b => b.VehicleName, (o, c) => new CarsModel
            {
                Type = o.Type.Trim(),
                TypeName = o.Name.Trim(),
                Count = c.Sum(a => a.Count),
            }).ToList();

            return Totals;
        }

        /// <summary>
        /// 車輛(環境清潔車輛)全部 縣市統計
        /// </summary>
        /// <returns></returns>
        public static List<VehicleCountModel> GetCarsByCity()
        {
            VehicleService VehicleService = new VehicleService();
            var Totals = VehicleService.GetCarsCountByCity();

            return Totals;
        }

        /// <summary>
        /// 車輛(類型)全部
        /// </summary>
        /// <returns></returns>
        public static List<VehicleTypeModel> GetCarsType()
        {
            VehicleTypeRepository VehicleTypeRepository = new VehicleTypeRepository();
            var Totals = VehicleTypeRepository.GetList();

            return Totals;
        }

        /// <summary>
        /// 消毒設備全部
        /// </summary>
        /// <returns></returns>
        public static List<DisinfectorSummaryCityReportModel> GetDisinfector()
        {
            DisinfectorService DisinfectorService = new DisinfectorService();
            var Totals = DisinfectorService.GetSummaryCityReport();

            return Totals;
        }

        /// <summary>
        /// 消毒藥劑全部
        /// </summary>
        /// <returns></returns>
        public static List<DisinfectantCityReportModel> GetDisinfectant()
        {
            DisinfectantService DisinfectantService = new DisinfectantService();
            var Totals = DisinfectantService.GetTownReport(new DisinfectantReportFilterParameter
            {
            });

            return Totals;
        }

        /// <summary>
        /// 消毒藥劑(Enum.環境消毒)
        /// </summary>
        /// <returns></returns>
        public static List<DisinfectantCityReportModel> GetDisinfectantEnumEnvironment()
        {
            DisinfectantService DisinfectantService = new DisinfectantService();
            var Environments = DisinfectantService.GetTownReport(new DisinfectantReportFilterParameter
            {
                UseType = DisinfectantUseTypeEnum.Environment,
            });

            return Environments;
        }

        /// <summary>
        /// 消毒藥劑(Enum.登革熱)
        /// </summary>
        /// <returns></returns>
        public static List<DisinfectantCityReportModel> GetDisinfectantEnumDengue()
        {
            DisinfectantService DisinfectantService = new DisinfectantService();
            var Dengues = DisinfectantService.GetTownReport(new DisinfectantReportFilterParameter
            {
                UseType = DisinfectantUseTypeEnum.Dengue,
            });

            return Dengues;
        }

        /// <summary>
        /// 水質抽驗全部
        /// </summary>
        /// <returns></returns>
        public static List<WaterCheckStatisticsEasyViewModel> GetWaterTotal(int diasterId)
        {
            WaterCheckService WaterCheckService = new WaterCheckService();
            var Totals = WaterCheckService.StatisticsEasy(diasterId);

            return Totals;
        }

        public class CarsModel
        {
            public string Type { get; set; }
            public string TypeName { get; set; }
            public int Count { get; set; }
        }
    }
}