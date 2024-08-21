using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
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
            int townId = user.TownId;
            ////int townId = 0;
            ////if (user.Duty == DutyEnum.EPB)
            ////{
            ////    //環保局
            ////    townId = 0;
            ////}
            ////else if (user.Duty == DutyEnum.Cleaning)
            ////{
            ////    //清潔隊
            ////    townId = user.TownId;
            ////}

            ViewBag.ActiveDays = GetActiveDays(diaster.StartTime, diaster.EndTime);

            DamageFilterParameter filterParameter = new DamageFilterParameter()
            {
                HType = 1,
                CityIds = user.CityId.ToListCollection(),
                DiasterIds = diaster.Id.ToListCollection(),
                TownId = townId,
            };

            if (user.Duty == DutyEnum.Cleaning)
            {
                filterParameter.TownIds = user.TownId.ToListCollection();
            }

            var datas = DamageRepository.GetListByFilter(filterParameter)
                .OrderBy(a => a.ReportDay)
                .ToList();

            //災情通報
            foreach (var data in datas)
            {
                //無資料，無圖檔
                if (data.Id == 0)
                {
                    data.Files = new List<FileDataModel>();
                    data.Images = new List<FileDataModel>();
                    continue;
                }

                data.Files = FileDataService.GetBySource(SourceTypeEnum.DamageFile, data.Id);
                data.Images = FileDataService.GetBySource(SourceTypeEnum.DamageImage, data.Id);
            }

            ViewBag.Data2 = datas;

            ViewBag.Diaster = diaster;            

            return View("~/Views/EPB/B1c.cshtml");
        }
        public ActionResult B1c17(DateTime? reportDay, int? diasterId = null)
        {
            var user = GetUserBrief();
            ViewBag.Msg = TempData["Msg"];
            if (user.Duty == DutyEnum.Cleaning)
            {
                //清潔隊有鄉鎮
                ViewBag.Towns = new TownRepository().GetByCityId(user.CityId);
            }
            
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

                ////ViewBag.Data2 = DamageRepository.GetListByFilter(filterParameter);
               
                return View("~/Views/EPB/B1c17.cshtml");
            }
        }

        //環境清理
        public ActionResult B1cc(int? diasterId = null)
        {
            List<DiasterModel> diasters = DiasterRepository.GetList().OrderByDescending(e => e.Id).ToList();
            DiasterModel diaster = null;

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

            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;
            if (diasterId.HasValue)
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
            int townId = user.TownId;
            ////int townId = 0;
            ////if (user.Duty == DutyEnum.EPB)
            ////{
            ////    //環保局
            ////    townId = 0;
            ////}
            ////else if (user.Duty == DutyEnum.Cleaning)
            ////{
            ////    //清潔隊
            ////    townId = user.TownId;
            ////}

            ViewBag.ActiveDays = GetActiveDays(diaster.StartTime, diaster.EndTime);

            DamageFilterParameter filterParameter = new DamageFilterParameter()
            {
                HType = 2,
                CityIds = user.CityId.ToListCollection(),
                DiasterIds = diaster.Id.ToListCollection(),
                TownId = townId,
            };

            if (user.Duty == DutyEnum.Cleaning)
            {
                filterParameter.TownIds = user.TownId.ToListCollection();
            }

            var datas = DamageRepository.GetListByFilter(filterParameter)
                .OrderBy(a => a.CleanDay)
                .ToList();

            //環境清理
            foreach (var data in datas)
            {
                //無資料，無圖檔
                if (data.Id == 0)
                {
                    data.Files = new List<FileDataModel>();
                    data.Images = new List<FileDataModel>();
                    continue;
                }

                data.Files = FileDataService.GetBySource(SourceTypeEnum.DamageCCFile, data.Id);
                data.Images = FileDataService.GetBySource(SourceTypeEnum.DamageCCImage, data.Id);
            }

            ViewBag.Data2 = datas;

            ViewBag.Diaster = diaster;

            return View("~/Views/EPB/B1cc.cshtml");
        }

        public ActionResult B1cc17(DateTime? cleanDay, int? diasterId = null)
        {
            var user = GetUserBrief();
            ViewBag.Msg = TempData["Msg"];
            if (user.Duty == DutyEnum.Cleaning)
            {
                //清潔隊有鄉鎮
                ViewBag.Towns = new TownRepository().GetByCityId(user.CityId);
            }

            ViewBag.CleanDay = cleanDay;
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

            if (cleanDay.HasValue == false)
            {
                TempData["Msg"] = "請先選擇環境清理通報日期";
                ViewBag.ActiveDays = GetActiveDays(diaster.StartTime, diaster.EndTime);

                return RedirectToAction("B1cc", "EPBxDamage");

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
                    //CleanDay = cleanDay,
                    ReportDay = cleanDay,
                };

                if (user.Duty == DutyEnum.Cleaning)
                {
                    filterParameter.TownIds = user.TownId.ToListCollection();
                }

                ////ViewBag.Data2 = DamageRepository.GetListByFilter(filterParameter);

                return View("~/Views/EPB/B1cc17.cshtml");
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

            var datas = new DamageService().GetConfirmList(new DamageConfirmListFilterParameter
            {
                DamageFilterParameter = new DamageFilterParameter
                {
                    CityIds = user.CityId.ToListCollection(),
                    DiasterIds = diaster.Id.ToListCollection(),
                    ReportDay = reportDay
                },
                Diaster = diaster,
            }).OrderBy(e => e.ReportDay).ThenBy(e => e.TownId);

            //災情通報
            foreach (var data in datas)
            {
                //無資料，無圖檔
                if (data.Id == 0)
                {
                    data.Files = new List<FileDataModel>();
                    data.Images = new List<FileDataModel>();
                    continue;
                }

                data.Files = FileDataService.GetBySource(SourceTypeEnum.DamageFile, data.Id);
                data.Images = FileDataService.GetBySource(SourceTypeEnum.DamageImage, data.Id);
            }

            ViewBag.Data2 = datas;

            return View("~/Views/EPB/B1c18.cshtml");
        }

        public ActionResult B1cc18(int? diasterId = null, DateTime? cleanDay = null)
        {
            List<DiasterModel> diasters = DiasterRepository.GetList().OrderByDescending(e => e.Id).ToList();

            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;

            DiasterModel diaster = GetDiaster(diasters, diasterId);

            cleanDay = cleanDay.HasValue ? cleanDay.Value : diaster.StartTime;

            var user = GetUserBrief();

            ViewBag.City = user.City;
            ViewBag.DiasterId = diaster.Id;
            ViewBag.DiasterName = diaster.DiasterName;
            ViewBag.StartTime = diaster.StartTime;
            ViewBag.EndTime = diaster.EndTime;
            ViewBag.CleanDay = cleanDay;
            ViewBag.ActiveDays = GetActiveDays(diaster.StartTime, diaster.EndTime);

            var datas = new DamageService().GetConfirmList(new DamageConfirmListFilterParameter
            {
                DamageFilterParameter = new DamageFilterParameter
                {
                    CityIds = user.CityId.ToListCollection(),
                    DiasterIds = diaster.Id.ToListCollection(),
                    CleanDay = cleanDay
                },
                Diaster = diaster,
            }).OrderBy(e => e.CleanDay).ThenBy(e => e.TownId);

            //環境清理
            foreach (var data in datas)
            {
                //無資料，無圖檔
                if (data.Id == 0)
                {
                    data.Files = new List<FileDataModel>();
                    data.Images = new List<FileDataModel>();
                    continue;
                }

                data.Files = FileDataService.GetBySource(SourceTypeEnum.DamageCCFile, data.Id);
                data.Images = FileDataService.GetBySource(SourceTypeEnum.DamageCCImage, data.Id);
            }

            ViewBag.Data2 = datas;

            return View("~/Views/EPB/B1cc18.cshtml");
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

            //沒Town(前端限定)，預設登入者Town
            if (model.TownId == 0)
                model.TownId = user.TownId;

            ////if (user.Duty == DutyEnum.EPB)
            ////{
            ////    //環保局
            ////    model.TownId = 0;
            ////}

            //先判斷是否有災情回報
            DateTime? date = model.ReportDay;
            DamageFilterParameter filter = new DamageFilterParameter
            {
                CityIds = model.CityId.ToListCollection(),
                DiasterIds = model.DiasterId.ToListCollection(),
                ReportDay = date,
                TownId = model.TownId,
            };

            DamageJoinModel damage = DamageRepository.GetListByFilter(filter).FirstOrDefault();

            if (damage == null)
            {
                //否則取環境清理
                filter = new DamageFilterParameter
                {
                    CityIds = model.CityId.ToListCollection(),
                    DiasterIds = model.DiasterId.ToListCollection(),
                    CleanDay = date,
                    TownId = model.TownId,
                };

                damage = DamageRepository.GetListByFilter(filter).FirstOrDefault();
            }

            int dmgId;
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

                //災情通報日期
                filter.ReportDay = date;
                filter.CleanDay = null;
                var add = DamageRepository.GetListByFilter(filter).FirstOrDefault();
                dmgId = add.Id;
            }
            else
            {
                damage.ReportDay = model.ReportDay;
                damage.DamagePlace = model.DamagePlace;
                damage.DamageArea = model.DamageArea;
                damage.FloodArea = model.FloodArea;
                damage.DumpSiteDesc = model.DumpSiteDesc;
                damage.IncinerationPlantDesc = model.IncinerationPlantDesc;
                damage.Other = model.Other;
                damage.PR_Garbage = model.PR_Garbage;
                damage.IncineratorIds = string.Join(",", incineratorIds);
                damage.LandfillIds = string.Join(",", landfillIds);
                damage.UpdateDate = DateTimeHelper.GetCurrentTime();
                damage.Status = DamageStatusEnum.Waiting;
                damage.IsDamage = true;
                DamageRepository.Update(damage);

                dmgId = damage.Id;
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

            //照片(環境清理)
            Dictionary<string, List<HttpPostedFileBase>> files = GetUploadFiles();
            List<HttpPostedFileBase> fileBases;
            string key;

            key = "DamageImage";
            if (files.ContainsKey(key))
            {
                fileBases = files[key];
                foreach (var file in fileBases)
                {
                    FileDataService.UploadFileByGuidName(new UploadFileBaseModel
                    {
                        File = file,
                        SourceId = dmgId,
                        SourceType = SourceTypeEnum.DamageImage,
                        User = user.UserName
                    }, false);
                }
            }

            //檔案(環境清理)
            key = "DamageFile";
            if (files.ContainsKey(key))
            {
                fileBases = files[key];
                foreach (var file in fileBases)
                {
                    FileDataService.UploadFileByGuidName(new UploadFileBaseModel
                    {
                        File = file,
                        SourceId = dmgId,
                        SourceType = SourceTypeEnum.DamageFile,
                        User = user.UserName
                    }, false);
                }
            }

            return RedirectToAction("B1c", "EPBxDamage");
            //return RedirectToAction("B1c17", "EPBxDamage", new {reportDay = model.ReportDay, model.DiasterId , menu = "menu3"});
        }

        /// <summary>
        /// 通報Menu2
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Report2(DamageModel model)
        {
            var user = GetUserBrief();

            model.CityId = user.CityId;

            //沒Town(前端限定)，預設登入者Town
            if (model.TownId == 0)
                model.TownId = user.TownId;

            ////if (user.Duty == DutyEnum.EPB)
            ////{
            ////    //環保局
            ////    model.TownId = 0;
            ////}

            //先判斷是否有災情回報
            DateTime? date = model.CleanDay;
            DamageFilterParameter filter = new DamageFilterParameter
            {
                CityIds = model.CityId.ToListCollection(),
                DiasterIds = model.DiasterId.ToListCollection(),
                ReportDay = date,
                TownId = model.TownId,
            };

            DamageJoinModel damage = DamageRepository.GetListByFilter(filter).FirstOrDefault();

            if (damage == null)
            {
                //否則取環境清理
                filter = new DamageFilterParameter
                {
                    CityIds = model.CityId.ToListCollection(),
                    DiasterIds = model.DiasterId.ToListCollection(),
                    CleanDay = date,
                    TownId = model.TownId,
                };
               
                damage = DamageRepository.GetListByFilter(filter).FirstOrDefault();
            }

            int dmgId;
            if (damage == null)
            {
                model.CreateDate = DateTimeHelper.GetCurrentTime();
                model.UpdateDate = DateTimeHelper.GetCurrentTime();
                model.CleanStatus = DamageStatusEnum.Waiting;
                model.IsDamageClean = true;
                DamageRepository.Create(model);

                //環境清理日期
                filter.ReportDay = null;
                filter.CleanDay = date;
                var add = DamageRepository.GetListByFilter(filter).FirstOrDefault();
                dmgId = add.Id;
            }
            else
            {
                damage.CleanDay = model.CleanDay;
                damage.DisinfectDate = model.DisinfectDate;
                damage.DisinfectArea = model.DisinfectArea;
                damage.CLE_MUD = model.CLE_MUD;
                damage.CLE_Trash = model.CLE_Trash;
                damage.CLE_Garbage = model.CLE_Garbage;
                damage.CleaningMemberQuantity = model.CleaningMemberQuantity;
                damage.NationalArmyQuantity = model.NationalArmyQuantity;
                damage.CLE_DisinfectorL = model.CLE_DisinfectorL;
                damage.CLE_DisinfectorW = model.CLE_DisinfectorW;
                damage.CLE_EquipmentDesc = model.CLE_EquipmentDesc;
                damage.CLE_CarDesc = model.CLE_CarDesc;
                damage.CleanStatus = DamageStatusEnum.Waiting;
                damage.IsDamageClean = true;
                DamageRepository.Update(damage);

                dmgId = damage.Id;
            }

            MapModel Place = new MapModel();
            Place = Place.FindGPS(user.City);
            ViewBag.Xpos = Place.Xpos;
            ViewBag.Ypos = Place.Ypos;
            ViewBag.DiasterId = model.DiasterId;
            ViewBag.CleanDay = model.CleanDay;
            ViewBag.Diaster = DiasterRepository.Get(model.DiasterId);
            ViewBag.Data = DiasterRepository.GetList();
            ViewBag.Data2 = DamageRepository.GetListByFilter(new DamageFilterParameter
            {
                CityIds = user.CityId.ToListCollection(),
                DiasterIds = model.DiasterId.ToListCollection()
            });

            //照片(環境清理)
            Dictionary<string, List<HttpPostedFileBase>> files = GetUploadFiles();
            List<HttpPostedFileBase> fileBases;
            string key;

            key = "DamageCCImage";
            if (files.ContainsKey(key))
            {
                fileBases = files[key];
                foreach (var file in fileBases)
                {
                    FileDataService.UploadFileByGuidName(new UploadFileBaseModel
                    {
                        File = file,
                        SourceId = dmgId,
                        SourceType = SourceTypeEnum.DamageCCImage,
                        User = user.UserName
                    }, false);
                }
            }

            //檔案(環境清理)
            key = "DamageCCFile";
            if (files.ContainsKey(key))
            {
                fileBases = files[key];
                foreach (var file in fileBases)
                {
                    FileDataService.UploadFileByGuidName(new UploadFileBaseModel
                    {
                        File = file,
                        SourceId = dmgId,
                        SourceType = SourceTypeEnum.DamageCCFile,
                        User = user.UserName
                    }, false);
                }
            }

            return RedirectToAction("B1cc", "EPBxDamage");
        }
        public ActionResult ConfirmReport(int id, int hType)
        {
            var user = GetUserBrief();

            DamageModel damage = DamageRepository.Get(id);

            ActionResult actResult = null;
            if (hType == 1)
            {
                damage.ConfirmTime = DateTimeHelper.GetCurrentTime();
                damage.Status = DamageStatusEnum.Confirm;

                actResult = RedirectToAction("B1c18", "EPBxDamage", new
                {
                    diasterId = damage.DiasterId,
                    reportDay = damage.ReportDay
                });
            }
            else if (hType == 2)
            {
                damage.CleanConfirmTime = DateTimeHelper.GetCurrentTime();
                damage.CleanStatus = DamageStatusEnum.Confirm;

                actResult = RedirectToAction("B1cc18", "EPBxDamage", new
                {
                    diasterId = damage.DiasterId,
                    cleanDay = damage.CleanDay
                });
            }

            DamageRepository.Update(damage);

            //////var town = TownRepository.Get(damage.TownId);

            //////TempData["Msg"] = $"已更新{town?.Name}主要災情與環境清潔通報";
            //////ViewBag.City = user.CityId;
            //////ViewBag.Data = DiasterRepository.GetList();
            //////ViewBag.Data2 = DamageRepository.GetListByFilter(new DamageFilterParameter
            //////{
            //////    CityIds = user.CityId.ToListCollection(),
            //////    DiasterIds = damage.DiasterId.ToListCollection()
            //////});

            return actResult;
        }

        ///////// <summary>
        ///////// 通報Menu3
        ///////// </summary>
        ///////// <param name="model"></param>
        ///////// <param name="file"></param>
        ///////// <param name="actType">1.災情通報2.環境清理</param>
        ///////// <returns></returns>
        //////[HttpPost]
        //////public ActionResult Report3(DamageModel model, List<HttpPostedFileBase> file, int actType = 1)
        //////{
        //////    string action = "";

        //////    var user = GetUserBrief();

        //////    if (user.Duty == DutyEnum.EPB)
        //////    {
        //////        //環保局
        //////        model.TownId = 0;
        //////    }
        //////    else if (user.Duty == DutyEnum.Cleaning)
        //////    {
        //////        //清潔隊
        //////        model.TownId = user.TownId;
        //////    }

        //////    DamageJoinModel damage = DamageRepository.GetListByFilter(new DamageFilterParameter
        //////    {
        //////        CityIds = user.CityId.ToListCollection(),
        //////        DiasterIds = model.DiasterId.ToListCollection(),
        //////        ReportDay = model.ReportDay,
        //////        TownId = model.TownId,
        //////    }).FirstOrDefault();

        //////    if (damage == null)
        //////    {
        //////        if (actType == 2)
        //////        {
        //////            TempData["Msg"] = "尚未建立該環境清理";
        //////            action = "B1cc17";
        //////        }
        //////        else
        //////        {
        //////            TempData["Msg"] = "尚未建立該災情通報";
        //////            action = "B1c17";
        //////        }
                
        //////        return RedirectToAction(action, "EPBxDamage", new { model.ReportDay , model.DiasterId, menu = "menu3" });
        //////    }
        //////    //FileDataService.DeleteFileBySource(SourceTypeEnum.Damage, damage.Id);

        //////    SourceTypeEnum image = SourceTypeEnum.None;
        //////    if (actType == 2)
        //////    {
        //////        image = SourceTypeEnum.DamageCCImage;
        //////    }
        //////    else
        //////    {
        //////        image = SourceTypeEnum.DamageImage;
        //////    }

        //////    foreach (var f in file)
        //////    {
        //////        FileDataService.UploadFileByGuidName(new UploadFileBaseModel
        //////        {
        //////            File = f,
        //////            SourceId = damage.Id,
        //////            SourceType = image,
        //////            User = user.UserName
        //////        });
        //////    }
        //////    damage.IsDamage = true;

        //////    DamageRepository.Update(damage);

        //////    MapModel Place = new MapModel();
        //////    Place = Place.FindGPS(user.City);
        //////    ViewBag.Xpos = Place.Xpos;
        //////    ViewBag.Ypos = Place.Ypos;

        //////    //return RedirectToAction("B1c1", "EPBxDamage", new { damage.ReportDay, damage.DiasterId, menu = "menu4" });
        //////    if (actType == 2)
        //////    {
        //////        action = "B1cc";
        //////    }
        //////    else
        //////    {
        //////        action = "B1c";
        //////    }

        //////    return RedirectToAction(action, "EPBxDamage");
        //////}

        ///////// <summary>
        ///////// 通報Menu4
        ///////// </summary>
        ///////// <param name="model"></param>
        ///////// <param name="file"></param>
        ///////// <param name="actType">1.災情通報2.環境清理</param>
        ///////// <returns></returns>
        //////[HttpPost]
        //////public ActionResult Report4(DamageModel model, List<HttpPostedFileBase> file, int actType = 1)
        //////{
        //////    string action = "";

        //////    var user = GetUserBrief();

        //////    if (user.Duty == DutyEnum.EPB)
        //////    {
        //////        //環保局
        //////        model.TownId = 0;
        //////    }
        //////    else if (user.Duty == DutyEnum.Cleaning)
        //////    {
        //////        //清潔隊
        //////        model.TownId = user.TownId;
        //////    }

        //////    DamageJoinModel damage = DamageRepository.GetListByFilter(new DamageFilterParameter
        //////    {
        //////        CityIds = user.CityId.ToListCollection(),
        //////        DiasterIds = model.DiasterId.ToListCollection(),
        //////        ReportDay = model.ReportDay,
        //////        TownId = model.TownId,
        //////    }).FirstOrDefault();

        //////    if(damage == null)
        //////    {
        //////        if (actType == 2)
        //////        {
        //////            TempData["Msg"] = "尚未建立該環境清理";
        //////            action = "B1cc17";
        //////        }
        //////        else
        //////        {
        //////            TempData["Msg"] = "尚未建立該災情通報";
        //////            action = "B1c17";
        //////        }

        //////        TempData["Msg"] = "尚未建立該災情通報";
        //////        return RedirectToAction(action, "EPBxDamage", new { model.ReportDay, model.DiasterId, menu = "menu4" });
        //////    }

        //////    //FileDataService.DeleteFileBySource(SourceTypeEnum.Damage, damage.Id);

        //////    SourceTypeEnum sFile = SourceTypeEnum.None;
        //////    if (actType == 2)
        //////    {
        //////        sFile = SourceTypeEnum.DamageCCFile;
        //////    }
        //////    else
        //////    {
        //////        sFile = SourceTypeEnum.DamageFile;
        //////    }

        //////    foreach (var f in file)
        //////    {
        //////        FileDataService.UploadFileByGuidName(new UploadFileBaseModel
        //////        {
        //////            File = f,
        //////            SourceId = damage.Id,
        //////            SourceType = sFile,
        //////            User = user.UserName
        //////        });
        //////    }
            
        //////    damage.IsDamage = true;
        //////    damage.FileId = null;

        //////    DamageRepository.Update(damage);

        //////    MapModel Place = new MapModel();
        //////    Place = Place.FindGPS(user.City);

        //////    ViewBag.Xpos = Place.Xpos;
        //////    ViewBag.Ypos = Place.Ypos;

        //////    if (actType == 2)
        //////    {
        //////        action = "B1cc";
        //////    }
        //////    else
        //////    {
        //////        action = "B1c";
        //////    }

        //////    return RedirectToAction(action, "EPBxDamage");
        //////}

    }
}