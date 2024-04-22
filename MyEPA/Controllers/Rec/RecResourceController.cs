using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class RecResourceController : LoginBaseController
    {
        DiasterService DiasterService = new DiasterService();
        RecResourceService RecResourceService = new RecResourceService();
        CityService CityService = new CityService();

        // GET: RecResource
        public ActionResult Index(int? diasterId = null)
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
                return View(new List<RecResourceModel>());
            }

            ViewBag.Citys = CityService.GetAll();

            List<RecResourceModel> result =
                RecResourceService.GetByDiasterId(diasterId.Value, GetUserBrief());

            ViewBag.DiasterId = diasterId;

            return View(result);
        }

        public ActionResult Create(int type = 0, int diasterId = 0)
        {
            if (type == 0 || diasterId == 0)
            {                
                return RedirectToAction("Index");
            }

            string diasterName = DiasterService.GetByFilter(new DiasterFilterParameter
            {
                Ids = diasterId.ToListCollection()
            })
                .Select(e => e.DiasterName).FirstOrDefault();

            ViewBag.DiasterId = diasterId;
            ViewBag.DiasterName = diasterName;

            UserBriefModel user = GetUserBrief();
            List<CityModel> citys = new List<CityModel>();
            if (!user.IsAdmin)
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

            ViewBag.Type = type;

            return View();
        }

        [HttpPost]
        public ActionResult Create(RecResourceModel model)
        {
            RecResourceService.Create(GetUserBrief(), model);
            return RedirectToAction("Index", new { diasterId = model.DiasterId });
        }
    }
}