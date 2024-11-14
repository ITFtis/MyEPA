using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Services;
using NPOI.HSSF.UserModel;
using NPOI.OpenXml4Net.OPC.Internal;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
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
                var tmp1Car = GetCars();
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

                    //xxxxxxxxxxx 92 29
                    var zzzzz = disinfectorDatas.Where(a => a.City == "金門縣");

                    int s1 = zzzzz.Sum(a => a.SprayerCount + a.DisinfectorCount + a.HotSmokeSachineCount
                                            + a.PressureWasherCount + a.SprayerCAR + a.SprayeSrHI
                                            + a.SprayeSrLO + a.SMOK + a.OtherCount);

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

                    //消毒藥劑 - 固態 4

                    //登革熱藥劑 - 液態(公升) 5

                    //登革熱藥劑 - 固態(公斤)

                    //oooooooooooooooooooo

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
                logger.Error("執行錯誤：簡報產製5_環境消毒物資");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        /// <summary>
        /// 車輛(環境清理機具)全部
        /// </summary>
        /// <returns></returns>
        public static List<CarsModel> GetCars()
        {
            VehicleTypeRepository VehicleTypeRepository = new VehicleTypeRepository();
            VehicleService VehicleService = new VehicleService();

            var carTypes = VehicleTypeRepository.GetList();
            var carDatas = VehicleService.GetCarsCountByCity();
            var Totals = carTypes.GroupJoin(carDatas, a => a.Name, b => b.VehicleName, (o, c) => new CarsModel
            {
                Type = o.Type.Trim(),
                TypeName = o.Name.Trim(),
                Count = c.Sum(a => a.Count),
            }).ToList();

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

        public class CarsModel
        {
            public string Type { get; set; }
            public string TypeName { get; set; }
            public int Count { get; set; }
        }
    }
}