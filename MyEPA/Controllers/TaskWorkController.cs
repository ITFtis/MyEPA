using iTextSharp.text;
using MyEPA.EPA.Attribute;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MyEPA.Controllers
{
    public class TaskWorkController : LoginBaseController
    {
        TaskWorkService TaskWorkService = new TaskWorkService();
        public ActionResult Index()
        {
            var result = TaskWorkService.GetAll();
            ViewBag.Msg = TempData["Msg"];
            TempData["Msg"] = string.Empty;

            return View(result);
        }

        public ActionResult Create()
        {
            return View(new TaskWorkModel() 
            {
                PhoneTime = DateTimeHelper.GetCurrentTime()
            });
        }
        [HttpPost]
        public ActionResult Create(TaskWorkModel model)
        {
            TaskWorkService.Create(GetUserBrief(),model);
            TempData["Msg"] = "新增成功";
            return RedirectToIndex();
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToIndex();
            }
            var result = TaskWorkService.Get(id.Value);
            if (result == null)
            {
                return RedirectToIndex();
            }
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(TaskWorkViewModel model)
        {
            TaskWorkService.Update(GetUserBrief() ,model);
            TempData["Msg"] = "修改成功";
            return RedirectToIndex();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = TaskWorkService.Delete(id);
            return JsonResult(result);
        }

        private RedirectToRouteResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}