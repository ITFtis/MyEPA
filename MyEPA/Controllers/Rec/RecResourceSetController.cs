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
        RecResourceService RecResourceService = new RecResourceService();
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
            ViewBag.RecResourceNeed = RecResourceService.Get(recResourceId);

            ViewBag.DiasterName = diasterName;
            ViewBag.Citys = SysFunc.GetCitysRecResource(GetUserBrief());
            ViewBag.RecResourceId = recResourceId;

            //(可提供資源為主表)設定數量(input)
            IEnumerable<RecResourceModel> iquery = RecResourceService.GetByDiasterId(diasterId);
            iquery = iquery.Where(a => a.Type == 2).OrderByDescending(a => a.Id);
            List<RecResourceModel> helps = iquery.ToList();

            //主表：Copy helps
            List<RecResourceViewModel> result = RecResourceViewModel.Copy(2, helps);

            //已調度
            List<RecResourceSetModel> sets = RecResourceSetService.GetByRecResourceIdNeed(recResourceId);

            //關聯已調度數量
            foreach (var r in result)
            {
                var v = sets.Where(a => a.RecResourceIdHelp == r.RecResourceIdHelp).FirstOrDefault();
                if (v != null)
                    r.SetQuantity = v.SetQuantity;
            }

            //////querystring
            ViewBag.Type = type;
            ViewBag.DiasterId = diasterId;

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(RecResourceViewModel obj)
        {
            //可提供調度
            var help = RecResourceService.Get(obj.RecResourceIdHelp);
            if (help == null)
            {
                return JsonResult(new AdminResultModel() { IsSuccess = false, ErrorMessage = "取調度資料失敗(RecResourceId)：" + obj.RecResourceIdHelp });
            }

            RecResourceSetModel entity = new RecResourceSetModel();
            var entitys = RecResourceSetService.GetByNeedHelp(obj.RecResourceIdNeed, obj.RecResourceIdHelp);
            if (entitys.Count == 0)
            {
                //新增
                entity = new RecResourceSetModel();
            }
            else
            {
                //修改
                entity = entitys.First();
            }

            entity.RecResourceIdNeed = obj.RecResourceIdNeed;
            entity.RecResourceIdHelp = obj.RecResourceIdHelp;
            entity.SetQuantity = obj.SetQuantity;
            entity.SetCityId = help.CityId;
            entity.SetContactPerson = help.ContactPerson;
            entity.SetContactMobilePhone = help.ContactMobilePhone;
            entity.SetItems = help.Items;
            entity.SetSpec = help.Spec;            
            entity.SetUnit = help.Unit;

            if (entitys.Count == 0)
            {
                //新增
                RecResourceSetService.Create(GetUserBrief(), entity);
            }
            else
            {
                //修改
                RecResourceSetService.Update(GetUserBrief(), entity);
            }

            AdminResultModel result = new AdminResultModel() { 
                IsSuccess = true,
            };

            return JsonResult(result);
        }
    }
}