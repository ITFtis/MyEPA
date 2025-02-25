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

namespace MyEPA.Controllers.Rec
{
    public class OpenContractNewDetailController : LoginBaseController
    {
        OpenContractService OpenContractService = new OpenContractService();
        OpenContractDetailService OpenContractDetailService = new OpenContractDetailService();
        OpenContractDetailItemCategoryService OpenContractDetailItemCategoryService = new OpenContractDetailItemCategoryService();

        // GET: OpenContractNewDetail
        public ActionResult Index(int openContractId)
        {
            ViewBag.OpenContract = OpenContractService.Get(openContractId);

            var result = OpenContractDetailService.GetList(openContractId);

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
        public ActionResult Create(OpenContractDetailModel model)
        {
            OpenContractDetailService.Create(GetUserName(), model);
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
            
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(OpenContractDetailModel model)
        {
            OpenContractDetailService.Update(GetUserName(), model);

            var result = OpenContractDetailService.Get(model.Id);
            return RedirectToOpenContract(result.OpenContractId);
        }

        [HttpPost]
        public ActionResult Delete(int openContractId, int id)
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