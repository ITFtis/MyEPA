using MyEPA.Extensions;
using MyEPA.Helper;
using System.IO;
using Microsoft.Office.Interop.Word;

namespace MyEPA.Services
{
    public partial class ContactManualReportService
    {
        public class PDFHelper
        {
            public static string ConvertToToPDF(Stream stream)
            {
                byte[] wordContent = stream.ToByte();

                string tmpFile = Path.GetTempFileName();
                FileStream tmpFileStream = File.OpenWrite(tmpFile);
                tmpFileStream.Write(wordContent, 0, wordContent.Length);
                tmpFileStream.Close();

                var app = new Application();

                var wordDocument = app.Documents.Open(tmpFile);

                wordDocument.TablesOfContents[1].Update();

                string fileName = DateTimeHelper.GetCurrentTime().ToString("yyyyMMddHHmmssfff");
                string path = UploadFileHelper.GetServerMapPath("ContactManualReport", $"ConvertToToPDF{fileName}.pdf");

                wordDocument.ExportAsFixedFormat(path, WdExportFormat.wdExportFormatPDF);

                wordDocument.Close();

                app.Quit();

                return path;
            }
        }
    }
}