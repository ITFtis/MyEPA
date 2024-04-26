using DocumentFormat.OpenXml.Office2010.Excel;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers.Rec
{
    public class RecResourceSetController : LoginBaseController
    {
        RecResourceSetService RecResourceSetService = new RecResourceSetService();
        DiasterService DiasterService = new DiasterService();        
        CityService CityService = new CityService();
        RecResourceRepository RecResourceRepository = new RecResourceRepository();

        // GET: RecResourceSet
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 調度需求Id
        /// </summary>
        /// <param name="RecResourceId"></param>
        /// <returns></returns>
        public ActionResult List(int RecResourceId = 0)
        {
            //調度配置
            int type = 3;

            var entity = RecResourceRepository.Get(RecResourceId);

            if (entity == null)
            {
                RedirectToAction("Index", "RecResource");
            }

            string diasterName = DiasterService.GetByFilter(new DiasterFilterParameter
            {
                Ids = entity.DiasterId.ToListCollection()
            })
                .Select(e => e.DiasterName).FirstOrDefault();

            ViewBag.DiasterName = diasterName;
            ViewBag.Citys = GetCitys();
            RecResourceSetModel Model = new RecResourceSetModel(entity);

            //////querystring
            ViewBag.Type = type;
            ViewBag.DiasterId = entity.DiasterId;

            return View(Model);
        }

        [HttpPost]
        public ActionResult List(int type, int diasterId, RecResourceSetModel model)
        {
            RecResourceSetService.Create(GetUserBrief(), model);
            return RedirectToAction("Index", "RecResource", new { type = 3, diasterId = diasterId });
        }

        private List<CityModel> GetCitys()
        {
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

            return citys;
        }
    }
}