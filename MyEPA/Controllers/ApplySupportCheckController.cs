using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ApplySupportCheckController: LoginBaseController
    {
        DiasterService DiasterService = new DiasterService();
        ApplySupportService ApplySupportService = new ApplySupportService();
        TownService TownService = new TownService();
        public ActionResult Index(int diasterId = 0)
        {            
            var diasters = DiasterService.GetAll();
            ViewBag.Diasters = diasters;
            ViewBag.DiasterId = diasterId == 0 && diasters.Any() ? diasters.FirstOrDefault().Id : diasterId;

            var userBrief = GetUserBrief();
            var viewModel = ApplySupportService.GetCheckViewModel(userBrief, diasterId);

            return View(viewModel);
        }

        public ActionResult Processing(int diasterId = 0, int? townId = null, int applyTypeId = 0)
        {
            var userBrief = GetUserBrief();
            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = DiasterService.GetAll();
            ViewBag.TownId = townId;
            ViewBag.Towns = TownService.GetByCityId(userBrief.CityId);
            ViewBag.ApplyTypeId = applyTypeId;
            ViewBag.ApplyTypes = GetTypes();

            var viewModel = ApplySupportService.ProcessingViewModel(userBrief, diasterId, townId, applyTypeId);

            //取 mapping 所需資訊
            if (viewModel.ApplyCar != null && viewModel.ApplyCar.Any()) 
            {
                ViewBag.CarDic = GetCarDictionary();
            }

            if (viewModel.ApplyMedicine != null && viewModel.ApplyMedicine.Any()) 
            {
                ViewBag.MedicineDic = GetMedicineDictionary();
            }

            if (viewModel.ApplySubsidy != null && viewModel.ApplySubsidy.Any()) 
            {
                ViewBag.SubsidyTypeDic = GetSubsidyTypeDictionary();
                ViewBag.SubsidyTypeDetails = GetSubsidyTypeDetailDictionary();
            }

            return View(viewModel);
        }

        public ActionResult EPAConfirm(ApplySupportConfirmViewModel confirm)
        {
            var userBrief = GetUserBrief();
            AdminResultModel result = ApplySupportService.EPAConfirm(userBrief, confirm);
            return JsonResult(result);
        }
        public ActionResult EPBConfirm(ApplySupportConfirmViewModel confirm)
        {
            var userBrief = GetUserBrief();
            AdminResultModel result = ApplySupportService.EPBConfirm(userBrief, confirm);
            return JsonResult(result);
        }
        public ActionResult EPACancel(ApplySupportCancelViewModel cancel)
        {
            var userBrief = GetUserBrief();
            AdminResultModel result = ApplySupportService.EPACancel(userBrief,cancel);
            return JsonResult(result);
        }
        public ActionResult EPBCancel(ApplySupportCancelViewModel cancel)
        {
            var userBrief = GetUserBrief();
            AdminResultModel result = ApplySupportService.EPBCancel(userBrief, cancel);
            return JsonResult(result);
        }
        public ActionResult EPAProcessing(int diasterId = 0, ApplyStatusEnum? status = null, int? townId = null, int? applyTypeId = null)
        {
            var userBrief = GetUserBrief();
            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = DiasterService.GetAll();
            ViewBag.TownId = townId;
            ViewBag.Towns = TownService.GetAll();

            ViewBag.ApplyTypeId = applyTypeId;
            ViewBag.ApplyTypes = GetTypes();

            var viewModel = ApplySupportService.EPAProcessingViewModel(userBrief, diasterId, townId, applyTypeId, status);

            //取 mapping 所需資訊
            if (viewModel.ApplyCar != null && viewModel.ApplyCar.Any())
            {
                ViewBag.CarDic = GetCarDictionary();
            }

            if (viewModel.ApplyMedicine != null && viewModel.ApplyMedicine.Any())
            {
                ViewBag.MedicineDic = GetMedicineDictionary();
            }

            if (viewModel.ApplySubsidy != null && viewModel.ApplySubsidy.Any())
            {
                ViewBag.SubsidyTypeDic = GetSubsidyTypeDictionary();
                ViewBag.SubsidyTypeDetails = GetSubsidyTypeDetailDictionary();
            }

            return View(viewModel);
        }
        private Dictionary<string, int> GetTypes()
        {
            var dictionary = new Dictionary<string, int>();
            foreach (ApplyTypeEnum enumObj in Enum.GetValues(typeof(ApplyTypeEnum)))
            {
                dictionary.Add(enumObj.GetDescription(), enumObj.ToInteger());
            }

            return dictionary;
        }

        private Dictionary<string, int> GetApplyStatus()
        {
            var dictionary = new Dictionary<string, int>();

            foreach (ApplyStatusEnum enumObj in Enum.GetValues(typeof(ApplyStatusEnum)))
            {
                if (enumObj == ApplyStatusEnum.SendToEpa)
                {
                    continue;
                }
                dictionary.Add(enumObj.GetDescription(), enumObj.ToInteger());
            }



            return dictionary;
        }

        public List<TownModel> GetTownsByCityId(int cityId)
        {
            return TownService.GetByCityId(cityId);
        }

        public static Dictionary<string, int> GetCarDictionary()
        {
            var dictionary = new Dictionary<string, int>();
            var types = new ApplyCarService().GetTypes();
            foreach (var type in types)
            {
                dictionary.Add(type.DisplayName, type.Id);
            }

            return dictionary;
        }

        public static Dictionary<string, int> GetMedicineDictionary()
        {
            var dictionary = new Dictionary<string, int>();
            foreach (ApplyMedicineTypeEnum enumObj in Enum.GetValues(typeof(ApplyMedicineTypeEnum)))
            {
                dictionary.Add(enumObj.GetDescription(), enumObj.ToInteger());
            }

            return dictionary;
        }

        public static Dictionary<string, int> GetSubsidyTypeDictionary()
        {
            var dictionary = new Dictionary<string, int>();
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

        public static List<ApplySubsidyTypeDetailModel> GetSubsidyTypeDetailDictionary()
        {
            var types = new ApplySubsidyService().GetTypeDetails();

            return types;
        }
    }
}