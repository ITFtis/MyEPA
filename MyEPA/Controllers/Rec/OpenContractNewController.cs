using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers.Rec
{
    public class OpenContractNewController : LoginBaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        OpenContractService OpenContractService = new OpenContractService();
        ResourceTypeService ResourceTypeService = new ResourceTypeService();
        CityService CityService = new CityService();
        TownService TownService = new TownService();

        FileDataService FileDataService = new FileDataService();

        // GET: OpenContractNew
        public ActionResult Index(int? cityId, int? townId, int? resourceTypeId, int? yearRange)
        {            
            bool isEffective = Request.QueryString["isEffective"] == null ? false : bool.Parse(Request.QueryString["isEffective"].ToString());
            bool isEPB = Request.QueryString["isEPB"] == null ? false : bool.Parse(Request.QueryString["isEPB"].ToString());

            OpenContractFilterParameter filter = new OpenContractFilterParameter
            {
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : null,
                TownIds = townId.HasValue ? townId.Value.ToListCollection() : null,
                ResourceTypeIds = resourceTypeId.HasValue ? resourceTypeId.Value.ToListCollection() : null,
                YearRange = yearRange,
                IsEffective = isEffective,
                IsEPB = isEPB,
            };

            //編輯或其他的錯誤訊息
            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"];
            }

            var user = GetUserBrief();
            var duty = user.Duty;

            switch (duty)
            {
                case DutyEnum.EPA:
                case DutyEnum.Corps:
                case DutyEnum.Team:
                    break;
                default:
                    filter.CityIds = user.CityId.ToListCollection();
                    break;
            }

            var result = OpenContractService.GetCountListByFilter(filter);

            //是否有編輯權限
            result.ForEach(p => p.CanEdit = OpenContractService.CheckPermissions(user, p.CityId, p.TownId));

            //排序
            result = result.OrderByDescending(a => a.CanEdit)
                        .ThenByDescending(a => a.CreateDate)
                        .ToList();

            ViewBag.Types = ResourceTypeService.GetList();
            ViewBag.Citys = CityService.GetCitysF1(user);
            //ViewBag.Towns = TownService.GetAll();

            //querystring
            ViewBag.YearRange = yearRange;
            ViewBag.ResourceTypeId = resourceTypeId;
            ViewBag.CityId = cityId;
            ViewBag.TownId = townId;
            ViewBag.IsEffective = isEffective;
            ViewBag.IsEPB = isEPB;
            ViewBag.User = user;

            return View(result);
        }

        public ActionResult Create()
        {
            var types = ResourceTypeService.GetList();

            ViewBag.Types = ResourceTypeService.GetList();

            return View(new OpenContractModel()
            {
                OContractDateBegin = DateTimeHelper.GetCurrentTime(),
                OContractDateEnd = DateTimeHelper.GetCurrentTime(),
                KeyInDate = DateTimeHelper.GetCurrentTime(),
            });
        }

        ////[HttpPost]
        ////public ActionResult Create(OpenContractModel model, HttpPostedFileBase file)
        ////{
        ////    OpenContractService.Create(GetUserBrief(), model, GetUploadFiles());
        ////    return RedirectToIndex();
        ////}

        //(主表)合約下一步
        [HttpPost]
        public ActionResult Next(OpenContractNextModel model)
        {
            Session["OpenContract"] = null;

            model.Files = GetUploadFiles();

            Session["OpenContract"] = model;
            return RedirectToAction("CreateNext", "OpenContractNewDetail");
        }

        public ActionResult Edit(int id)
        {
            var types = ResourceTypeService.GetList();
            var user = GetUserBrief();

            //Copy或其他的錯誤訊息
            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"];
            }

            var result = OpenContractService.Get(id);
            if (result == null)
            {
                return RedirectToIndex();
            }

            ViewBag.Types = ResourceTypeService.GetList();
            ViewBag.User = user;
            ViewBag.CanEdit = OpenContractService.CheckPermissions(user, result.CityId, result.TownId);

            ViewBag.CoverFiles = FileDataService.GetBySource(SourceTypeEnum.OpenContractCover, id);
            ViewBag.SealFiles = FileDataService.GetBySource(SourceTypeEnum.OpenContractSeal, id);            
            ViewBag.DetailFiles = FileDataService.GetBySource(SourceTypeEnum.OpenContractDetail, id);

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(OpenContractModel model, HttpPostedFileBase file)
        {
            bool done = OpenContractService.Update(GetUserBrief(), model, GetUploadFiles());
            if(!done)
            {
                TempData["Msg"] = OpenContractService.ErrorMessage;
            }

            return RedirectToAction("index");
        }
        
        /// <summary>
        /// 複製來源主約Id
        /// </summary>
        /// <param name="copyId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Copy(int copyId)
        {
            var user = GetUserBrief();

            try
            {
                //建置並取得主約Id
                var id = OpenContractService.CopyOpenContractById(user, copyId);

                //細目
                OpenContractDetailService OpenContractDetailService = new OpenContractDetailService();
                var details = OpenContractDetailService.GetList2(copyId);
                foreach (var detail in details)
                {
                    detail.OpenContractId = id;
                    OpenContractDetailService.Create(GetUserBrief(), detail, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error("複製來源主約錯誤(copyId)：" + copyId);
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);

                return Json(new { result = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = OpenContractService.Delete(GetUserBrief(), id);
            return JsonResult(result);
        }        

        private RedirectToRouteResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}