using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ApplySubsidyController : ApplyBaseController<ApplySubsidyModel, ApplySubsidyViewModel>
    {
        public ApplySubsidyController()
        {
            BaseApplyService = new ApplySubsidyService();
        }

        [HttpGet]
        public override ActionResult Create(ApplyRequestViewModel requestViewModel)
        {
            ViewBag.Options = Options();
            return base.Create(requestViewModel);
        }

        [HttpPost]
        public ActionResult CreateModel(ApplySubsidyModel model, List<ApplySubsidyDetailModel> details, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            var user = GetUserBrief();

            model.Details = details;

            BaseApplyService.Create(user, model, file, requestViewModel);

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        public override ActionResult Edit(int id, ApplyRequestViewModel requestViewModel)
        {
            ViewBag.Options = Options();
            ViewBag.OptionDetails = GetTypeDetails();
            return base.Edit(id, requestViewModel);
        }

        [HttpPost]
        public ActionResult EditModel(ApplySubsidyModel model, List<ApplySubsidyDetailModel> details, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            var user = GetUserBrief();

            model.Details = details;

            BaseApplyService.Edit(user, model, file);
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
        protected override Dictionary<string, int> GetTypes()
        {
            var dictionary = new Dictionary<string, int>()
            {
                { "請選擇",0 }
            };

            foreach (var enumObj in ExtensionsOfEnum.GetEnumAllValue<ApplySubsidyHandlingSituationTypeEnum>())
            {
                if (enumObj == 0)
                {
                    continue;
                }
                dictionary.Add(enumObj.GetDescription(), enumObj.ToInteger());
            }

            return dictionary;
        }
        private static Dictionary<string, int> Options()
        {
            var dictionary = new Dictionary<string, int>()
            {
                { "請選擇",0 }
            };


            foreach (ApplySubsidyTypeEnum enumObj in Enum.GetValues(typeof(ApplySubsidyTypeEnum)))
            {
                if (enumObj == 0)
                {
                    continue;
                }
                dictionary.Add(enumObj.GetDescription(), enumObj.ToInteger());
            }

            return dictionary;
        }

        [HttpGet]
        public ActionResult TypeDetails(int typeId)
        {
            return JsonResult(GetTypeDetails(typeId));
        }


        private List<ApplySubsidyTypeDetailModel> GetTypeDetails()
        {
            return ((ApplySubsidyService)BaseApplyService).GetTypeDetails();
        }

        private List<ApplySubsidyTypeDetailModel> GetTypeDetails(int typeId)
        {
            return ((ApplySubsidyService)BaseApplyService).GetTypeDetails(typeId);
        }
    }
}