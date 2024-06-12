using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Office.Interop.Word;
using MyEPA.Models;
using MyEPA.Repositories;
using MyEPA.Services;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

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
            var cars = VehicleTypeRepository.GetList();
            var carsDatas = VehicleService.GetCarsCountByCity();

            var car1 = cars.GroupJoin(carsDatas, a => a.Name, b => b.VehicleName, (o, c) => new
            {
                Type = o.Type.Trim(),
                TypeName = o.Name.Trim(),
                Count = c.Sum(a => a.Count),
            }).ToList();

            //匯出Excel
            //我要下載的檔案位置
            string filefolder = Server.MapPath("~/FileDatas/Template/");
            string fileName = "(範本)應變支援內容說明.xlsx";
            string path = filefolder + fileName;

            //取得檔案名稱
            string filename = System.IO.Path.GetFileName(path);

            StringBuilder sb = new StringBuilder();
            string toFolder = Server.MapPath("~/FileDatas/Temp/");
            string toFileName = fileName + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
            string toPath = toFolder + toFileName;

            using (FileStream stream = System.IO.File.OpenRead(path))
            {
                XSSFWorkbook workbook = new XSSFWorkbook(stream);
                XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0);
                workbook.SetSheetName(workbook.GetSheetIndex(sheet), "環保資源整備與應變簡報");

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