using MyEPA.EPA.Attribute;
using MyEPA.Models;
using MyEPA.Services;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class OpenContractDetailController : LoginBaseController
    {
        OpenContractService OpenContractService = new OpenContractService();
        OpenContractDetailService OpenContractDetailService = new OpenContractDetailService();
        OpenContractDetailItemCategoryService OpenContractDetailItemCategoryService = new OpenContractDetailItemCategoryService();
        public ActionResult Index(int? openContractId = null)
        {
            if(openContractId.HasValue == false)
            {
                return RedirectToOpenContract();
            }
            ViewBag.OpenContract = OpenContractService.Get(openContractId.Value);
            var result = OpenContractDetailService.GetList(openContractId.Value);
            return View(result);
        }

        public ActionResult Create(int openContractId)
        {
            ViewBag.OpenContractDetailItemCategorys = OpenContractDetailItemCategoryService.GetAll();
            return View(new OpenContractDetailModel() 
            {
                OpenContractId = openContractId
            });
        }
        [HttpPost]
        public ActionResult Create(OpenContractDetailModel model)
        {
            OpenContractDetailService.Create(GetUserBrief(), model, GetUploadFiles());
            return RedirectToOpenContract(model.OpenContractId);
        }

        public ActionResult Edit(int openContractId, int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToOpenContract(openContractId);
            }
            var result = OpenContractDetailService.Get(id.Value);
            if (result == null)
            {
                return RedirectToOpenContract(openContractId);
            }
            ViewBag.OpenContractDetailItemCategorys = OpenContractDetailItemCategoryService.GetAll();
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(OpenContractDetailModel model)
        {
            OpenContractDetailService.Update(GetUserBrief(), model, GetUploadFiles());
            return RedirectToOpenContract(model.OpenContractId);
        }

        [HttpPost]
        public ActionResult Delete(int openContractId, int id)
        {
            AdminResultModel result = OpenContractDetailService.Delete(id);
            return JsonResult(result);
        }

        private RedirectToRouteResult RedirectToOpenContract(int? openContractId = null)
        {
            return RedirectToAction("Index", "OpenContract", new { id = openContractId });
        }
    }
}