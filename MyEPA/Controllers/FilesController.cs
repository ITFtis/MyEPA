using MyEPA.Helper;
using MyEPA.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class FilesController : LoginBaseController
    {
        [System.Web.Mvc.HttpPost]
        public ActionResult Upload(HttpPostedFileBase file,string title)
        {
            string fileText = Request["filetext"];
            string DOWNADMIN = Request["DOWNADMIN"];
            string City = Session["AuthenticateCity"].ToString().Trim();
            if (file != null && file.ContentLength > 0)
            {
                FileUploadRepository FR = new FileUploadRepository(); 
                var fileName = Path.GetFileName(file.FileName);

                var path = UploadFileHelper.GetServerMapPath(fileName);

                FR.Create(new FileUploadModel
                {
                    DOWNADMIN = DOWNADMIN,
                    FILENAME = fileName,
                    FILETEXT = fileText,
                    UPTIME = DateTimeHelper.GetCurrentTime(),
                    USERLV = 0,
                    USERID = City
                });
                file.SaveAs(path);
            }
            return RedirectToAction("B6x1","EPB",new { title });
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Delete(string FileName, string ID, string title)
        {
            var path = UploadFileHelper.GetServerMapPath(FileName);
            try
            {
                System.IO.File.Delete(path);
            }
            catch(Exception ex)
            {

            }
            FileUploadRepository FR = new FileUploadRepository();
            ViewBag.msg = FR.Delete(ID);

            AdminResultModel result = new AdminResultModel 
            {
                IsSuccess = true
            };

            return JsonResult(result);
        }


        public ActionResult Download(string FileName)
        {
             var path = UploadFileHelper.GetServerMapPath(FileName);
            //讀成串流
            Stream iStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            //回傳出檔案
            return File(iStream, "application/unknown", FileName);
        }
    }
}