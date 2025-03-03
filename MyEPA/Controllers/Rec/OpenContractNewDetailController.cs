using System;
using System.Collections.Generic;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Models.Deds;

namespace MyEPA.Controllers.Rec
{
    public class OpenContractNewDetailController : LoginBaseController
    {
        OpenContractService OpenContractService = new OpenContractService();
        OpenContractDetailService OpenContractDetailService = new OpenContractDetailService();
        OpenContractDetailItemCategoryService OpenContractDetailItemCategoryService = new OpenContractDetailItemCategoryService();

        FileDataService FileDataService = new FileDataService();

        // GET: OpenContractNewDetail
        public ActionResult Index(int openContractId)
        {
            var result = OpenContractDetailService.GetList(openContractId);

            //排序
            result = result.OrderByDescending(a => a.CreateDate)
                        .ToList();

            var openContract = OpenContractService.Get(openContractId);
            var user = GetUserBrief();
            ViewBag.OpenContract = openContract;
            ViewBag.CanEdit = OpenContractService.CheckPermissions(user, openContract.CityId, openContract.TownId);

            return View(result);
        }

        public ActionResult Create(int openContractId)
        {
            ViewBag.OpenContractDetailItemCategorys = OpenContractDetailItemCategoryService.GetAll();

            //預設值
            var result = new OpenContractDetailModel()
            {
                OpenContractId = openContractId,
                Count = "0",
                Price = "0",
            };

            return View(result);
        }
        [HttpPost]
        public ActionResult Create(OpenContractDetailModel model, HttpPostedFileBase file)
        {
            OpenContractDetailService.Create(GetUserBrief(), model, GetUploadFiles());
            return RedirectToOpenContract(model.OpenContractId);
        }

        /// <summary>
        /// Create (下一步流程)
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateNext()
        {
            var openContract = Session["OpenContract"];
            if (openContract == null)
            {
                TempData["Msg"] = "新增失敗：無合約資料，請新增合約";
                return RedirectToAction("Index", "OpenContractNew");
            }

            ViewBag.OpenContractDetailItemCategorys = OpenContractDetailItemCategoryService.GetAll();

            //預設值
            var result = new OpenContractDetailModel()
            {
                OpenContractId = 0,
                Count = "0",
                Price = "0",
            };

            return View(result);
        }

        /// <summary>
        /// Create (下一步流程)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateNext(OpenContractDetailModel model, HttpPostedFileBase file)
        {
            //合約(第一步)
            OpenContractModel main = (OpenContractModel)Session["OpenContract"];
            OpenContractService.Create(GetUserBrief(), main, GetUploadFiles());

            Session["OpenContract"] = null;

            //明細(第二步)
            model.OpenContractId = main.Id;
            OpenContractDetailService.Create(GetUserBrief(), model, GetUploadFiles());
            return RedirectToOpenContract(model.OpenContractId);
        }

        public ActionResult Edit(int id)
        {
            var result = OpenContractDetailService.Get(id);
            if (result == null)
            {
                return RedirectToOpenContract(result.OpenContractId);
            }

            ViewBag.OpenContractDetailItemCategorys = OpenContractDetailItemCategoryService.GetAll();

            ViewBag.DetailFiles = FileDataService.GetBySource(SourceTypeEnum.OpenContractDetail, id);

            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(OpenContractDetailModel model)
        {
            OpenContractDetailService.Update(GetUserBrief(), model, GetUploadFiles());

            var result = OpenContractDetailService.Get(model.Id);
            return RedirectToOpenContract(result.OpenContractId);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = OpenContractDetailService.Delete(id);
            return JsonResult(result);
        }

        private RedirectToRouteResult RedirectToOpenContract(int openContractId)
        {
            return RedirectToAction("Index", "OpenContractNewDetail", new { openContractId = openContractId });
        }
    }
}