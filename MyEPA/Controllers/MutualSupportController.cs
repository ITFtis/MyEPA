using MyEPA.Enums;
using MyEPA.EPA.Attribute;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class MutualSupportController : LoginBaseController
    {
        MutualSupportService MutualSupportService = new MutualSupportService();
        ResourceTypeService ResourceTypeService = new ResourceTypeService();
        public ActionResult Index(int? type)
        {
            var types = ResourceTypeService.GetList();
            if (type.HasValue == false)
            {
                type = types.FirstOrDefault().Id;
            }
            var result = MutualSupportService.GetByFilter(new MutualSupportFilterParameter 
            {
                ResourceTypeIds = type.HasValue ? type.Value.ToListCollection() : new List<int>()
            });
            ViewBag.Type = type;
            ViewBag.Types = types;
            return View(result);
        }
        public ActionResult Create(int? type)
        {
            if (type.HasValue == false)
            {
                return RedirectToIndex();
            }
            ViewBag.SupportTypes =
               ExtensionsOfEnum.GetEnumAllValue<SupportTypeEnum>()
               .ToDictionary(e => (int)e, e => e.GetDescription());
            return View(new MutualSupportModel()
            {
                ResourceTypeId = type.Value,
                StartDate = DateTimeHelper.GetCurrentTime(),
                EndDate = DateTimeHelper.GetCurrentTime(),
            });
        }
        [HttpPost]
        public ActionResult Create(MutualSupportModel model, HttpPostedFileBase file)
        {
            MutualSupportService.Create(GetUserBrief(), model, file);
            return RedirectToIndex();
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.SupportTypes =
                ExtensionsOfEnum.GetEnumAllValue<SupportTypeEnum>()
                .ToDictionary(e => (int)e, e => e.GetDescription());
            if (id.HasValue == false)
            {
                return RedirectToIndex();
            }
            var result = MutualSupportService.Get(id.Value);
            if (result == null)
            {
                return RedirectToIndex();
            }
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(MutualSupportViewModel model, HttpPostedFileBase file)
        {
            MutualSupportService.Update(GetUserBrief(), model, file);
            return RedirectToIndex();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = MutualSupportService.Delete(id);
            return JsonResult(result);
        }

        public ActionResult Search(MutualSupportSearchFilterViewModel filter)
        {
            ViewBag.Types = ResourceTypeService.GetList();
            ViewBag.TypeId = filter.ResourceTypeId;
            ViewBag.Year = filter.Year;
            ViewBag.SupportType = filter.SupportType;
            ViewBag.SupportTypes = 
                ExtensionsOfEnum.GetEnumAllValue<SupportTypeEnum>()
                .ToDictionary(e => (int)e, e => e.GetDescription());
            ViewBag.AcceptSection = filter.AcceptSection;
            ViewBag.Section = filter.Section;

            var result = MutualSupportService
                .Search(new MutualSupportFilterParameter
                {
                    ResourceTypeIds = filter.ResourceTypeId.HasValue ? filter.ResourceTypeId.Value.ToListCollection() : null,
                    Year = filter.Year,
                    AcceptSection = filter.AcceptSection,
                    Section = filter.Section,
                    SupportType = filter.SupportType.HasValue ? filter.SupportType.Value.ToListCollection() : null,
                });

            return View(result);
        }

        private RedirectToRouteResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}