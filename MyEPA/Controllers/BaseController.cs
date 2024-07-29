using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class BaseController : Controller
    {
        HttpContextWrapper Context = new HttpContextWrapper(System.Web.HttpContext.Current);
        public UserBriefModel GetUserBrief()
        {
            if (Context.Session["AuthenticateId"] == null)
            {
                return null;
            }

            return new UserBriefModel
            {
                UserId = GetUserId(),
                City = GetUserCity(),
                UserName = GetUserName(),
                Town = GetUserTown(),
                CityId = GetUserCityId(),
                TownId = GetUserTownId(),
                Duty = (DutyEnum)GetUserDutyId(),
                Name = GetName(),
                IsAdmin = GetIsAdmin(),
                Area = GetArea(),
                ContactManualDuty = GetContactManualDuty(),
                ContactManualDepartmentId = GetUserContactManualDepartmentId(),
                ContactManualDepartment = GetUserContactManualDepartment()
            };
        }
        public string GetUserContactManualDepartment()
        {
            return GetSessionValue("ContactManualDepartment");
        }

        public int? GetUserContactManualDepartmentId()
        {
            return GetNullableIntSessionValue("ContactManualDepartmentId");
        }
        public ContactManualDutyEnum GetContactManualDuty()
        {
            return (ContactManualDutyEnum)GetIntSessionValue("ContactManualDuty");
        }
        private Nullable<int> GetNullableIntSessionValue(string name)
        {
            return GetSessionValue(name).TryToInt();
        }

        private int GetIntSessionValue(string name)
        {
            return GetSessionValue(name).TryToInt().GetValueOrDefault();
        }
        private string GetSessionValue(string name)
        {
            var value = Context.Session[name];

            if (value == null)
            {
                return null;
            }

            return value.ToString();
        }
        public int GetUserCityId()
        {
            return GetIntSessionValue("AuthenticateCityId");
        }
        public int GetUserTownId()
        {
            return GetIntSessionValue("AuthenticateTownId");
        }
        public string GetUserCity()
        {
            return GetSessionValue("AuthenticateCity");
        }
        public string GetUserTown()
        {
            return GetSessionValue("AuthenticateTown");
        }
        public string GetContentType(string fileName)
        {
            return MimeMapping.GetMimeMapping(fileName);
        }
        public int GetUserDutyId()
        {
            return GetIntSessionValue("DutyId");
        }
        public int GetUserId()
        {
            return GetIntSessionValue("UserId");
        }
        public string GetUserName()
        {
            return Context.Session["AuthenticateId"].ToString();
        }
        public string GetName()
        {
            return Context.Session["Name"].ToString();
        }
        public bool IsLogin()
        {
            if (Context.Session == null)
            {
                return false;
            }
            return Context.Session["AuthenticateId"] != null;
        }


        public AreaEnum? GetArea()
        {
            int? areaId = GetSessionValue("Area").TryToInt();
            if (areaId.HasValue == false)
            {
                return null;
            }
            return (AreaEnum)areaId.Value;
        }
        public bool GetIsAdmin()
        {
            return GetSessionValue("IsAdmin").TryToBool().GetValueOrDefault();
        }
        public ActionResult FileByPDF<T>(List<T> model, string fileName, List<string> ignoreFields = null) where T : class
        {
            FileDataBaseModels fileData = GeneratePDF(model, fileName, ignoreFields);
            return File(fileData);
        }
        public ActionResult FileByODS<T>(List<T> model, string fileName, List<string> ignoreFields = null) where T : class
        {
            FileDataBaseModels fileData = GenerateODS(model, fileName, ignoreFields);
            return File(fileData);
        }
        public ActionResult File(Stream stream,string fileName)
        {
            return File(stream, GetContentType(fileName), fileName);
        }
        public ActionResult File(FileDataBaseModels file)
        {
            return File(file.FileByte, GetContentType(file.RealFileNameAndExtension), file.UserFileName);
        }
        public ActionResult FilePathResult(FileDataBaseModels file)
        {
            return File(file.ServerMapPath, GetContentType(file.RealFileNameAndExtension));
        }
        public string ViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public FileDataBaseModels GenerateODS<T>(List<T> model, string fileName, List<string> ignoreFields = null) where T : class
        {
            var dt = model.ToDataTable(ignoreFields);

            var dataTables = new DataTableViewModel
            {
                DataTable = dt,
                Title = fileName
            }.ToListCollection();
             
            byte[] pdfFile = ExportToODS(dataTables).ToArray();

            string extension = ".ods";
            if (fileName.Contains(extension) == false)
            {
                fileName += extension;
            }
            return new FileDataBaseModels
            {
                Extension = extension,
                FileByte = pdfFile,
                FileLength = pdfFile.Length,
                RealFileName = fileName,
                UserFileName = fileName
            };
        }
        public FileDataBaseModels GenerateExcel<T>(List<T> model, string fileName, List<string> ignoreFields = null) where T : class
        {
            var dt = model.ToDataTable(ignoreFields);

            var dataTables = new DataTableViewModel
            {
                DataTable = dt,
                Title = fileName
            }.ToListCollection();

            byte[] pdfFile = ExportToExcel(dataTables).ToArray();

            string extension = ".xlsx";
            if (fileName.Contains(extension) == false)
            {
                fileName += extension;
            }
            return new FileDataBaseModels
            {
                Extension = extension,
                FileByte = pdfFile,
                FileLength = pdfFile.Length,
                RealFileName = fileName,
                UserFileName = fileName
            };
        }
        public FileDataBaseModels GenerateExcelByManyTable<T>(T model, string fileName) where T : class
        {
            byte[] pdfFile = ExportToExcel(model.ToDataTables()).ToArray();
            string extension = ".xlsx";
            if (fileName.Contains(extension) == false)
            {
                fileName += extension;
            }
            return new FileDataBaseModels
            {
                Extension = extension,
                FileByte = pdfFile,
                FileLength = pdfFile.Length,
                RealFileName = fileName,
                UserFileName = fileName
            };
        }
        public FileDataBaseModels GenerateODSByManyTable<T>(T model, string fileName) where T : class
        {
            var dataTables = model.ToDataTables();

            byte[] pdfFile = ExportToODS(dataTables).ToArray();

            string extension = ".ods";
            if (fileName.Contains(extension) == false)
            {
                fileName += extension;
            }
            return new FileDataBaseModels
            {
                Extension = extension,
                FileByte = pdfFile,
                FileLength = pdfFile.Length,
                RealFileName = fileName,
                UserFileName = fileName
            };
        }
        public FileDataBaseModels GeneratePDF<T>(List<T> model, string fileName, List<string> ignoreFields = null) where T : class
        {
            var dt = model.ToDataTable(ignoreFields);
            
            byte[] pdfFile = ExportToPdf(new DataTableViewModel 
            {
                DataTable = dt,
                Title = fileName
            }, fileName);
            string extension = ".pdf";
            if (fileName.Contains(extension) == false)
            {
                fileName += extension;
            }
            return new FileDataBaseModels
            {
                Extension = extension,
                FileByte = pdfFile,
                FileLength = pdfFile.Length,
                RealFileName = fileName,
                UserFileName = fileName
            };
        }
        public FileDataBaseModels GeneratePDFByManyTable<T>(T model, string fileName) where T : class
        {
            byte[] pdfFile = ExportToPdf(model.ToDataTables(), fileName).ToArray();
            string extension = ".pdf";
            if (fileName.Contains(extension) == false)
            {
                fileName += extension;
            }
            return new FileDataBaseModels
            {
                Extension = extension,
                FileByte = pdfFile,
                FileLength = pdfFile.Length,
                RealFileName = fileName,
                UserFileName = fileName
            };
        }
        public FileDataBaseModels GeneratePDFByHtml(string htmlText, string fileName, Rectangle pageSize = null)
        {
            byte[] pdfFile = this.ConvertHtmlTextToPDF(pageSize, htmlText);
            string extension = ".pdf";
            return new FileDataBaseModels
            {
                Extension = extension,
                FileByte = pdfFile,
                FileLength = pdfFile.Length,
                RealFileName = fileName,
                UserFileName = fileName
            };
        }
        public FileDataBaseModels GeneratePDFByHtml(string viewName, object result, string fileName, Rectangle pageSize = null)
        {
            string htmlText = ViewToString(viewName, result);
            return GeneratePDFByHtml(htmlText, fileName, pageSize);
        }
        public FileDataBaseModels GeneratePDFByHtml(object result, string fileName, Rectangle pageSize = null)
        {
            return GeneratePDFByHtml(ControllerContext.RouteData.Values["action"].ToString(), result, fileName, pageSize);
        }
        /// <summary>
        /// 將Html文字 輸出到PDF檔裡
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public byte[] ConvertHtmlTextToPDF(Rectangle pageSize, string htmlText)
        {
            if (string.IsNullOrEmpty(htmlText))
            {
                return null;
            }
            //避免當htmlText無任何html tag標籤的純文字時，轉PDF時會掛掉，所以一律加上<p>標籤
            htmlText = "<p>" + htmlText + "</p>";
            htmlText = htmlText.Replace("<head>", string.Empty);
            htmlText = htmlText.Replace("</head>", string.Empty);

            MemoryStream outputStream = new MemoryStream();//要把PDF寫到哪個串流
            byte[] data = Encoding.UTF8.GetBytes(htmlText);//字串轉成byte[]
            MemoryStream msInput = new MemoryStream(data);
            Document doc = new Document(pageSize);//要寫PDF的文件，建構子沒填的話預設直式A4
 
            PdfWriter writer = PdfWriter.GetInstance(doc, outputStream);
            //指定文件預設開檔時的縮放為100%
            PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
            //開啟Document文件 
            doc.Open();
            //使用XMLWorkerHelper把Html parse到PDF檔裡
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8, new UnicodeFontFactory());
            //將pdfDest設定的資料寫到PDF檔
            PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
            writer.SetOpenAction(action);
            doc.Close();
            msInput.Close();
            outputStream.Close();
            //回傳PDF檔案 
            return outputStream.ToArray();
        }
        public ActionResult JsonResult(object result)
        {
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(result), "application/json");
        }
        private MemoryStream ExportToPdf(List<DataTableViewModel> dataTables, string fileName)
        {
            Document doc = new Document(PageSize.A4.Rotate());

            MemoryStream outputStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, outputStream);

            doc.Open();

            string fontPath = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\..\Fonts\kaiu.ttf";
            BaseFont bfChinese = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font5 = new Font(bfChinese, 14);



            SetMessage(doc, fileName, new Font(bfChinese, 20));
            SetNewLine(doc);
            for (int i = 0; i < dataTables.Count; i++)
            {
                var dt = dataTables[i];
                SetTableTitle(doc, dt.Title, font5);
                SetDataTable(doc, dt.DataTable, font5);
                if (i % 2 == 0 && i != dataTables.Count - 1)
                {
                    SetNewLine(doc);
                }
            }

            //指定文件預設開檔時的縮放為100%
            //PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
            //將pdfDest設定的資料寫到PDF檔
            //PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
            // writer.SetOpenAction(action);


            doc.Close();
            outputStream.Close();

            return outputStream;
        }
        private MemoryStream ExportToExcel(List<DataTableViewModel> dataTables)
        {

            XLWorkbook wb = new XLWorkbook();

            foreach (var dataTable in dataTables)
            {
                wb.Worksheets.Add(dataTable.DataTable, dataTable.Title.Replace("[",string.Empty).Replace("]", string.Empty));
            }

            MemoryStream outputStream = new MemoryStream();

            wb.SaveAs(outputStream);
            
            return outputStream;
        }

        private byte[] ExportToPdf(DataTableViewModel dataTable, string fileName)
        {
            Document doc = new Document(PageSize.A4.Rotate());

            MemoryStream outputStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, outputStream);

            doc.Open();

            string fontPath = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\..\Fonts\kaiu.ttf";
            BaseFont bfChinese = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font5 = new Font(bfChinese, 14);

            SetMessage(doc, fileName, new Font(bfChinese, 20));
            SetNewLine(doc);
            SetDataTable(doc, dataTable.DataTable, font5);

            doc.Close();
            outputStream.Close();

            return outputStream.ToArray();
        }

        private static void SetDataTable(Document doc, DataTable dt, Font font5)
        {
            PdfPTable table = new PdfPTable(dt.Columns.Count);

            List<float> widths = new List<float>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                widths.Add(4f);
            }
            table.SetWidths(widths.ToArray());

            table.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell(new Phrase("Products"));

            cell.Colspan = dt.Columns.Count;

            foreach (DataColumn c in dt.Columns)
            {
                table.AddCell(new Phrase(c.ColumnName, font5));
            }

            foreach (DataRow r in dt.Rows)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        table.AddCell(new Phrase(r[i].ToString(), font5));
                    }
                }
            }

            doc.Add(table);
        }
        private static void SetTableTitle(Document doc, string title, Font font5)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return;
            }
            PdfPTable table = new PdfPTable(1);
            table.WidthPercentage = 100;
            table.AddCell(new Phrase(title, font5));
            doc.Add(table);
        }
        private static void SetNewLine(Document doc)
        {
            SetMessage(doc, Environment.NewLine);
        }
        private static void SetMessage(Document doc, string message, Font font5 = null)
        {
            Paragraph simplePara = new Paragraph();
            if (font5 == null)
            {
                simplePara.Add(new Paragraph(message));
            }
            else
            {
                simplePara.Add(new Paragraph(message, font5));
            }

            doc.Add(simplePara);
        }
        private MemoryStream ExportToODS(List<DataTableViewModel> dataTables)
        {
            var excelStream = ExportToExcel(dataTables);

            Workbook workbook = new Workbook();

            workbook.LoadFromStream(excelStream);

            MemoryStream outputStream = new MemoryStream();

            workbook.SaveToStream(outputStream, FileFormat.ODS);

            return outputStream;
        }
    }
}