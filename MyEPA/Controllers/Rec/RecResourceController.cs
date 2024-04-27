using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Services;
using MyEPA.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace MyEPA.Controllers
{
    public class RecResourceController : LoginBaseController
    {
        RecResourceService RecResourceService = new RecResourceService();
        DiasterService DiasterService = new DiasterService();        
        CityService CityService = new CityService();

        // GET: RecResource
        public ActionResult Index(int? type, int? diasterId = null)
        {
            if (type.HasValue == false)
            {
                type = 1;
            }

            List<DiasterModel> diasters = DiasterService.GetAll();

            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            
            ViewBag.Diasters = diasters;

            if (diasterId.HasValue == false)
            {
                return View(new List<RecResourceModel>());
            }

            IEnumerable<RecResourceModel> iquery = RecResourceService.GetByDiasterId(diasterId.Value);

            //調度需求(改自己，全部看)
            //提供需求(改自己，)
            //var user = GetUserBrief();
            //if (!user.IsAdmin)
            //{
            //    iquery = iquery.Where(a => a.CityId == user.CityId);
            //}

            ////iquery = iquery.Where(a => a.Type == type);
            iquery = iquery.OrderByDescending(a => a.Id);
            List<RecResourceModel> result = iquery.ToList();

            ViewBag.Citys = CityService.GetAll();
            ViewBag.IsAdmin = GetIsAdmin();            
            ViewBag.User = GetUserBrief();

            //querystring
            ViewBag.DiasterId = diasterId;
            ViewBag.Type = type;

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
            
            ViewBag.DiasterName = diasterName;            
            ViewBag.Citys = GetCitys();

            //querystring
            ViewBag.Type = type;
            ViewBag.DiasterId = diasterId;

            return View();
        }

        [HttpPost]
        public ActionResult Create(int type, RecResourceModel model)
        {
            RecResourceService.Create(GetUserBrief(), model);
            return RedirectToAction("Index", new { type = model.Type, diasterId = model.DiasterId });
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index");
            }

            var result = RecResourceService.Get(id.Value);
            if (result == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Citys = GetCitys();

            //querystring
            ViewBag.Type = result.Type;

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(RecResourceModel model)
        {
            RecResourceService.Update(GetUserBrief(), model);

            //返回原有tab
            var entity = RecResourceService.Get(model.Id);

            return RedirectToAction("Index", new { type = entity.Type, diasterId = entity.DiasterId });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = RecResourceService.Delete(id);
            return JsonResult(result);
        }

        [HttpPost]
        public ActionResult Finish(int id)
        {
            AdminResultModel result = RecResourceService.Finish(id);
            return JsonResult(result);
        }

        public ActionResult DownPDF(int Id)
        {
            //我要下載的檔案位置
            string filepath = Server.MapPath("~/FileDatas/環境部災害應變資源調度原則v3.docx");
            //取得檔案名稱
            string filename = System.IO.Path.GetFileName(filepath);
            //讀成串流
            Stream iStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
            //回傳出檔案
            return File(iStream, GetContentType("docx"), filename);
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