using MyEPA.Models.FilterParameter;
using MyEPA.Models;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace MyEPA.Controllers.Rec
{
    public class UserLoginController : LoginBaseController
    {
        UsersService UsersService = new UsersService();

        // GET: UserLogin
        public ActionResult Index()
        {
            ViewBag.Msg = TempData["Msg"];

            UsersFilterParameter filter = new UsersFilterParameter()
            {
            };

            IEnumerable<UserLoginViewModel> iquery = UsersService.GetUserLoginByFilter(filter);
            iquery = iquery.OrderByDescending(a => a.Id);
            List<UserLoginViewModel> result = iquery.ToList();

            return View(result);
        }

        /// <summary>
        /// 匯出Excel
        /// </summary>
        public ActionResult ExportList()
        {
            string toPath = "";
            //string toPath = EasyReport.Export1_Urgent();

            if (toPath != "")
            {
                //讀成串流
                var iStream = new FileStream(toPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                //回傳出檔案
                return File(iStream, GetContentType("xlsx"), Path.GetFileName(toPath));
            }
            else
            {
                TempData["Msg"] = "執行失敗：" + "匯出Excel";
                return RedirectToAction("Index");
            }
        }
    }
}