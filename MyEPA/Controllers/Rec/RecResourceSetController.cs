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
                
        // GET: RecResourceSet
        ////////public ActionResult Index()
        ////////{
        ////////    return View();
        ////////}

        /// <summary>
        /// 調度需求Id
        /// </summary>
        /// <param name="recResourceId"></param>
        /// <returns></returns>
        public ActionResult List(int type, int diasterId, int recResourceId = 0)
        {
            //調度配置
            string diasterName = DiasterService.GetByFilter(new DiasterFilterParameter
            {
                Ids = diasterId.ToListCollection()
            })
                .Select(e => e.DiasterName).FirstOrDefault();

            RecResourceService RecResourceService = new RecResourceService();
            ViewBag.RecResource = RecResourceService.Get(recResourceId);
            ViewBag.DiasterName = diasterName;
            ViewBag.Citys = SysFunc.GetCitysRecResource(GetUserBrief());
            ViewBag.RecResourceId = recResourceId;

            //RecResourceSetModel Model = new RecResourceSetModel(entity);
            IEnumerable<RecResourceSetModel> iquery = RecResourceSetService.GetByRecResourceId(recResourceId);
            iquery = iquery.OrderByDescending(a => a.Id);
            List<RecResourceSetModel> result = iquery.ToList();

            //////querystring
            ViewBag.Type = type;
            ViewBag.DiasterId = diasterId;

            return View(result);
        }

        [HttpPost]
        public ActionResult List(RecResourceSetModel model)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(int type, int diasterId, RecResourceSetModel model)
        {
            RecResourceSetService.Create(GetUserBrief(), model);
            return RedirectToAction("List", new { type = 3, diasterId = diasterId, recResourceId = model.RecResourceId });
        }

        [HttpPost]
        public ActionResult Get(int id)
        {
            var result = RecResourceSetService.Get(id);

            return JsonResult(result);
        }

        public ActionResult Edit(int type, int diasterId, RecResourceSetModel model)
        {            
            RecResourceSetService.Update(GetUserBrief(), model);

            var result = RecResourceSetService.Get(model.Id);

            return RedirectToAction("List", new { type = type, diasterId = diasterId, recResourceId = result.RecResourceId });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {            
            AdminResultModel result = RecResourceSetService.Delete(id);
            return JsonResult(result);
        }        
    }
}