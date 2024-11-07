using iTextSharp.text;
using MyEPA.Enums;
using MyEPA.EPA.Attribute;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MyEPA.Controllers
{
    public class OpenContractController : LoginBaseController
    {
        OpenContractService OpenContractService = new OpenContractService();
        ResourceTypeService ResourceTypeService = new ResourceTypeService();
        CityService CityService = new CityService();
        TownService TownService = new TownService();
        public ActionResult Index(int? type, int? id = null)
        {
            var types = ResourceTypeService.GetList();
            if (type.HasValue == false)
            {
                type = types.FirstOrDefault().Id;
            }

            OpenContractFilterParameter filter = new OpenContractFilterParameter 
            {
                ResourceTypeIds = type.Value.ToListCollection(),
            };
            var user = GetUserBrief();
            var duty = user.Duty;

            switch(duty)
            {
                case DutyEnum.EPA:
                case DutyEnum.Corps:
                case DutyEnum.Team:
                    break;
                default:
                    filter.CityIds = user.CityId.ToListCollection();
                    break;
            }

            var result = OpenContractService.GetListByFilter(filter);

            //排序
            result = result.OrderByDescending(a => a.CreateDate).ToList();

            if (id.HasValue == false && result.Any())
            {
                id = result.FirstOrDefault().Id;
            }

            //細目
            OpenContractDetailService OpenContractDetailService = new OpenContractDetailService();
            var details = OpenContractDetailService.GetList2(id.Value);

            ViewBag.Type = type;
            ViewBag.Id = id;
            ViewBag.DetailCount = details.Count;
            ViewBag.Types = types;
            return View(result);
        }

        public ActionResult Create(int? type)
        {
            if(type.HasValue == false)
            {
                return RedirectToIndex();
            }
            return View(new OpenContractModel() 
            {
                OContractDateBegin = DateTimeHelper.GetCurrentTime(),
                OContractDateEnd = DateTimeHelper.GetCurrentTime(),
                KeyInDate = DateTimeHelper.GetCurrentTime(),
            });
        }
        [HttpPost]
        public ActionResult Create(OpenContractModel model, HttpPostedFileBase file)
        {
            OpenContractService.Create(GetUserBrief(),model, file);
            return RedirectToIndex();
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToIndex();
            }
            var result = OpenContractService.Get(id.Value);
            if (result == null)
            {
                return RedirectToIndex();
            }
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(string submitButton, OpenContractViewModel model, HttpPostedFileBase file)
        {
            int type = model.ResourceTypeId;

            if (submitButton == "Copy")
            {
                //複製來源主約Id
                return CopyOpenContractById(model.Id);
            }

            OpenContractService.Update(GetUserBrief() ,model, file);
            
            return RedirectToAction("index", new { type = type });
        }

        /// <summary>
        /// 複製來源主約Id
        /// </summary>
        /// <param name="copyId"></param>
        /// <returns></returns>
        public ActionResult CopyOpenContractById(int copyId)
        {
            var user = GetUserBrief();

            //建置並取得主約Id
            var id = OpenContractService.CopyOpenContractById(user, copyId);

            //細目
            OpenContractDetailService OpenContractDetailService = new OpenContractDetailService();
            var details = OpenContractDetailService.GetList2(copyId);
            foreach (var detail in details)
            {
                detail.OpenContractId = id;
                OpenContractDetailService.Create(GetUserName(), detail);
            }

            ViewBag.CopyIdNew = id;
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = OpenContractService.Delete(GetUserBrief(),id);
            return JsonResult(result);
        }

        public ActionResult Search(int? year,int? cityId,int? townId,int? typeId,bool? isEffective)
        {
            ViewBag.Citys = CityService.GetAll();
            ViewBag.Towns = TownService.GetAll();
            ViewBag.Types = ResourceTypeService.GetList();

            ViewBag.CityId = cityId;
            ViewBag.TownId = townId;
            ViewBag.TypeId = typeId;
            ViewBag.IsEffective = isEffective;
            ViewBag.Year = year;

            var result = OpenContractService
                .Search(new OpenContractFilterParameter
                {
                    CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : null,
                    ResourceTypeIds = typeId.HasValue ? typeId.Value.ToListCollection() : null,
                    Year = year,
                    TownIds = townId.HasValue ? townId.Value.ToListCollection() : null,
                    IsEffective = isEffective,
                });
                
            return View(result);
        }

        private RedirectToRouteResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}