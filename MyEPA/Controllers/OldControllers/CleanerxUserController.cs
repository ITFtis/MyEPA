using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Services;

namespace MyEPA.Controllers
{
    public class CleanerxUserController : LoginBaseController
    {
        UsersRepository UsersRepository = new UsersRepository();
        TownService TownService = new TownService();
        public ActionResult C5x1()
        {
            ViewBag.City = Session["AuthenticateCity"].ToString();
            ViewBag.UserId = Session["AuthenticateId"];
            return View("~/Views/Cleaner/C5x1.cshtml");
        }

        public ActionResult C5x2()
        {
            UserModel User = new UserModel();
            string City = Session["AuthenticateCity"].ToString();
            string Town = Session["AuthenticateTown"].ToString();
            ViewBag.Data = User.Search("ALL", City, Town, "ALL", "ALL");
            ViewBag.City = City;
            ViewBag.Town= Town;
            return View("~/Views/Cleaner/C5x2.cshtml");
        }

        public ActionResult C5x3()
        {
            UserModel User = new UserModel();
            string City = Session["AuthenticateCity"].ToString();
            string Town = Session["AuthenticateTown"].ToString();
            ViewBag.Data = User.Search("ALL", City, Town, "ALL", "ALL");
            ViewBag.City = City;
            ViewBag.Town = Town;
            return View("~/Views/Cleaner/C5x3.cshtml");
        }

        public ActionResult ChangePwd()
        {
            UserModel User = new UserModel();
            string Id = Session["AuthenticateId"].ToString().Trim();
            string NewPwd = Request["NewPwd"].Trim();
            string OldPwd = Request["OldPwd"].Trim();
            string OldCorrectPwd = Session["Pwd"].ToString();

            if (OldPwd == OldCorrectPwd)
            {
                ViewBag.Msg = User.ChangePwd(Id, NewPwd); ViewBag.UserId = Id;
                Session["Pwd"] = NewPwd;
            }
            else
            { ViewBag.Msg = "抱歉，您輸入的舊密碼錯了，所以無法更新密碼"; ViewBag.UserId = Id; }

            ViewBag.City = Session["AuthenticateCity"].ToString().Trim();

            string Duty = Session["AuthenticateDuty"].ToString().Trim();
            ViewBag.UserId = Id;
            return View("~/Views/Cleaner/C5x1.cshtml");
        }

        public ActionResult ChangeVoicePwd()
        {
            UserModel User = new UserModel();
            string Id = Session["AuthenticateId"].ToString().Trim();

            string NewVoicePwd = Request["NewVoicePwd"].Trim();
            ViewBag.Msg = User.ChangeVoicePwd(Id, NewVoicePwd);

            ViewBag.UserId = Id;
            return View("~/Views/Cleaner/C5x1.cshtml");
        }

        public ActionResult Delete()
        {
            string DeleteId = Request["DeleteId"].ToString().Trim();
            string DeleteDuty = Request["DeleteDuty"].ToString().Trim();
            string DeleteCity = Request["DeleteCity"].ToString().Trim();

            UserModel User = new UserModel();
            ViewBag.Msg = User.Delete(DeleteId, DeleteDuty, DeleteCity);
            string City = Session["AuthenticateCity"].ToString().Trim();
            string Town = Session["AuthenticateTown"].ToString().Trim();
            ViewBag.Data = User.Search("ALL",City,Town,"ALL","ALL");
            ViewBag.City = City;
            ViewBag.Town = Town;
            return View("~/Views/Cleaner/C5x3.cshtml");
        }


        public ActionResult InputRegister()
        {

            Registers Register = new Registers();
            string City = Session["AuthenticateCity"].ToString().Trim();
            string Town = Session["AuthenticateTown"].ToString().Trim();
            string Id = Request["Id"];
            if (string.IsNullOrEmpty(Id))
            {
                ViewBag.Msg = "Id 不能空白";
                ViewBag.City = City;
                return View("~/Views/Cleaner/C5x3Register.cshtml");
            }
            string Name = Request["Name"];
            if (string.IsNullOrEmpty(Name))
            {
                ViewBag.Msg = "名字 不能空白";
                ViewBag.City = City;
                ViewBag.Town = Town;
                return View("~/Views/Cleaner/C5x3Register.cshtml");
            }
            string Pwd = Request["Pwd"];
            if (string.IsNullOrEmpty(Pwd))
            {
                ViewBag.Msg = "密碼不能空白";
                ViewBag.City = City;
                ViewBag.Town = Town;
                return View("~/Views/Cleaner/C5x3Register.cshtml");
            }
            string VoicePwd = Request["VoicePwd"];
            string Duty = "清潔隊";
            string HumanType = Request["HumanType"];
            string MobilePhone = Request["MobilePhone"];
            string MainContacter = Request["MainContacter"];
            string ReportPriority = Request["ReportPriority"];
            int PositionId = Convert.ToInt32(Request["PositionId"]);
            string ResponseMsg = Register.Add(Id, Name, Pwd, Duty, City, Town, MobilePhone, HumanType, MainContacter, ReportPriority, PositionId);
            ViewBag.Msg = ResponseMsg;
            ViewBag.City = City;
            return View("~/Views/Cleaner/C5x3Register.cshtml");
        }

        //聯絡人資料註冊
        public ActionResult Register()
        {
            ViewBag.City = Session["AuthenticateCity"].ToString().Trim();
            ViewBag.Town= Session["AuthenticateTown"].ToString().Trim();
            return View("~/Views/Cleaner/C5x3Register.cshtml");
        }

        public ActionResult Search()
        {
            string Duty = "ALL";
            string City = Session["AuthenticateCity"].ToString().Trim();
            string Town = Session["AuthenticateTown"].ToString().Trim();
            string SearchHumanType = Request["SearchHumanType"].ToString().Trim();
            string SearchMainContacter = Request["SearchMainContacter"].ToString().Trim();

            UserModel User = new UserModel();
            ViewBag.Data = User.Search(Duty, City, Town, SearchHumanType, SearchMainContacter);
            ViewBag.City = City;
            ViewBag.Town = Town;
            return View("~/Views/Cleaner/C5x2.cshtml");
        }
        public ActionResult Search2()
        {
            string Duty = "ALL";
            string City = Session["AuthenticateCity"].ToString().Trim();
            string Town = Session["AuthenticateTown"].ToString().Trim();
            string SearchHumanType = Request["SearchHumanType"].ToString().Trim();
            string SearchMainContacter = Request["SearchMainContacter"].ToString().Trim();
            UserModel User = new UserModel();
            ViewBag.Data = User.Search(Duty, City, Town, SearchHumanType, SearchMainContacter);
            ViewBag.City = City;
            ViewBag.Town = Town;
            return View("~/Views/Cleaner/C5x3.cshtml");
        }

        //要更新的欄位，內容不能為Null，否則會更新失敗
        public ActionResult Update()
        {
            //要更新的欄位，內容不能為Null，否則會更新失敗
            UserModel User = new UserModel();
            string Id = Request["EditingId"];
            string Name = Request["EditingName"];
            string Pwd = Request["EditingPwd"];
            string VoicePwd = Request["EditingVoicePwd"];
            string Duty = Request["EditingDuty"];
            string City = Request["EditingCity"];
            string Town = Request["EditingTown"];
            string MobilePhone = Request["EditingMobilePhone"];
            string HumanType = Request["EditingHumanType"];
            string MainContacter = Request["EditingMainContacter"];
            string ReportPriority = Request["EditingReportPriority"];
            int PositionId = Convert.ToInt32(Request["PositionId"]);
            string Msg = User.Update(Id, Name, Pwd, VoicePwd, Duty, City, Town, MobilePhone, HumanType, MainContacter, ReportPriority, PositionId);
            ViewBag.Msg = Msg;

            City = Session["AuthenticateCity"].ToString();
            Town = Session["AuthenticateTown"].ToString();
            ViewBag.Data = ViewBag.Data = User.Search("ALL", City, Town, "ALL", "ALL");
            ViewBag.City = City;
            ViewBag.Town = Town;
            return View("/Views/Cleaner/C5x3.cshtml");
        }
        public ActionResult Confirm(int? townId = null)
        {
            var user = GetUserBrief();

            UsersInfoFilterParameter filter = new UsersInfoFilterParameter
            {
                CityIds = user.CityId.ToListCollection()
            };

            switch (_Duty)
            {
                case DutyEnum.EPB:
                    {
                        if (townId.HasValue)
                        {
                            filter.TownIds = townId.Value.ToListCollection();
                        }
                        else
                        {
                            filter.TownIds = TownService.GetByCityId(user.CityId).Select(e => e.Id);
                        }
                        ViewBag.Towns = TownService.GetByCityId(user.CityId);
                        ViewBag.TownId = townId;
                    }
                    break;
                default:
                    {
                        filter.TownIds = user.TownId.ToListCollection();
                    }
                    break;
            }
            
            List<UsersModel> users = 
                UsersRepository.GetUsersInfoByFilter(filter)
                .Select(e => (UsersModel)e).ToList();

            foreach (var item in users)
            {
                item.ConfirmTime = DateTimeHelper.GetCurrentTime();
            }

            UsersRepository.Update(users);

            return RedirectToAction("C3x1Users", "Cleaner",new { townId });
        }
    }
}