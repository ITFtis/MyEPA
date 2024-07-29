using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using MyEPA.Enums;
using MyEPA.EPA.Attribute;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    [LoginRequired]
    public class LoginBaseController : BaseController
    {
        DiasterService diasterService = new DiasterService();
        public DutyEnum _Duty { get; set; }

        public void Logout()
        {
            Session["IsAdmin"] = null;
            Session["UserId"] = null;
            Session["AuthenticateId"] = null;
            Session["Name"] = null;
            Session["AuthenticateCity"] = null;
            Session["AuthenticateTown"] = null;
            Session["AuthenticateCityId"] = null;
            Session["AuthenticateTownId"] = null;
            Session["DutyId"] = null;
            Session["AuthenticateDuty"] = null;
            Session["ContactManualDuty"] = null;
            Session["ContactManualDepartmentId"] = null;
            Session["ContactManualDepartment"] = null;
            Session["PwdUpdateDate"] = null;
        }

        public LoginBaseController()
        {
            if (IsLogin())
            {
                _Duty = (DutyEnum)GetUserDutyId();
                ViewBag.DutyId = _Duty;
                ViewBag.IsDiasterRuning = diasterService.IsExistsRuning();
                ViewBag.IsAdmin = GetIsAdmin();
                ViewBag.Notices = new NoticeService().GetByFilter(new NoticeFilterParameter());
                ViewBag.Area = GetArea();
                ViewBag.ContactManualDuty = GetContactManualDuty();
                var user = GetUserBrief();
                ViewBag.AlertRecResource = new RecResourceService().GetAlter(user); //"xxx";
            }
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            List<string> Edits = new List<string>();
            Edits.Add("/Users/EditPwd");

            //302，已設定Redirect，再一次將出現錯誤
            if (HttpContext.Response.StatusCode != 302)
            {
                var user = GetUserBrief();

                if (!Edits.Contains(HttpContext.Request.FilePath) && user != null)
                {
                    //密碼超過3個月                    
                    bool IsResetPwd = false;
                    int diffDays = DateFormat.ToDiffDays(user.PwdUpdateDate, DateTime.Now);

                    if (user.PwdUpdateDate == DateTime.MinValue)
                    {
                        IsResetPwd = true;
                    }
                    else if (diffDays <= 0)
                    {
                        IsResetPwd = true;
                    }

                    if (IsResetPwd)
                    {
                        filterContext.HttpContext.Response.Redirect("~/Users/EditPwd?OverTime=Y");
                    }
                }
            }

            string name = (string)ControllerContext.RouteData.Values["controller"];
            if (Enum.TryParse(name, true, out ContactManualBreadCrumbTypeEnum type))
            {
                ViewBag.BreadCrumbs = new BreadCrumbService().GetBreadCrumbsByType(type);
            }
        }
        public Dictionary<string, List<HttpPostedFileBase>> GetUploadFiles()
        {
            if(Request == null)
            {
                return new Dictionary<string, List<HttpPostedFileBase>>();
            }
            Dictionary<string, List<HttpPostedFileBase>> dicFiles = new Dictionary<string, List<HttpPostedFileBase>>();

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files.Get(i);

                if(string.IsNullOrWhiteSpace(file.FileName))
                {
                    continue;
                }

                string key = Request.Files.GetKey(i);

                if (dicFiles.ContainsKey(key))
                {
                    List<HttpPostedFileBase> files = dicFiles[key];
                    files.Add(file);
                }
                else
                {
                    dicFiles.Add(Request.Files.GetKey(i), new List<HttpPostedFileBase>() { file });
                }
            }

            return dicFiles;
        }

      
       
    }
}