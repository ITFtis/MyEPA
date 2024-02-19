using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Repositories;
using MyEPA.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class HomeController : BaseController
    {
        SMSLoginService _SMSLoginService = new SMSLoginService();
        UsersService _UsersService = new UsersService();
        public ActionResult Error()
        {
            ViewBag.Title = "系統暫時無法提供服務!!!";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Title = "聯絡資訊";
            return View();
        }

        public ActionResult Index()
        {
            Session["AuthenticateDuty"] = "Null";
            ViewBag.PostList = new NoticeService().GetAll();
            string x = "hello";
            x = x + "test";
            ViewBag.Title = "環境災害管理資訊系統";
            ViewBag.PostType = "notice";

            //要出現除錯訊息，請於VS2017，在偵測下方，把Release換成Debug，
            //然後點偵錯、視窗、輸出，以出現除錯視窗
            //最後，則點〉去執行程式

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Title = "登入首頁";
            return View(); 
        }

        public ActionResult LoginEmail()
        {           
            return View();
        }

        public ActionResult forget()
        {
            return View();
        }
        public ActionResult SMSLoginUserName()
        {
            ViewBag.Title = "簡訊登入-輸入帳號";
            return View();
        }

        [HttpPost]
        public ActionResult SMSLoginUserName(SMSLoginUserNameModel loginUser)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = _UsersService.GetByUserName(loginUser.UserName);

            if(user == null)
            {
                ModelState.AddModelError(nameof(SMSLoginUserNameModel.UserName), "此帳號不存在");
                return View();
            }

            ViewBag.Title = "簡訊登入-輸入帳號";
            TempData["SMSUser"] = loginUser;
            return RedirectToAction("SMSLogin", loginUser);
        }
        public ActionResult SMSLogin()
        {
            var user = (SMSLoginUserNameModel)TempData["SMSUser"];
            if (user == null || string.IsNullOrWhiteSpace(user.UserName))
            {
                return RedirectToAction("SMSLoginUserName");
            }
            ViewBag.Title = "簡訊登入";
            return View(user);
        }
        
        public ActionResult GetValidateCode(string key)
        {
            var vc = ValidateCodeHelper.GetValidateCode();
           
            if (CacheHelper.IsExist(key) == false)
            {
                CacheHelper.SetCacheValue(key, vc.Code, expiredTimeSeconds: 60);
            }

            return File(vc.Image, "image/jpeg");
        }

        public string GetValidateKey()
        {
            return Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 發送簡訊
        /// </summary>
        /// <param name="username"></param>
        [HttpPost]
        public ActionResult SendSMSVerify(SMSLoginUserNameModel loginUser)
        {
            AdminResultModel<object> result = new AdminResultModel<object>
            {
                IsSuccess = false,
                Result = null
            }; 
            
            if (loginUser.ValidateCode == null)
            {
                result.ErrorMessage = "驗證碼為必填項目";
                return JsonResult(result);
            }
            var code = CacheHelper.GetCacheValue<string>(loginUser.ValidateKey);
            if (code == null || code.Equals(loginUser.ValidateCode) == false)
            {
                result.ErrorMessage = "驗證碼錯誤";
                return JsonResult(result);
            }
            var user = _UsersService.GetByUserName(loginUser.UserName);
           
            if (user == null)
            {
                result.ErrorMessage = "查無此帳號";
                return JsonResult(result);
            }
            if (string.IsNullOrWhiteSpace(user.MobilePhone))
            {
                result.ErrorMessage = "此帳號無手機號碼";
                return JsonResult(result);
            }

            var sendSMSVerifyTimeKey = CacheKeyHelper.GetSendSMSVerifyTime(loginUser.UserName);
            int maxTime = 60;
            if (CacheHelper.IsExist(sendSMSVerifyTimeKey) == false)
            {
                CacheHelper.SetCacheValue(sendSMSVerifyTimeKey, DateTimeHelper.GetCurrentTime(), maxTime);
            }
            else
            {
                DateTime time = CacheHelper.GetCacheValue<DateTime>(sendSMSVerifyTimeKey);
                int second = 60 - (int)(DateTimeHelper.GetCurrentTime() - time).TotalSeconds;
                if(second > 0)
                {
                    result.Result = new 
                    {
                        Second = second
                    };
                    return JsonResult(result);
                }
            }

            var sendSMSVerifyCountByDate = CacheKeyHelper.GetSendSMSVerifyCountByDate(loginUser.UserName);
            //有效期限直接設定每日
            var count = CacheHelper.Increment(sendSMSVerifyCountByDate, 60 * 60 * 24);

            var smsVerifyMaxCount = SettingHelper.SMSVerifyMaxCount;
            if (count > smsVerifyMaxCount)
            {
                result.ErrorMessage = $"很抱歉，您已達今日發送次數上限({smsVerifyMaxCount}次)";
                return JsonResult(result);
            }
            string smsLoginKey = CacheKeyHelper.GetSMSLogin(loginUser.UserName);

            string verifyCode = RandomHelper.GetRandNumbers(length: 6);

            int smsVerifyTimeLimitMinutes = SettingHelper.SMSVerifyTimeLimitMinutes;

            new SMSHttp().SendSMSNow("[簡訊驗證碼]", $"[登入驗證] 您的登入驗證碼為:{verifyCode}，限 {smsVerifyTimeLimitMinutes} 分鐘內有效。請勿將此驗證碼提供給其他人", user.MobilePhone);
            //minutes
            //15分鐘有效期限
            CacheHelper.SetCacheValue(smsLoginKey, verifyCode, smsVerifyTimeLimitMinutes * 60);
            
            result.IsSuccess = true;
            return JsonResult(result);
        }
        [HttpPost]
        public ActionResult SMSLogin(SMSVerifyLoginModel smsVerifyLogin)
        {
            AdminResultModel result = new AdminResultModel
            {
                IsSuccess = false
            };

            result = _SMSLoginService.SMSVerify(smsVerifyLogin);

            if(result.IsSuccess == false)
            {
                return JsonResult(result);
            }

            var user = _UsersService.GetByUserName(smsVerifyLogin.UserName);

            if (user == null)
            {
                result.ErrorMessage = "帳號不存在，系統暫停3秒，請回上一步重新輸入";
                System.Threading.Thread.Sleep(3000);
                return JsonResult(result);
            }

            switch (smsVerifyLogin.Type)
            {
                case SystemTypeEnum.ContactManual:
                    user.DutyId = DutyEnum.ContactManual.ToInteger();
                    break;
                case SystemTypeEnum.EPACar:
                    user.DutyId = DutyEnum.EPACar.ToInteger();
                    break;
            }
            _UsersService.AddUserLoginLog(user);//寫登入log
            SetSession(user);

            result.IsSuccess = true;

            return JsonResult(result);
        }
        [HttpPost]
        public ActionResult Login(string username, string pwd, SystemTypeEnum type)
        {
            var user = _UsersService.GetUserByUserNameAndPwd(username, pwd);

            if (user == null)
            {
                Session["AuthenticateDuty"] = "Null";
                Session["Pwd"] = pwd;
                ViewBag.Msg = "抱歉，帳號或密碼不正確，系統暫停3秒，請重新輸入";
                System.Threading.Thread.Sleep(3000);
                return View("~/Views/Home/Login.cshtml");
            }

            if (user.Email.IsEmptyOrNull() || user.Email== "NULL")
            {
                ViewBag.username = username;
                ViewBag.pwd = pwd;
                ViewBag.type = type;
                return View("~/Views/Home/LoginEmail.cshtml");
            }
            else
            {
                _UsersService.AddUserLoginLog(user);//寫登入log
                return Login(user, type);
            }
        }
        [HttpPost]
        public ActionResult LoginEmail(string username, string pwd, SystemTypeEnum type, string email)
        {
            var user = _UsersService.GetUserByUserNameAndPwd(username, pwd);

            if (user == null)
            {
                Session["AuthenticateDuty"] = "Null";
                Session["Pwd"] = pwd;
                ViewBag.Msg = "抱歉，帳號或密碼不正確，系統暫停3秒，請重新輸入";
                System.Threading.Thread.Sleep(3000);
                return View("~/Views/Home/Login.cshtml");
            }
            if (!email.Contains("@"))
            {
                Session["AuthenticateDuty"] = "Null";
                Session["Pwd"] = pwd;
                ViewBag.Msg = "抱歉，請輸入正確的Email";
                System.Threading.Thread.Sleep(3000);
                return View("~/Views/Home/Login.cshtml");

            }

            _UsersService.AddUserEmail(user, email);//寫登入log        
            _UsersService.AddUserLoginLog(user);//寫登入log          
            return Login(user, type);

        }



        [HttpPost]
        public ActionResult forget(string username, string email)
        {
            var user = _UsersService.GetUserByUserNameAndemail(username, email);

            if (user == null)
            {               
                ViewBag.Msg = "抱歉，帳號或email不正確，系統暫停3秒，請重新輸入";
                System.Threading.Thread.Sleep(3000);
                return View("~/Views/Home/forget.cshtml");
            }
            else
            {
                ViewBag.PWD = user.Pwd;
            }
            return View();

        }

        public ActionResult LoginRedirect()
        {
            var user = GetUserBrief();
            var duty = user.Duty;
            switch (duty)
            {
                case DutyEnum.Water:
                case DutyEnum.EPB:
                case DutyEnum.EPA:
                case DutyEnum.Cleaning:
                    {
                        return RedirectToAction("Index", $"{duty.ToString()}Member", new { });
                    }
                case DutyEnum.Team:
                    {
                        Session["Area"] = new UserAreaRepository().Get(user.UserId)?.AreaId;
                        return RedirectToAction("Index", "EPAMember", new { });
                    }
                case DutyEnum.Corps:
                    {
                        return RedirectToAction("Index", "EPAMember", new { });
                    }
                case DutyEnum.EPACar:
                    {
                        string url = SettingHelper.CarSystemUrl;
                        return Redirect($"{url}?UserName={user.UserName}");
                    }
                case DutyEnum.ContactManual:
                    {
                        return RedirectToAction("Index", "ContactManual", new { });
                    }
                default:
                    return View("~/Views/Home/Login.cshtml");
            }
        }
 
       




        private ActionResult Login(UsersModel user, SystemTypeEnum type)
        {
            DutyEnum duty = (DutyEnum)user.DutyId;

            string dutyName = duty.GetDescription();
            ViewBag.City = user.City;
            ViewBag.Town = user.Town;
            ViewBag.Duty = dutyName;
            ViewBag.Msg = $"歡迎{dutyName}進入系統";

            switch (type)
            {
                case SystemTypeEnum.ContactManual:
                    user.DutyId = DutyEnum.ContactManual.ToInteger();
                    break;
                case SystemTypeEnum.EPACar:
                    user.DutyId = DutyEnum.EPACar.ToInteger();
                    break;
            }
           
            SetSession(user);
            return LoginRedirect();
        }

        private void SetSession(UsersModel user)
        {
            DutyEnum duty = (DutyEnum)user.DutyId;
            string dutyName = duty.GetDescription();
            Session["IsAdmin"] = user.IsAdmin;
            Session["UserId"] = user.Id;
            Session["AuthenticateId"] = user.UserName;
            Session["Name"] = user.Name;
            Session["AuthenticateCity"] = user.City;
            Session["AuthenticateTown"] = user.Town;
            Session["AuthenticateCityId"] = user.CityId;
            Session["AuthenticateTownId"] = user.TownId;
            Session["DutyId"] = user.DutyId;
            Session["AuthenticateDuty"] = dutyName;
            Session["ContactManualDuty"] = user.ContactManualDuty.ToInteger();
            Session["ContactManualDepartmentId"] = user.ContactManualDepartmentId;
            if (user.ContactManualDepartmentId.HasValue)
            {
                Session["ContactManualDepartment"] = new ContactManualDepartmentService().Get(user.ContactManualDepartmentId.Value)?.Name;
            }
        }

        //以下程式先註解，證實無用後刪除
        //[HttpPost]
        //public ActionResult Test(string par1)
        //{
        //    ViewBag.Msg = par1;
        //    return View();
        //}


        public ActionResult News()
        {
            ViewBag.PostList = new NewsService().GetAll().OrderByDescending(e => e.CreateDate).ToList();
            ViewBag.Title = "新聞報導";
            ViewBag.PostType = "news";
            return View();
        }

        public ActionResult Notice() 
        { 
            ViewBag.PostList =  new NoticeService().GetAll().OrderByDescending(e => e.CreateDate).ToList();
            ViewBag.Title = "最新公告";
            ViewBag.PostType = "notice";
            return View();
        }

        public ActionResult Operation()
        {
            ViewBag.Title = "操作說明書";
            Response.Redirect("~/HTML5_20201216/Emis_edu/index.html");
            return View();
        }

        public ActionResult Opinion()
        {
            return View();
        }

        public ActionResult Resource()
        {
            ViewBag.Title = "其它資源";
            return View();
        }

        public ActionResult SiteMap()
        {
            ViewBag.Title = "網站導覽";
            return View(); 
        }

        public ActionResult Register()
        {
            ViewBag.Positions = new PositionService().GetAll();
            ViewBag.Msg = TempData["Msg"];
            return View();
        }

    }
}