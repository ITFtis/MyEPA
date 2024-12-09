using MyEPA.Enums;
using MyEPA.EPA.Attribute;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    [LoginRequired]
    public class DamageController : LoginBaseController
    {
        DamageMainService DamageMainService = new DamageMainService();
        DamageService DamageService = new DamageService();
        DiasterService DiasterService = new DiasterService();
        TownService TownService = new TownService();
        CityService CityService = new CityService();
        LandfillService LandfillService = new LandfillService();
        FileDataService FileDataService = new FileDataService();

        public ActionResult Index(int? diasterId = null, int? cityId = null,int? townId = null, DateTime? cleanStartTime = null,DateTime? cleanEndTime = null, int? areaId = null)
        {
            List<DiasterModel> diasters = DiasterService.GetAll();
            
            DamageFilterParameter filter = new DamageFilterParameter()
            {
                CityIds = new List<int>(),
                TownIds = new List<int>(),
                DiasterIds = new List<int>(),
                CleanEndTime = cleanEndTime,
                CleanStartTime = cleanStartTime
            };

            if (cityId.HasValue)
            {
                filter.CityIds.Add(cityId.Value);
            }
            if(townId.HasValue)
            {
                filter.TownIds.Add(townId.Value);
            }
            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            if (areaId.HasValue)
            {
                filter.AreaId = areaId;
            }

            filter.DiasterIds.Add(diasterId.Value);

            ViewBag.DiasterId = diasterId;
            ViewBag.CityId = cityId;
            ViewBag.TownId = townId;
            ViewBag.CleanStartTime = cleanStartTime;
            ViewBag.CleanEndTime = cleanEndTime;
            ViewBag.Diasters = diasters;
            ViewBag.Citys = CityService.GetCountyOrderBySort();
            ViewBag.AreaId = areaId;

            var datas = DamageService.GetCleanByFilter(filter);
            datas = datas.OrderBy(a => a.CitySort)
                        .ThenBy(a => a.TownName)
                        .ThenByDescending(a => a.CleanDay).ToList();

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

            var result = datas;

            return View(result);
        }

        public ActionResult Details(int id)
        {
            var user = GetUserBrief();

            var result = DamageService.Get(id);

            ViewBag.Landfills = new LandfillService().GetByFilter(new LandfillFilterParameter
            {
                City = user.City
            }).Select(e => new SelectListItem { Text = e.ContactUnit, Value = e.Id.ToString() }).ToList();

            ViewBag.Incinerators = new IncineratorService().GetByFilter(new IncineratorFilterParameter
            {
                City = user.City
            }).Select(e => new SelectListItem { Text = e.ContactUnit, Value = e.Id.ToString() }).ToList();

            //災情通報
            ViewBag.Files = FileDataService.GetBySource(SourceTypeEnum.DamageFile, id);
            ViewBag.Images = FileDataService.GetBySource(SourceTypeEnum.DamageImage, id);

            //環境清理
            ViewBag.CCFiles = FileDataService.GetBySource(SourceTypeEnum.DamageCCFile, id);
            ViewBag.CCImages = FileDataService.GetBySource(SourceTypeEnum.DamageCCImage, id);

            return View(result);
        }
        public ActionResult Edit(int id)
        {
            var user = GetUserBrief();

            var result = DamageService.Get(id);
            
            ViewBag.Landfills = new LandfillService().GetByFilter(new LandfillFilterParameter
            {
                City = user.City
            }).Select(e => new SelectListItem { Text = e.ContactUnit, Value = e.Id.ToString() }).ToList();

            ViewBag.Incinerators = new IncineratorService().GetByFilter(new IncineratorFilterParameter
            {
                City = user.City
            }).Select(e => new SelectListItem { Text = e.ContactUnit, Value = e.Id.ToString() }).ToList();
            //災情通報
            ViewBag.Files = FileDataService.GetBySource(SourceTypeEnum.DamageFile, id);
            ViewBag.Images = FileDataService.GetBySource(SourceTypeEnum.DamageImage, id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(DamageViewModel model)
        {
            DamageService.Update(GetUserBrief(), model, GetUploadFiles());

            return RedirectToAction("B1c", "EPBxDamage", new { diasterId = model.DiasterId });

        }

        //環境清理
        public ActionResult EditCC(int id)
        {
            var user = GetUserBrief();
            var result = DamageService.Get(id);

            //環境清理
            ViewBag.CCFiles = FileDataService.GetBySource(SourceTypeEnum.DamageCCFile, id);
            ViewBag.CCImages = FileDataService.GetBySource(SourceTypeEnum.DamageCCImage, id);
            return View(result);
        }

        [HttpPost]
        public ActionResult EditCC(DamageViewModel model)
        {
            //環境清理
            DamageService.UpdateCC(GetUserBrief(), model, GetUploadFiles());

            return RedirectToAction("B1cc", "EPBxDamage", new { diasterId = model.DiasterId });

        }

        public ActionResult del(int id, string returnUrl = "")
        {
            SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
            X.Open();
            string G = "DELETE FROM [dbo].[Damage] where Id=@id";
            SqlCommand Q = new SqlCommand(G, X);
            Q.Parameters.AddWithValue("@id", id);
            Q.ExecuteNonQuery();
            X.Close();

            if (returnUrl != "")
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("B1c", "EPBxDamage");
            }
        }

        public ActionResult TownList(int diasterId, DateTime? date, int? cityId = null, int? townId = null)
        {
            var datas = DamageService.GetTownList(diasterId, date, cityId);

            if (townId != null)
            {
                //環保局通報(1筆)
                datas = datas.Where(a => a.TownId == townId).ToList();
            }
            else
            {
                //鄉鎮通報(多筆)
                datas = datas.Where(a => a.IsTown == true).ToList();
            }

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

            var result = datas;

            if (townId != null)
            {
                //環保局通報(1筆)
                return View("~/Views/Damage/CityList.cshtml", datas);
            }
            else
            {
                //鄉鎮通報(多筆)
                return View(result);
            }    
        }

        public ActionResult TownCCList(int diasterId, DateTime? date, int? cityId = null, int? townId = null)
        {
            var datas = DamageService.GetTownCCList(diasterId, date, cityId);

            if (townId != null)
            {
                //環保局通報(1筆)
                datas = datas.Where(a => a.TownId == townId).ToList();
            }
            else
            {
                //鄉鎮通報(多筆)
                datas = datas.Where(a => a.IsTown == true).ToList();
            }

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

            var result = datas;

            if (townId != null)
            {
                //環保局通報(1筆)
                return View("~/Views/Damage/CityCCList.cshtml", datas);
            }
            else
            {
                //鄉鎮通報(多筆)
                return View(result);
            }
        }

        public ActionResult TeamConfirmList(int? diasterId = null,int? cityId = null)
        {
            var diasters = DiasterService.GetAll();
            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            AreaEnum? area = GetArea();
            if (area.HasValue == false)
            {
                throw new NotImplementedException();
            }
            var result = DamageService.GetConfirmList(diasterId.Value, area.Value, cityId);

            ViewBag.DiasterId = diasterId;
            ViewBag.CityId = cityId;
            ViewBag.Diasters = diasters;
            ViewBag.Citys = CityService.GetListByFilter(new CityFilterParameter 
            {
                AreaIds = area.Value.ToInteger().ToListCollection(),
                IsCounty = true
            });

            return View(result);
        }

        public ActionResult TeamConfirmCCList(int? diasterId = null, int? cityId = null)
        {
            var diasters = DiasterService.GetAll();
            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            AreaEnum? area = GetArea();
            if (area.HasValue == false)
            {
                throw new NotImplementedException();
            }
            var result = DamageService.GetConfirmCCList(diasterId.Value, area.Value, cityId);

            ViewBag.DiasterId = diasterId;
            ViewBag.CityId = cityId;
            ViewBag.Diasters = diasters;
            ViewBag.Citys = CityService.GetListByFilter(new CityFilterParameter
            {
                AreaIds = area.Value.ToInteger().ToListCollection(),
                IsCounty = true
            });

            return View(result);
        }

        /// <summary>
        /// 區大隊 => 確認環保局通報
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="hType">1.災情通報2.環境清理</param>
        /// <returns></returns>
        public ActionResult Confirm(int id, DamageStatusEnum status, int hType)
        {
            DamageService.TeamConfirm(GetUserBrief(), id, status, hType);
            return JsonResult(new
            {
                IsSuccess = true
            });
        }
        /// <summary>
        /// 狀態改為無災情
        /// </summary>
        /// <param name="model"></param>
        /// <param name="hType">1.災情通報2.環境清理</param>
        /// <returns></returns>
        public ActionResult NotDamage(DamageModel model, int hType)
        {
            var user = GetUserBrief();

            //沒Town(前端限定)，預設登入者Town
            if (model.TownId == 0)
                model.TownId = user.TownId;

            DamageService.NotDamage(GetUserBrief(), model, hType);
            return JsonResult(new
            {
                IsSuccess = true
            });
        }
        /// <summary>
        /// 環保局結案
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Done(int diasterId)
        {
            DamageMainService.UpdateByDone(diasterId,GetUserCityId());
            return JsonResult(new
            {
                IsSuccess = true
            });
        }
        /// <summary>
        /// 災情通報
        /// </summary>
        /// <param name="diasterId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public ActionResult Report(int? diasterId,int? cityId = null)
        {
            var diasters = DiasterService.GetAll();
            if(diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            ViewBag.Diasters = diasters;
            ViewBag.DiasterId = diasterId;
            ViewBag.CityId = cityId;
            ViewBag.Citys = CityService.GetCountyOrderBySort();
            var result = DamageService.GetReport(new DamageReportFilterModel 
            {
                DiasterId = diasterId.Value,
                CityId = cityId
            });
            return View(result);
        }
        public ActionResult DownReportPDF(int? diasterId, int? cityId = null)
        {
            var model = DamageService.GetReport(new DamageReportFilterModel
            {
                DiasterId = diasterId.Value,
                CityId = cityId
            });
            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("UpdateDate");
            ignoreFields.Add("Status");
            ignoreFields.Add("CityId");
            
            return File(GeneratePDF(model, "災情通報", ignoreFields));
        }
        public ActionResult DownReportODS(int? diasterId, int? cityId = null)
        {
            var model = DamageService.GetReport(new DamageReportFilterModel
            {
                DiasterId = diasterId.Value,
                CityId = cityId
            });
            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("UpdateDate");
            ignoreFields.Add("Status");
            ignoreFields.Add("CityId");

            return File(GenerateODS(model, "災情通報", ignoreFields));
        }
        public ActionResult TownReport(int diasterId,int cityId)
        {
            ViewBag.DiasterId = diasterId;
            var result = DamageService.GetTownReport(new DamageTownReportFilterModel
            {
                DiasterId = diasterId,
                CityId = cityId
            });
            return View(result);
        }
        /// <summary>
        /// 災情統計
        /// </summary>
        /// <param name="diasterId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ActionResult CityStatistics(int? diasterId = null, DateTime? date = null, int? areaId = null)
        {
            var diasters = DiasterService.GetAll();
            if(diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            ViewBag.Diasters = diasters;
            ViewBag.Diaster = diasters.FirstOrDefault(e => e.Id == diasterId.Value);
            ViewBag.Date = date;
            ViewBag.AreaId = areaId;
            var result = DamageService.GetCityStatistics(new DamageReportFilterModel
            {
                DiasterId = diasterId.Value,
                Date = date,
                AreaId = areaId,
            });

            return View(result);
        }
        /// <summary>
        /// 災情統計-鄉鎮
        /// </summary>
        /// <param name="diasterId"></param>
        /// <param name="cityId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ActionResult TownStatistics(int diasterId,int cityId, DateTime? date = null)
        {
            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = DiasterService.GetAll();

            var result = DamageService.GetTownStatistics(new DamageReportFilterModel
            {
                DiasterId = diasterId,
                CityId = cityId,
                Date = date
            });

            return View(result);
        }
        /// <summary>
        /// 環境設施災損查詢
        /// </summary>
        /// <param name="diasterId"></param>
        /// <param name="cityId"></param>
        /// <param name="townId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult FacilityDamage(int? diasterId,int? cityId, int? townId, FacilityDamageTypeEnum type = FacilityDamageTypeEnum.ALL, int? areaId = null)
        {
            List<DiasterModel> diasters = DiasterService.GetAll();

            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            ViewBag.Type = type;
            ViewBag.Citys = CityService.GetAll();
            ViewBag.Towns = TownService.GetAll();
            ViewBag.CityId = cityId;
            ViewBag.TownId = townId;
            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;
            ViewBag.AreaId = areaId;

            ViewBag.FacilityDamageTypes = ExtensionsOfEnum.GetEnumAllValue<FacilityDamageTypeEnum>();
            var filter = new DamageFilterParameter();


            filter = new DamageFilterParameter()
            {
                DiasterIds = diasterId.Value.ToListCollection(),
                Type = type,
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int>(),
                TownIds = townId.HasValue ? townId.Value.ToListCollection() : new List<int>(),
                AreaId = areaId,
            };

            var datas = DamageService.GetFacilityDamages(filter, type);
            datas = datas.OrderBy(a => a.CitySort)
                        .ThenBy(a => a.TownName)
                        .ThenByDescending(a => a.ReportDay).ToList();

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

            var result = datas;
            return View(result);
        }
        public ActionResult CorpsHandlingSituationShow(int id, FacilityDamageTypeEnum type)
        {
            var result = DamageService.GetDamage(id);
            ViewBag.Type = type;
            return View(result);
        }

        public ActionResult CorpsHandlingSituation(int id, FacilityDamageTypeEnum type, string returnUrl = "")
        { 
            var result = DamageService.GetCorpsHandlingSituation(id, type);

            TempData["returnUrl"] = returnUrl;

            //三區回報處理情形
            ViewBag.DamageProcessFiles = FileDataService.GetBySource(SourceTypeEnum.DamageProcessFile, id);
            ViewBag.DamageProcessImages = FileDataService.GetBySource(SourceTypeEnum.DamageProcessImage, id);

            return View(result);
        }
        [HttpPost]
        public ActionResult CorpsHandlingSituation(CorpsHandlingSituationViewModel model)
        {
            var user = GetUserBrief();
            DamageModel damage = DamageService.UpdateCorpsHandlingSituation(model, user);            

            //照片(三區回報處理情形)
            Dictionary<string, List<HttpPostedFileBase>> files = GetUploadFiles();
            List<HttpPostedFileBase> fileBases;
            string key;

            key = "DamageProcessImage";
            if (files.ContainsKey(key))
            {
                fileBases = files[key];
                foreach (var file in fileBases)
                {
                    FileDataService.UploadFileByGuidName(new UploadFileBaseModel
                    {
                        File = file,
                        SourceId = model.DamageId,
                        SourceType = SourceTypeEnum.DamageProcessImage,
                        User = user.UserName
                    }, false);
                }
            }

            //檔案(環境清理)
            key = "DamageProcessFile";
            if (files.ContainsKey(key))
            {
                fileBases = files[key];
                foreach (var file in fileBases)
                {
                    FileDataService.UploadFileByGuidName(new UploadFileBaseModel
                    {
                        File = file,
                        SourceId = model.DamageId,
                        SourceType = SourceTypeEnum.DamageProcessFile,
                        User = user.UserName
                    }, false);
                }
            }

            string returnUrl = TempData["returnUrl"].ToString();
            if (returnUrl != "")
            {
                Response.Redirect(returnUrl);
                return null;
            }
            else
            {
                return RedirectToAction("FacilityDamage", "Damage");
            }
            //return RedirectToAction("FacilityDamage", "Damage",new { diasterId  = damage.DiasterId, cityId = damage.CityId,townId = damage.TownId,type = model.Type });
        }

        public ActionResult MemoShow(int id, FacilityDamageTypeEnum type)
        {
            var result = DamageService.GetDamage(id);
            ViewBag.Type = type;
            return View(result);
        }

        public ActionResult Memo(int id, FacilityDamageTypeEnum type, string returnUrl = "")
        {
            var result = DamageService.GetDamageMemo(id, type);

            TempData["returnUrl"] = returnUrl;

            return View(result);
        }
        [HttpPost]
        public ActionResult Memo(DamageMemoViewModel model)
        {
            DamageModel damage = DamageService.UpdateMemo(model);

            string returnUrl = TempData["returnUrl"].ToString();
            if (returnUrl != "")
            {
                Response.Redirect(returnUrl);
                return null;
            }
            else
            {
                return RedirectToAction("FacilityDamage", "Damage");
            }

            //return RedirectToAction("FacilityDamage", "Damage");
            //return RedirectToAction("FacilityDamage", "Damage", new { diasterId = damage.DiasterId, cityId = damage.CityId, townId = damage.TownId, type = model.Type });
        }
        

    }
}