using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Office.Interop.Word;
using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Services;
using NPOI.HSSF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using Spire.Pdf.Graphics;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Xceed.Words.NET;

namespace MyEPA.Controllers.Report
{
    public class EasyReportController : LoginBaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: EasyReport
        //public ActionResult Index()
        //{
        //    return View();
        //}

        /// <summary>
        /// 緊急應變統計表
        /// </summary>
        /// <returns></returns>
        public ActionResult DisasterResponseReport()
        {
            if (!GetIsAdmin())
                return RedirectToAction("LoginRedirect", "Home");

            return View();
        }

        /// <summary>
        /// 匯出緊急應變統計表
        /// </summary>
        public ActionResult ExportDisasterResponseReport()
        {
            //1.車輛
            VehicleTypeRepository VehicleTypeRepository = new VehicleTypeRepository();
            VehicleService VehicleService = new VehicleService();
            //CityService CityService = new CityService();

            //var cars = VehicleService.GetCarTypes();
            var carTypes = VehicleTypeRepository.GetList();
            var carDatas = VehicleService.GetCarsCountByCity();
            var tmp1Car = carTypes.GroupJoin(carDatas, a => a.Name, b => b.VehicleName, (o, c) => new
            {
                Type = o.Type.Trim(),
                TypeName = o.Name.Trim(),
                Count = c.Sum(a => a.Count),
            }).ToList();

            //2.1 消毒設備            
            var disinfectorDatas = EasyReport.GetDisinfector();
            var tmp2Disinfector = disinfectorDatas;

            //匯出Excel
            //我要下載的檔案位置
            string filefolder = Server.MapPath("~/FileDatas/Template/");
            string fileName = "(範本)緊急應變統計表_1.xlsx";
            string path = filefolder + fileName;

            //取得檔案名稱
            string filename = System.IO.Path.GetFileName(path);

            StringBuilder sb = new StringBuilder();
            string toFolder = Server.MapPath("~/FileDatas/Temp/");
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
                var d22_disinfectants = EasyReport.GetDisinfectantEnumEnvironment();
                dic2.Add("EnvironmentSolidAmount", d22_disinfectants.Where(a => a.DrugState == "固體").Sum(a => a.Amount).ToString());
                dic2.Add("EnvironmentLiquidAmount", d22_disinfectants.Where(a => a.DrugState != "固體").Sum(a => a.Amount).ToString());

                //2.3 => 登革熱
                var d23_disinfectants = EasyReport.GetDisinfectantEnumDengue();
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
            }

            //讀成串流
            var iStream = new FileStream(toPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            //回傳出檔案
            return File(iStream, GetContentType("xlsx"), Path.GetFileName(toPath));

            //return RedirectToAction("DisasterResponseReport");
        }
    }
}