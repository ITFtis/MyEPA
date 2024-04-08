using MyEPA.Models;
using MyEPA.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class WaterCheckDetailController : LoginBaseController
    {
        WaterCheckDetailService WaterCheckDetailService = new WaterCheckDetailService();
        WaterCheckService WaterCheckService = new WaterCheckService();
        CityService CityService = new CityService();
        DiasterService DiasterService = new DiasterService();
        WaterDivisionService WaterDivisionService = new WaterDivisionService();
        public ActionResult Index(WaterCheckModel waterCheck)
        {
            UserBriefModel user = GetUserBrief();

            List<WaterCheckDetailModel> result = null;
            if (waterCheck.Id == 0)
            {
                //新增
                result = WaterCheckDetailService.GetListByWaterCheckId(user, waterCheck);
            }
            else
            {
                //修改
                result = WaterCheckDetailService.GetListByWaterCheckId2(user, waterCheck.Id);
            }

            ViewBag.User = user;
            ViewBag.WaterCheck = waterCheck;

            return View(result);
        }
        public ActionResult Edit(int id)
        {
            UserBriefModel user = GetUserBrief();
            List<CityModel> citys = new List<CityModel>();
            if (user.Duty == Enums.DutyEnum.EPB)
            {
              
                citys.Add(CityService.Get(user.CityId));
               
            }
            else
            {
                citys = CityService.GetAll().Select(e => new CityModel
                {
                    City = e.City,
                    Id = e.Id,
                }).ToList();
            }
            ViewBag.Citys = citys;
            var result = WaterCheckDetailService.GetById(id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(WaterCheckDetailModel model)
        {
            WaterCheckDetailService.Update(GetUserBrief(), model);

            var waterCheck = WaterCheckService.Get(model.WaterCheckId);
            return RedirectToAction("Index", waterCheck);
        }
        public ActionResult Create(int waterCheckId)
        {
            UserBriefModel user = GetUserBrief();
            List<CityModel> citys = new List<CityModel>();
            if (user.Duty == Enums.DutyEnum.EPB)
            {

                citys.Add(CityService.Get(user.CityId));

            }
            else
            {
                citys = CityService.GetAll().Select(e => new CityModel
                {
                    City = e.City,
                    Id = e.Id,
                }).ToList();
            }
            ViewBag.Citys = citys;

            return View(new WaterCheckDetailModel {WaterCheckId = waterCheckId });
        }
        [HttpPost]
        public ActionResult Create(WaterCheckDetailModel model)
        {
            WaterCheckDetailService.Create(GetUserBrief(),model);
            var waterCheck = WaterCheckService.Get(model.WaterCheckId);
            return RedirectToAction("Index", waterCheck);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = WaterCheckDetailService.Delete(id);
            return JsonResult(result);
        }

        public ActionResult Report(int? diasterId, int? waterDivisionId)
        {
            List<DiasterModel> diasters = DiasterService.GetAll();

            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            ViewBag.WaterDivisionId = waterDivisionId;
            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;
            ViewBag.WaterDivisions = WaterDivisionService.GetWaterDivisions();
            if (diasterId.HasValue == false)
            {
                return View(new List<WaterCheckDetailModel>());
            }

            List<WaterCheckDetailViewModel> result =
                WaterCheckDetailService.GetListByDiasterId(diasterId.Value, waterDivisionId, GetUserBrief());


            return View(result);
        }

        public ActionResult DownReport(int? diasterId, string file, int? waterDivisionId)
        {
            List<DiasterModel> diasters = DiasterService.GetAll();

            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }

            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;

            if (diasterId.HasValue == false)
            {
                return View(new List<WaterCheckDetailModel>());
            }

            List<WaterCheckDetailViewModel> result =
                WaterCheckDetailService.GetListByDiasterId(diasterId.Value, waterDivisionId, GetUserBrief());

            List<string> ignoreFields = new List<string>();

            ignoreFields.Add(nameof(WaterCheckDetailViewModel.TownId));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.CityId));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.Id));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.WaterCheckId));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.TownName));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.ChlorineStand));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.ChlorineWay));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.EColiType));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.EColiStand));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.EColiWay));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.HydrogenStand));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.HydrogenWay));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.TurbidityStand));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.TurbidityWay));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.OtherWay));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.Other2Name));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.Other2Value));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.Other2Way));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.Other3Value));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.Other3Way));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.GpsX));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.GpsY));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.Status));
            ignoreFields.Add(nameof(WaterCheckDetailViewModel.Recheck));



            if (file == "PDF")
            {
                return File(GeneratePDF(result, "水質抽檢通報表", ignoreFields));
            }
            return File(GenerateODS(result, "水質抽檢通報表", ignoreFields));
        }
    }
}
