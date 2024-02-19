using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Models.SearchViewModel;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class NoticeController : LoginBaseController
    {
        NoticeService NoticeService = new NoticeService();
        DiasterService DiasterService = new DiasterService();

        public ActionResult Index(int? diasterId = null)
        {
            List<DiasterModel> diasters = DiasterService.GetAll();

            if (diasterId.HasValue == false)
            {
                diasterId = -1;
            }

            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;

            if (diasterId.HasValue == false)
            {
                return View(new List<NoticeModel>());
            }

            var result = NoticeService.GetByDiasterId(diasterId.Value);

            return View(result);
        }

        public ActionResult Create(int diasterId)
        {
            DiasterModel diaster = 
                DiasterService
                .GetByFilter(
                    new DiasterFilterParameter 
                    { 
                        Ids = diasterId.ToListCollection() 
                    })
                .FirstOrDefault();

            ViewBag.Diaster = diaster;

            return View(new NoticeModel 
            { 
                DiasterId = diasterId,
            });
        }

        [HttpPost]
        public ActionResult Create(NoticeModel model)
        {
            NoticeService.Create(GetUserBrief(),model);
            return RedirectToIndex(model.DiasterId);
        }

        public ActionResult Edit(int id)
        {
            var result = NoticeService.Get(id);

            DiasterModel diaster =
                DiasterService
                .GetByFilter(
                    new DiasterFilterParameter
                    {
                        Ids = result.DiasterId.ToListCollection()
                    })
                .FirstOrDefault();

            ViewBag.Diaster = diaster;

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(NoticeModel model)
        {
            NoticeService.Update(GetUserBrief(), model);
            return RedirectToIndex(model.DiasterId);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = NoticeService.Delete(id);
            return JsonResult(result);
        }

        public ActionResult Search(NoticeSearchViewModel search)
        {
            List<DiasterModel> diasters = DiasterService.GetAll();

            if (search.DiasterId.HasValue == false)
            {
                search.DiasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            if (search.StartDate.HasValue == false)
            {
                search.StartDate = DateTimeHelper.GetCurrentTime().AddMonths(-1).Date;
            }
            if (search.EndDate.HasValue == false)
            {
                search.EndDate = DateTimeHelper.GetCurrentTime().Date;
            }
            ViewBag.Search = search;
            ViewBag.Diasters = diasters;

            if (search.DiasterId.HasValue == false)
            {
                return View(new List<NoticeModel>());
            }

            var result =
                NoticeService.GetByFilter(new NoticeFilterParameter
                {
                    DiasterIds = search.DiasterId.Value.ToListCollection(),
                    StartDate = search.StartDate.HasValue ? search.StartDate.Value.Date : default(DateTime?),
                    EndDate = search.EndDate.HasValue ? search.EndDate.Value.To_23_59_59() : default(DateTime?),
                    Keyword = search.keyword
                });
            return View(result);
        }
        private RedirectToRouteResult RedirectToIndex(int diasterId)
        {
            return RedirectToAction("Index",new { diasterId });
        }
    }
}