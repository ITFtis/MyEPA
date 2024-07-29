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
    public class EPAxUserController : LoginBaseController
    { 
        UserAreaRepository UserAreaRepository = new UserAreaRepository();
        UsersRepository UsersRepository = new UsersRepository();
        //修改網路與語音密碼
        public ActionResult A8x1()
        {
            ViewBag.UserId = Session["AuthenticateId"];
            ViewBag.City = Session["AuthenticateCity"].ToString();
            return View("~/Views/EPA/A8x1.cshtml");
        }

        //聯絡人資料查詢
        public ActionResult A8x2()
        {
            var data = new UsersService().GetAll();
            data = GetUserByDuty(data);
            ViewBag.Data = data;
            ViewBag.Positions = new PositionService().GetAll();
            ViewBag.Towns = new TownService().GetByCityId(GetUserBrief().CityId);
            return View("~/Views/EPA/A8x2.cshtml");
        }

        //聯絡人資料維護
        public ActionResult A8x3()
        {
            var data = new UsersService().GetAll();
            data = GetUserByDuty(data);
            foreach (var item in data)
            {
                if(item.Duty == DutyEnum.Team.GetDescription() || item.Duty == DutyEnum.Corps.GetDescription())
                {
                    item.Duty = DutyEnum.EPA.GetDescription();
                }
            }
            ViewBag.Msg = TempData["Msg"];
            ViewBag.Data = data;
            ViewBag.Positions = new PositionService().GetAll();
            return View("~/Views/EPA/A8x3.cshtml");
        }

        //以下會進入A9x12ApproveRegister.cshtml
        //該網頁連結 @model IEnumerable<MyEPA.Models.Registers>
        //因此會從Entity Framework資料庫調出Register的表單，並且逐筆顯現

        //以下會在顯示註冊者的GridView時，接受環保署審核人員點選某筆的同意或取消鍵
        //並傳回其ID（以RegisterId, RejectId顯示），並檢驗兩者的值是否為空，
        //（空的審查者沒點選該鍵，然後依同意或取消做不同的事

        public ActionResult A9x12ApproveRegister()
        {
            ViewBag.msg = TempData["msg"];
            return View("~/Views/EPA/A9x12ApproveRegister.cshtml",new RegistersRepository().GetList().ToList());
        }

        [HttpPost]
        public ActionResult Approve(string RegisterId, string RejectId)
        {
            string msg = string.Empty;
            RegistersModel register = null;
            if (!string.IsNullOrEmpty(RegisterId))
            {
                var registersRepository = new RegistersRepository();
                register = registersRepository.Get(RegisterId);
            }

            if (register == null)
            {
                msg = "查無資料";
                if (!string.IsNullOrEmpty(RejectId))
                {
                    Registers Register = new Registers();
                    Register.Remove(RejectId);
                    msg = "申請者資料已退回";
                }
            }
            else 
            {
                //同意註冊者之後，從Register調出他的所有資料，包含RegisterName,RegisterPwd, RegisterDuty, ...等等
                //然後將該註冊者從Register名單移出，並且利用UserModel的Add方法，
                //將該註冊者加入到User名單中

                int departmentId = 0;
                DutyEnum duty = (DutyEnum)register.DutyId.Value;
                AreaEnum? areaEnum = null;
                if (duty == DutyEnum.EPA)
                {
                    if(register.Town.Contains("環境督察大隊"))
                    {
                        areaEnum = register.Town.GetValueFromDescription<AreaEnum>();

                        duty = DutyEnum.Team;                        
                    }
                    else if(register.Town.Equals("環境督察總隊"))
                    {
                        duty = DutyEnum.Corps;
                    }
                    List<DepartmentModel> departments = new DepartmentRepository().GetList();

                    DepartmentModel department = 
                        departments.FirstOrDefault(e => e.Name == register.Town);
                    
                    if (department == null)
                    {
                        department = departments.FirstOrDefault(e => e.Name == "中央");
                    }

                    departmentId = department.Id;
                }
                if (register.MainContacter == "是" && UsersRepository.IsExistsByMainContacter(register.CityId.Value, register.TownId.Value))
                {
                    msg = "新增失敗，主要負責人已存在，不可重覆設定";

                    TempData["msg"] = msg;

                    return RedirectToAction("A9x12ApproveRegister", "EPAxUser", new { });
                }
                var user = GetUserBrief();
                int userId = UsersRepository.CreateAndResultIdentity<int>(new UsersModel 
                {
                    City = register.City,
                    IsAdmin = false,
                    CityId = register.CityId,                    
                    DepartmentId = departmentId,
                    Duty = duty.GetDescription(),
                    DutyId = duty.ToInteger(),
                    Email = register.Email,
                    UserName = RegisterId,
                    Name = register.Name,
                    Pwd = register.Pwd,
                    PwdUpdateDate = DateTime.Now.AddDays(90),
                    VoicePwd = register.VoicePwd,
                    MobilePhone = register.MobilePhone,
                    Town = register.Town,
                    TownId = register.TownId,
                    FaxNumber = string.Empty,
                    HomeNumber = string.Empty,
                    HumanType = register.HumanType,
                    MainContacter = register.MainContacter,
                    OfficePhone = register.OfficePhone,
                    PositionId = register.PositionId,
                    Remark = string.Empty,
                    ContactManualDuty= ContactManualDutyEnum.User,
                    ReportPriority = register.ReportPriority,
                    UpdateDate = DateTimeHelper.GetCurrentTime(),
                    UpdateUser = user.UserName,
                    ConfirmTime = DateTimeHelper.GetCurrentTime(),
                });
                if (areaEnum.HasValue)
                {
                    UserAreaRepository.Create(new UserAreaModel
                    {
                        AreaId = areaEnum.Value.ToInteger(),
                        UserId = userId
                    });
                }
                msg = new Registers().Remove(RegisterId);
                msg += "\r\n審核成功";
            }

            TempData["msg"] = msg;

            return RedirectToAction("A9x12ApproveRegister", "EPAxUser", new {  });
        }

        //聯絡人資料註冊
        public ActionResult Register()
        {
            ViewBag.Positions = new PositionService().GetAll();
            
            ViewBag.Msg = TempData["Msg"];

            return View("~/Views/EPA/A8x3Register.cshtml");
        }

        //更改網頁密碼
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
            return View("~/Views/EPA/A8x1.cshtml");
        }

        //更改語音密碼
        public ActionResult ChangeVoicePwd()
        {
            UserModel User = new UserModel();
            string Id = Session["AuthenticateId"].ToString().Trim();

            string NewVoicePwd = Request["NewVoicePwd"].Trim();
            ViewBag.Msg = User.ChangeVoicePwd(Id, NewVoicePwd);
            ViewBag.UserId = Id;
            return View("~/Views/EPA/A8x1.cshtml");      
        }
        private string AllReplaceEmpty(string input)
        {
            if(string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }
            return input.Replace("ALL", string.Empty);
        }
            
        public ActionResult Search(string SearchDuty,string SearchCity,string SearchTown,string SearchHumanType,string SearchMainContacter,string SearchISEnvironmentalProtectionAdministration, string SearchISEnvironmentalProtectionDepartment,string SearchISBook)
        { 
            SearchDuty = AllReplaceEmpty(SearchDuty);
            SearchCity = AllReplaceEmpty(SearchCity);
            SearchTown = AllReplaceEmpty(SearchTown);
            SearchHumanType = AllReplaceEmpty(SearchHumanType);
            SearchMainContacter = AllReplaceEmpty(SearchMainContacter);
            SearchISEnvironmentalProtectionAdministration = AllReplaceEmpty(SearchISEnvironmentalProtectionAdministration);
            SearchISEnvironmentalProtectionDepartment = AllReplaceEmpty(SearchISEnvironmentalProtectionDepartment);
            SearchISBook = AllReplaceEmpty(SearchISBook);

            List<int> cityIds = new List<int>();
            List<int> townIds = new List<int>();
            if (string.IsNullOrWhiteSpace(SearchCity) == false)
            {
                var city = new CityService().GetByCityName(SearchCity);
                if (city != null)
                {
                    cityIds = city.Id.ToListCollection();
                    if (string.IsNullOrWhiteSpace(SearchTown) == false)
                    {
                        var town = new TownService().GetByFilter(new TownFilterParameter
                        {
                            CityIds = city.Id.ToListCollection(),
                            TownName = SearchTown
                        });
                        if (town != null)
                        {
                            townIds = town.Id.ToListCollection();
                        }
                    }
                }
            }
            
            List<int> dutyIds = new List<int>();
            if(string.IsNullOrWhiteSpace(SearchDuty) == false)
            {
                DutyEnum duty = SearchDuty.GetValueFromDescription<DutyEnum>();
                switch (duty)
                {
                    case DutyEnum.EPA:
                        dutyIds.Add(duty.ToInteger());
                        dutyIds.Add(DutyEnum.Team.ToInteger());
                        dutyIds.Add(DutyEnum.Corps.ToInteger());
                        break;
                    default:
                        dutyIds.Add(duty.ToInteger());
                        break;
                }
            }
            
            var data = new UsersService().GetUsersByFilter(new UsersBriefFilterParameter
            {
                CityIds = cityIds,
                TownIds = townIds,
                DutyIds = dutyIds,
                HumanType = SearchHumanType,
                MainContacter = SearchMainContacter,
              ISEnvironmentalProtectionAdministration = SearchISEnvironmentalProtectionAdministration,
                      ISEnvironmentalProtectionDepartment = SearchISEnvironmentalProtectionDepartment,
                ISBook= SearchISBook
            });

            data = GetUserByDuty(data);
            ViewBag.Data = data;
            ViewBag.Positions = new PositionService().GetAll();
            ViewBag.City = Session["AuthenticateCity"].ToString();
            return View("~/Views/EPA/A8x2.cshtml");
        }
        private List<UsersModel> GetUserByDuty(List<UsersModel> data)
        {
            var userBrief = GetUserBrief();
            if (userBrief.Duty == DutyEnum.EPB)
            {
                data = data.Where(e => e.CityId == userBrief.CityId)
                    .Where(e => e.DutyId == userBrief.Duty.ToInteger() || e.DutyId == DutyEnum.Cleaning.ToInteger()).ToList();
            }
            else if (userBrief.Duty == DutyEnum.Cleaning)
            {
                data = data.Where(e => e.CityId == userBrief.CityId && e.TownId == userBrief.TownId && e.DutyId == userBrief.Duty.ToInteger()).ToList();
            }
            else if (userBrief.Duty == DutyEnum.Water)
            {
                data = data.Where(e => e.DutyId == userBrief.Duty.ToInteger()).ToList();
            }
            return data;
        }
        public ActionResult Search2(string SearchDuty, string SearchCity, string SearchTown, string SearchHumanType, string SearchMainContacter, string SearchISEnvironmentalProtectionAdministration, string SearchISEnvironmentalProtectionDepartment, string SearchISBook)
        {
            SearchDuty = AllReplaceEmpty(SearchDuty);
            SearchCity = AllReplaceEmpty(SearchCity);
            SearchTown = AllReplaceEmpty(SearchTown);
            SearchHumanType = AllReplaceEmpty(SearchHumanType);
            SearchMainContacter = AllReplaceEmpty(SearchMainContacter);
            SearchISEnvironmentalProtectionAdministration = AllReplaceEmpty(SearchISEnvironmentalProtectionAdministration);
            SearchISEnvironmentalProtectionDepartment = AllReplaceEmpty(SearchISEnvironmentalProtectionDepartment);
            SearchISBook = AllReplaceEmpty(SearchISBook);

            List<int> cityIds = new List<int>();
            List<int> townIds = new List<int>();
            if (string.IsNullOrWhiteSpace(SearchCity) == false)
            {
                var city = new CityService().GetByCityName(SearchCity);
                if(city != null)
                {
                    cityIds = city.Id.ToListCollection();
                    if (string.IsNullOrWhiteSpace(SearchTown) == false)
                    {
                        var town = new TownService().GetByFilter(new TownFilterParameter
                        {
                            CityIds = city.Id.ToListCollection(),
                            TownName = SearchTown
                        });
                        if (town != null)
                        {
                            townIds = town.Id.ToListCollection();
                        }
                    }
                }
            }

            List<int> dutyIds = new List<int>();
            if (string.IsNullOrWhiteSpace(SearchDuty) == false)
            {
                DutyEnum duty = SearchDuty.GetValueFromDescription<DutyEnum>();
                switch (duty)
                {
                    case DutyEnum.EPA:
                        dutyIds.Add(duty.ToInteger());
                        dutyIds.Add(DutyEnum.Team.ToInteger());
                        dutyIds.Add(DutyEnum.Corps.ToInteger());
                        break;
                    default:
                        dutyIds.Add(duty.ToInteger());
                        break;
                }
            }

            var data = new UsersService().GetUsersByFilter(new UsersBriefFilterParameter
            {
                CityIds = cityIds,
                TownIds = townIds,
                DutyIds = dutyIds,
                HumanType = SearchHumanType,
                MainContacter = SearchMainContacter,
                  ISEnvironmentalProtectionAdministration = SearchISEnvironmentalProtectionAdministration,
                ISEnvironmentalProtectionDepartment = SearchISEnvironmentalProtectionDepartment,
                ISBook = SearchISBook
            });
            data = GetUserByDuty(data);
            ViewBag.Data = data;
            ViewBag.Positions = new PositionService().GetAll();
            ViewBag.City = Session["AuthenticateCity"].ToString();
            return View("~/Views/EPA/A8x3.cshtml");
        }

        public ActionResult Delete()
        {
            string DeleteId= Request["DeleteId"].ToString();
            string DeleteDuty= Request["DeleteDuty"].ToString().Trim();
            string DeleteCity = Request["DeleteCity"].ToString().Trim();

            UserModel User = new UserModel();
            ViewBag.Msg= User.Delete(DeleteId,DeleteDuty,DeleteCity);

            return RedirectToAction("A8x3", "EPAxUser");
        }
        private string GetRequest(string key)
        {
            return ((string)Request[key]).Trim();
        }
        [HttpPost]
        public ActionResult Update(UsersEditViewModel model)
        {
            var foundDuty = new DutyRepository().GetByDutyName(model.EditingDuty);

            if(foundDuty == null)
            {
                ViewBag.Msg = "查無此機關類別(角色)";
                return RedirectToAction("A8x3", "EPAxUser");
            }
            var user = UsersRepository.GetUserByUserName(model.EditingId);
            if (user == null)
            {
                ViewBag.Msg = "查無此人員";
                return RedirectToAction("A8x3", "EPAxUser");
            }

            if (!PwdHelper.ValidPassword(model.EditingPwd))
            {
                TempData["Msg"] = PwdHelper.ErrorMessage;                
                return RedirectToAction("A8x3", "EPAxUser");
            }

            int departmentId = 0;
            DutyEnum duty = (DutyEnum)foundDuty.Id;
            AreaEnum? areaEnum = null;

            int? cityId = new CityRepository().GetByCityName(model.EditingCity)?.Id;

            if (cityId.HasValue == false)
            {
                TempData["Msg"] = $"修改失敗 {model.EditingCity} 不存在";
                return RedirectToAction("A8x3", "EPAxUser");
            }

            int? townId = new TownRepository().GetByTownName(cityId.Value, model.EditingTown)?.Id;
            if (townId.HasValue == false)
            {
                TempData["Msg"] = $"修改失敗 {model.EditingTown} 不存在";
                return RedirectToAction("A8x3", "EPAxUser");
            }
            
            if(model.EditingMainContacter == "是")
            {
                //主聯絡人 一個縣市 & 一個鄉鎮只能有一個
                if (UsersRepository.IsExistsByMainContacter(cityId.GetValueOrDefault(), townId.GetValueOrDefault(), user.Id))
                {
                    TempData["Msg"] = $"修改失敗，主要負責人已存在，不可重覆設定";
                    return RedirectToAction("A8x3", "EPAxUser");
                }
            }

            var u = GetUserBrief();
            switch (duty)
            {
                case DutyEnum.EPA:
                    if (model.EditingTown.Contains("環境督察大隊"))
                    {
                        areaEnum = model.EditingTown.GetValueFromDescription<AreaEnum>();

                        duty = DutyEnum.Team;
                    }
                    else if (model.EditingTown.Equals("環境督察總隊"))
                    {
                        duty = DutyEnum.Corps;
                    }
                    List<DepartmentModel> departments = new DepartmentRepository().GetList();

                    DepartmentModel department =
                        departments.FirstOrDefault(e => e.Name == model.EditingTown);

                    if (department == null)
                    {
                        department = departments.FirstOrDefault(e => e.Name == "中央");
                    }

                    departmentId = department.Id;
                    break;
                default:
                    

                    break;

            }
            user.Town = model.EditingTown;
            user.Duty = duty.GetDescription();
            user.DutyId = duty.ToInteger();
           
            user.CityId = cityId;
            user.Name = model.EditingName;
            user.Pwd = model.EditingPwd;
            user.PwdUpdateDate = DateTime.Now.AddDays(90);
            user.VoicePwd = model.EditingVoicePwd;
            user.MobilePhone = model.EditingMobilePhone;
            user.OfficePhone = model.EditingOfficePhone;
            user.Email = model.EditingEmail;
            user.HumanType = string.Join("、", model.EditingHumanType ?? new List<string>());
            user.MainContacter = model.EditingMainContacter;
            user.ReportPriority = model.EditingReportPriority;
            user.DepartmentId = departmentId;
            user.PositionId = model.EditingPositionId;
            user.ISEnvironmentalProtectionAdministration = model.SearchISEnvironmentalProtectionAdministration;
            user.ISEnvironmentalProtectionDepartment = model.SearchISEnvironmentalProtectionDepartment;
            user.ISBook = model.SearchISBook;
            user.UpdateDate = DateTimeHelper.GetCurrentTime();
            user.ConfirmTime = user.UpdateDate;
            user.UpdateUser = u.UserName;
            if (user.CityId.HasValue)
            {
                user.TownId = new TownRepository().GetByTownName(user.CityId.Value,model.EditingTown)?.Id;
            }

            UsersRepository.Update(user);

            //如果有權限先刪除
            if (UserAreaRepository.IsExistsByUserId(user.Id))
            {
                UserAreaRepository.Delete(user.Id);
            }
            //新增區域大隊權限
            if (areaEnum.HasValue)
            {
                UserAreaRepository.Create(new UserAreaModel
                {
                    AreaId = areaEnum.Value.ToInteger(),
                    UserId = user.Id
                });
            }

            ViewBag.Msg = "修改成功";
            return RedirectToAction("A8x3", "EPAxUser");
        }

        public ActionResult InputRegister(RegistersModel model, List<string> humanType)
        {
            if (!PwdHelper.ValidPassword(model.Pwd))
            {
                TempData["Msg"] = PwdHelper.ErrorMessage;
                return RedirectToAction("Register", "EPAxUser", new { });
            }
            AdminResultModel result = new RegisterService().Register(model, humanType);

            TempData["Msg"] = result.ErrorMessage;

            return RedirectToAction("Register", "EPAxUser", new { });
        }
    }
}