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
    public class NewsController : LoginBaseController
    {
        NewsService NewsService = new NewsService();
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
                return View(new List<NewsModel>());
            }

            var result = NewsService.GetByDiasterId(diasterId.Value).OrderByDescending(e => e.CreateDate);

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

            return View(new NewsModel 
            { 
                DiasterId = diasterId,
            });
        }

        [HttpPost]
        public ActionResult Create(NewsModel model)
        {
            NewsService.Create(GetUserBrief(),model);
            return RedirectToIndex(model.DiasterId);
        }

        public ActionResult Edit(int id)
        {
            var result = NewsService.Get(id);

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
        public ActionResult Edit(NewsModel model)
        {
            NewsService.Update(GetUserBrief(), model);
            return RedirectToIndex(model.DiasterId);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = NewsService.Delete(id);
            return JsonResult(result);
        }

        public ActionResult Search(NewsSearchViewModel search)
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
                NewsService.GetByFilter(new NewsFilterParameter
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