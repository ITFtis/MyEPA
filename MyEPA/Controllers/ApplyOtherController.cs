using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ApplyOtherController : ApplyBaseController<ApplyOtherModel, ApplyOtherViewModel>
    {
        public ApplyOtherController()
        {
            BaseApplyService = new ApplyOtherService();
        }
        [HttpPost]
        public ActionResult CreateModel(ApplyOtherModel model, List<ApplyOtherDetailModel> details, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            var user = GetUserBrief();
            model.Details = details;
            BaseApplyService.Create(user, model, file, requestViewModel);

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult EditModel(ApplyOtherModel model, List<ApplyOtherDetailModel> details, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
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

            foreach (ApplyOtherSubsidyTypeEnum enumObj in Enum.GetValues(typeof(ApplyOtherSubsidyTypeEnum)))
            {
                if (enumObj == 0)
                {
                    continue;
                }
                dictionary.Add(enumObj.GetDescription(), enumObj.ToInteger());
            }

            return dictionary;
        }
    }
}