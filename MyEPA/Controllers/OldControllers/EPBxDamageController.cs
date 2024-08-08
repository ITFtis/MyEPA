using System;
using System.Collections.Generic;
using System.IO;
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
    public class EPBxDamageController : LoginBaseController
    {
        DamageRepository DamageRepository = new DamageRepository { };
        DiasterRepository DiasterRepository = new DiasterRepository();
        TownRepository TownRepository = new TownRepository();
        FileDataService FileDataService = new FileDataService();
        public ActionResult B1c1()
        {
            ViewBag.City = GetUserCity();
            return View("~/Views/EPB/B1c1.cshtml");
        }
        public ActionResult B1c(int? diasterId = null)
        {
            List<DiasterModel> diasters = DiasterRepository.GetList().OrderByDescending(e => e.Id).ToList();
            DiasterModel diaster = null;

            if(diasterId.HasValue)
            {
                diaster = diasters.FirstOrDefault(e => e.Id == diasterId.Value);
                if(diaster == null)
                {
                    diasterId = null;
                }
            }

            if (diasterId.HasValue == false)
            {
                diaster = diasters.FirstOrDefault(e => e.Status == NormalActiveStatusEnum.Active.ToInteger());
                if(diaster != null)
                {
                    diasterId = diaster.Id;
                }
            }

            if (diasterId.HasValue == false)
            {
                diaster = diasters.FirstOrDefault();
                if (diasters != null)
                {
                    diasterId = diaster.Id;
                }
            }
            
            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;
            if(diasterId.HasValue)
            {
                ViewBag.DamageMain =
                new DamageMainRepository().GetByFilter(new DamageMainFilterParameter
                {
                    CityIds = GetUserCityId().ToListCollection(),
                    DiasterIds = diasterId.Value.ToListCollection()
                });
            }
            
            ViewBag.Msg = TempData["Msg"];

            var user = GetUserBrief();
            int townId = 0;
            if (user.Duty == DutyEnum.EPB)
            {
                //環保局
                townId = 0;
            }
            else if (user.Duty == DutyEnum.Cleaning)
            {
                //清潔隊
                townId = user.TownId;
            }

            ViewBag.ActiveDays = GetActiveDays(diaster.StartTime, diaster.EndTime);

            DamageFilterParameter filterParameter = new DamageFilterParameter()
            {
                CityIds = user.CityId.ToListCollection(),
                DiasterIds = diaster.Id.ToListCollection(),
                TownId = townId,
            };

            if (user.Duty == DutyEnum.Cleaning)
            {
                filterParameter.TownIds = user.TownId.ToListCollection();
            }

            ViewBag.Data2 =
                DamageRepository.GetListByFilter(filterParameter)
                .OrderBy(a => a.ReportDay)
                .ToList();

            ViewBag.Diaster = diaster;

            return View("~/Views/EPB/B1c.cshtml");
        }
        public ActionResult B1c17(DateTime? reportDay, int? diasterId = null)
        {
            var user = GetUserBrief();
            ViewBag.Msg = TempData["Msg"];
            ViewBag.Towns = new TownRepository().GetByCityId(user.CityId);
            ViewBag.ReportDay = reportDay;
            ViewBag.User = user;
            DiasterModel diaster = null;
            if (diasterId.HasValue == false)
            {
                diaster = DiasterRepository.GetByRuning();
            }
            else
            {
                diaster = DiasterRepository.Get(diasterId);
            }

            ViewBag.Landfills = new LandfillRepository().GetByFilter(new LandfillFilterParameter
            {
                City = user.City
            }).Select(e => new SelectListItem { Text = e.ContactUnit, Value = e.Id.ToString() }).ToList();

            ViewBag.Incinerators = new IncineratorRepository().GetByFilter(new IncineratorFilterParameter
            {
                City = user.City
            }).Select(e => new SelectListItem { Text = e.ContactUnit, Value = e.Id.ToString() }).ToList();

            ViewBag.Diaster = diaster;

            if (reportDay.HasValue == false)
            {
                TempData["Msg"] = "請先選擇通報日期";
                ViewBag.ActiveDays = GetActiveDays(diaster.StartTime, diaster.EndTime);

                return RedirectToAction("B1c", "EPBxDamage");

            }
            else
            {
                MapModel Place = new MapModel();
                Place = Place.FindGPS(user.City);
                ViewBag.Xpos = Place.Xpos;
                ViewBag.Ypos = Place.Ypos;

                DamageFilterParameter filterParameter = new DamageFilterParameter()
                {
                    CityIds = user.CityId.ToListCollection(),
                    DiasterIds = diaster.Id.ToListCollection(),
                    ReportDay = reportDay,
                };

                if(user.Duty == DutyEnum.Cleaning)
                {
                    filterParameter.TownIds = user.TownId.ToListCollection();
                }

                ViewBag.Data2 = DamageRepository.GetListByFilter(filterParameter);
               
                return View("~/Views/EPB/B1c17.cshtml");
            }
        }
        private List<DateTime> GetActiveDays(DateTime start,DateTime end)
        {
            List<DateTime> activeDays = new List<DateTime>();
            for (DateTime date = start; date < end; date = date.AddDays(1))
            {
                activeDays.Add(date);
            }
            return activeDays;
        }

        private DiasterModel GetDiaster(List<DiasterModel> diasters,int? diasterId)
        {
            var diaster = new DiasterModel();
            if (diasterId.HasValue)
            {
                diaster = diasters.FirstOrDefault(e => e.Id == diasterId.Value);
                if (diaster == null)
                {
                    diasterId = null;
                }
            }

            if (diasterId.HasValue == false)
            {
                diaster = diasters.FirstOrDefault(e => e.Status == NormalActiveStatusEnum.Active.ToInteger());
                if (diaster != null)
                {
                    diasterId = diaster.Id;
                }
            }

            if (diasterId.HasValue == false)
            {
                diaster = diasters.FirstOrDefault();
                if (diasters != null)
                {
                    diasterId = diaster.Id;
                }
            }
            return diaster;
        }
        public ActionResult B1c18(int? diasterId = null, DateTime? reportDay = null)
        {
            List<DiasterModel> diasters = DiasterRepository.GetList().OrderByDescending(e => e.Id).ToList();
          
            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;

            DiasterModel diaster = GetDiaster(diasters, diasterId);

            reportDay = reportDay.HasValue ? reportDay.Value : diaster.StartTime;

            var user = GetUserBrief();

            ViewBag.City = user.City;
            ViewBag.DiasterId = diaster.Id;
            ViewBag.DiasterName = diaster.DiasterName;
            ViewBag.StartTime = diaster.StartTime;
            ViewBag.EndTime = diaster.EndTime;
            ViewBag.ReportDay = reportDay;
            ViewBag.ActiveDays = GetActiveDays(diaster.StartTime, diaster.EndTime);
            ViewBag.Data2 = new DamageService().GetConfirmList(new DamageConfirmListFilterParameter
            {
                DamageFilterParameter = new DamageFilterParameter
                {
                    CityIds = user.CityId.ToListCollection(),
                    DiasterIds = diaster.Id.ToListCollection(),
                    ReportDay = reportDay
                },
                Diaster = diaster,
            }).OrderBy(e => e.ReportDay).ThenBy(e => e.TownId);

            return View("~/Views/EPB/B1c18.cshtml");
        }
        [HttpPost]
        public ActionResult Report(DamageModel model,List<int> incineratorIds,List<int> landfillIds)
        {
            if(incineratorIds == null)
            {
                incineratorIds = new List<int>();
            }
            if (landfillIds == null)
            {
                landfillIds = new List<int>();
            }
            var user = GetUserBrief();

            model.CityId = user.CityId;

            if (user.Duty == DutyEnum.EPB)
            {
                //環保局
                model.TownId = 0;
            }

            DamageJoinModel damage = DamageRepository.GetListByFilter(new DamageFilterParameter
            {
                CityIds = model.CityId.ToListCollection(),
                DiasterIds = model.DiasterId.ToListCollection(),
                ReportDay = model.ReportDay,
                TownId = model.TownId,
            }).FirstOrDefault();

            if (damage == null)
            {
                model.CreateDate = DateTimeHelper.GetCurrentTime();
                model.UpdateDate = DateTimeHelper.GetCurrentTime();
                model.DutyId = user.Duty;
                model.Status = DamageStatusEnum.Waiting;
                model.IncineratorIds = string.Join(",", incineratorIds);
                model.LandfillIds = string.Join(",", landfillIds);
                model.IsDamage = true;
                DamageRepository.Create(model);
                
            }
            else
            {
                damage.DamagePlace = model.DamagePlace;
                damage.DamageArea = model.DamageArea;
                damage.FloodArea = model.FloodArea;
                damage.DumpSiteDesc = model.DumpSiteDesc;
                damage.IncinerationPlantDesc = model.IncinerationPlantDesc;
                damage.Other = model.Other;
                damage.DisinfectDate = model.DisinfectDate;
                damage.DisinfectArea = model.DisinfectArea;
                damage.PR_Garbage = model.PR_Garbage;
                damage.IncineratorIds = string.Join(",", incineratorIds);
                damage.LandfillIds = string.Join(",", landfillIds);
                damage.UpdateDate = DateTimeHelper.GetCurrentTime();
                model.IsDamage = true;
                DamageRepository.Update(damage);
            }
            MapModel Place = new MapModel();
            Place = Place.FindGPS(user.City);
            ViewBag.Xpos = Place.Xpos;
            ViewBag.Ypos = Place.Ypos;
                             ViewBag.DiasterId = model.DiasterId;
            ViewBag.ReportDay = model.ReportDay;
            ViewBag.Diaster = DiasterRepository.Get(model.DiasterId);
            ViewBag.Data = DiasterRepository.GetList();
            ViewBag.Data2 = DamageRepository.GetListByFilter(new DamageFilterParameter
            {
                CityIds = user.CityId.ToListCollection(),
                DiasterIds = model.DiasterId.ToListCollection()
            });
            return RedirectToAction("B1c17", "EPBxDamage", new {reportDay = model.ReportDay, model.DiasterId , menu = "menu2"});
        }

        public ActionResult Report2(DamageModel model)
        {
            var user = GetUserBrief();

            model.CityId = user.CityId;

            if (user.Duty == DutyEnum.EPB)
            {
                //環保局
                model.TownId = 0;
            }

            DamageJoinModel damage = DamageRepository.GetListByFilter(new DamageFilterParameter
            {
                CityIds = model.CityId.ToListCollection(),
                DiasterIds = model.DiasterId.ToListCollection(),
                ReportDay = model.ReportDay,
                TownId = model.TownId,
            }).FirstOrDefault();

            if (damage == null)
            {
                model.DisinfectDate = DateTimeHelper.GetCurrentTime().Date;
                model.CreateDate = DateTimeHelper.GetCurrentTime();
                model.UpdateDate = DateTimeHelper.GetCurrentTime();
                model.IsDamage = true;
                DamageRepository.Create(model);
            }
            else
            {
                damage.CLE_Disinfect = model.CLE_Disinfect;
                damage.CLE_MUD = model.CLE_MUD;
                damage.CLE_Trash = model.CLE_Trash;
                damage.CLE_Garbage = model.CLE_Garbage;
                damage.CleaningMemberQuantity = model.CleaningMemberQuantity;
                damage.NationalArmyQuantity = model.NationalArmyQuantity;
                damage.IsDamage = true;
                DamageRepository.Update(damage);
            }
            MapModel Place = new MapModel();
            Place = Place.FindGPS(user.City);
            ViewBag.Xpos = Place.Xpos;
            ViewBag.Ypos = Place.Ypos;
            ViewBag.DiasterId = model.DiasterId;
            ViewBag.ReportDay = model.ReportDay;
            ViewBag.Diaster = DiasterRepository.Get(model.DiasterId);
            ViewBag.Data = DiasterRepository.GetList();
            ViewBag.Data2 = DamageRepository.GetListByFilter(new DamageFilterParameter
            {
                CityIds = user.CityId.ToListCollection(),
                DiasterIds = model.DiasterId.ToListCollection()
            });
            return RedirectToAction("B1c17", "EPBxDamage", new { reportDay = model.ReportDay, model.DiasterId, menu = "menu3" });
        }
        public ActionResult ConfirmReport(int id)
        {
            var user = GetUserBrief();

            DamageModel damage = DamageRepository.Get(id);

            damage.ConfirmTime = DateTimeHelper.GetCurrentTime();
            damage.Status = DamageStatusEnum.Confirm;

            DamageRepository.Update(damage);

            var town = TownRepository.Get(damage.TownId);

            TempData["Msg"] = $"已更新{town?.Name}主要災情與環境清潔通報";
            ViewBag.City = user.CityId;
            ViewBag.Data = DiasterRepository.GetList();
            ViewBag.Data2 = DamageRepository.GetListByFilter(new DamageFilterParameter
            {
                CityIds = user.CityId.ToListCollection(),
                DiasterIds = damage.DiasterId.ToListCollection()
            });
            return RedirectToAction("B1c18", "EPBxDamage", new
            {
                diasterId = damage.DiasterId,
                reportDay = damage.ReportDay
            });
        }

        [HttpPost]
        public ActionResult Report3(DamageModel model, List<HttpPostedFileBase> file)
        {
            var user = GetUserBrief();

            if (user.Duty == DutyEnum.EPB)
            {
                //環保局
                model.TownId = 0;
            }
            else if (user.Duty == DutyEnum.Cleaning)
            {
                //清潔隊
                model.TownId = user.TownId;
            }

            DamageJoinModel damage = DamageRepository.GetListByFilter(new DamageFilterParameter
            {
                CityIds = user.CityId.ToListCollection(),
                DiasterIds = model.DiasterId.ToListCollection(),
                ReportDay = model.ReportDay,
                TownId = model.TownId,
            }).FirstOrDefault();

            if (damage == null)
            {
                TempData["Msg"] = "尚未建立該災情通報";
                return RedirectToAction("B1c17", "EPBxDamage", new { model.ReportDay , model.DiasterId, menu = "menu3" });
            }
            //FileDataService.DeleteFileBySource(SourceTypeEnum.Damage, damage.Id);

            foreach (var f in file)
            {
                FileDataService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = f,
                    SourceId = damage.Id,
                    SourceType = SourceTypeEnum.DamageImage,
                    User = user.UserName
                });
            }
            damage.IsDamage = true;

            DamageRepository.Update(damage);

            MapModel Place = new MapModel();
            Place = Place.FindGPS(user.City);
            ViewBag.Xpos = Place.Xpos;
            ViewBag.Ypos = Place.Ypos;

            return RedirectToAction("B1c1", "EPBxDamage", new { damage.ReportDay, damage.DiasterId, menu = "menu4" });
        }

        [HttpPost]
        public ActionResult Report4(DamageModel model, List<HttpPostedFileBase> file)
        {
            var user = GetUserBrief();

            if (user.Duty == DutyEnum.EPB)
            {
                //環保局
                model.TownId = 0;
            }
            else if (user.Duty == DutyEnum.Cleaning)
            {
                //清潔隊
                model.TownId = user.TownId;
            }

            DamageJoinModel damage = DamageRepository.GetListByFilter(new DamageFilterParameter
            {
                CityIds = user.CityId.ToListCollection(),
                DiasterIds = model.DiasterId.ToListCollection(),
                ReportDay = model.ReportDay,
                TownId = model.TownId,
            }).FirstOrDefault();

            if(damage == null)
            {
                TempData["Msg"] = "尚未建立該災情通報";
                return RedirectToAction("B1c17", "EPBxDamage", new { model.ReportDay, model.DiasterId, menu = "menu4" });
            }

            //FileDataService.DeleteFileBySource(SourceTypeEnum.Damage, damage.Id);
            foreach (var f in file)
            {
                FileDataService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = f,
                    SourceId = damage.Id,
                    SourceType = SourceTypeEnum.DamageFile,
                    User = user.UserName
                });
            }
            
            damage.IsDamage = true;
            damage.FileId = null;

            DamageRepository.Update(damage);

            MapModel Place = new MapModel();
            Place = Place.FindGPS(user.City);

            ViewBag.Xpos = Place.Xpos;
            ViewBag.Ypos = Place.Ypos;

            return RedirectToAction("B1c", "EPBxDamage");
        }

    }
}