using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Repositories;
using MyEPA.Services;

namespace MyEPA.Controllers
{
    public class EPAxDiasterController : LoginBaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        DiasterRepository DiasterRepository = new DiasterRepository();
        DiasterBLModel Diaster = new DiasterBLModel();
        bool[] CityCover = new bool[23] {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false };               
        public ActionResult A9x1()
        {
            ViewBag.Data = DiasterRepository.GetList().OrderByDescending(e => e.Id).ToList();
            return View("~/Views/EPA/A9x1.cshtml");
        }

        public ActionResult A9x2Set()
        {
            string DiasterNameForSet = Request["DiasterNameForSet"];
            CityCover = Diaster.GetCityCover(DiasterNameForSet);
            ViewBag.DiasterName = DiasterNameForSet;
            ViewBag.CityCover = CityCover;
            return View("~/Views/EPA/A9x2Set.cshtml");
        }

        public ActionResult A9x2SetCoverCity()
        {    
            string DiasterName = Request["DiasterName"];
            string CoverCity = "0";
            //沒勾選時，傳回的字串是false；有勾選時卻傳回 true,false 
            //因此以false 來判斷是否勾選
            if ((Request["City001"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City002"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; } else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City003"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; } else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City004"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; } else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City005"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City006"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City007"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City008"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }

            if ((Request["City009"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City010"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City011"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City012"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }

            if ((Request["City013"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City014"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City015"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City016"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }

            if ((Request["City017"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City018"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City019"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City020"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }

            if ((Request["City021"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }
            if ((Request["City022"]).ToString() == "false")
            { CoverCity = CoverCity + "0"; }
            else { { CoverCity = CoverCity + "1"; } }
            string Msg = Diaster.SetCityCover(DiasterName,CoverCity);
            ViewBag.Msg = Msg;
            ViewBag.Data = DiasterRepository.GetList();
            return View("~/Views/EPA/A9x2.cshtml");
        }


        public ActionResult A9x2Browse()
        {
            ViewBag.Data = DiasterRepository.GetList();
            string DiasterNameForBrowse = Request["DiasterNameForBrowse"];
            CityCover = Diaster.GetCityCover(DiasterNameForBrowse);
            ViewBag.CityCover = CityCover;
            return View("~/Views/EPA/A9x2Browse.cshtml");
        }

        public ActionResult A9x2()
        {
            ViewBag.CityCover = CityCover;
            ViewBag.Data = DiasterRepository.GetList().OrderByDescending(e=>e.Id).ToList();
            return View("~/Views/EPA/A9x2.cshtml");      
        }

        public ActionResult Add(DiasterModel diaster)
        {
            diaster.DiasterState = "災害關閉";
            diaster.Status = NormalActiveStatusEnum.Inactive.ToInteger();
            int id = DiasterRepository.CreateAndResultIdentity<int>(diaster);

            //災害id
            var user = GetUserBrief();
            if (id > 0)
            {
                try
                {
                    //(閥值)消毒設備資料建置(LogDisinfectorModel)
                    LogDisinfectorService LogDisinfectorService = new LogDisinfectorService();
                    LogDisinfectorService.Create(user, id);

                    //(閥值)消毒藥劑資料建置(LogDisinfectantModel)
                    LogDisinfectantService LogDisinfectantService = new LogDisinfectantService();
                    LogDisinfectantService.Create(user, id);
                }
                catch (Exception ex)
                {
                    logger.Error("閥值資料建置錯誤");
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                }
            }

            return RedirectToAction("A9x1", "EPAxDiaster");
        }

        public ActionResult Edit()
        {
            DiasterRepository repository = new DiasterRepository();

            string Id = Request["EditingId"];

            DiasterModel diaster = repository.Get(Id);

            if(diaster != null)
            {
                string diasterName = Request["EditingDiasterName"];
                string diasterType = Request["EditingDiasterType"];
                string isValidDefend = Request["EditingIsValidDefend"];
                string startTime = Request["EditingStartTime"];
                string endTime = Request["EditingEndTime"];
                string comment = Request["EditingComment"];
                string diasterState = Request["EditingDiasterState"];
                diaster.DiasterName = diasterName;
                diaster.DiasterType = diasterType;
                diaster.IsValidDefend = isValidDefend.TryToInt().Value;
                diaster.StartTime = startTime.TryToDateTime().Value;
                diaster.EndTime = endTime.TryToDateTime().Value;
                diaster.Comment = comment;
                diaster.DiasterState = diasterState;
                diaster.Status = diasterState == "災害啟動" ? 1 : 0;
                repository.Update(diaster);
            }

            ViewBag.Data = DiasterRepository.GetList();
            return RedirectToAction("A9x1", "EPAxDiaster");
            //return View("~/Views/EPA/A9x1.cshtml");
        }

    }
}