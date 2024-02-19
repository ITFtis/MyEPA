using MyEPA.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class DutyController : LoginBaseController
    {
        public ActionResult GetSearchSelectListItem(DutyEnum? selectItem)
        {
            List<DutyEnum> dutys = new List<DutyEnum>();
            dutys.Add(DutyEnum.EPA);
            dutys.Add(DutyEnum.EPB);
            dutys.Add(DutyEnum.Water);
            dutys.Add(DutyEnum.Cleaning);

            var result = dutys.Select(duty => new SelectListItem
            {
                Text = duty.GetDescription(),
                Value = duty.ToIntegerString(),
                Selected = selectItem == duty
            }).ToList();

            result.Insert(0, new SelectListItem
            {
                Text = "請選擇",
                Value = null
            });

            return PartialView(result);
        }

        public ActionResult GetSelectListItem(DutyEnum? selectItem)
        {
            List<DutyEnum> dutys = new List<DutyEnum>();
            dutys.Add(DutyEnum.EPA);
            dutys.Add(DutyEnum.EPB);
            dutys.Add(DutyEnum.Water);
            dutys.Add(DutyEnum.Cleaning);

            var result = dutys.Select(duty => new SelectListItem
            {
                Text = duty.GetDescription(),
                Value = duty.ToIntegerString(),
                Selected = selectItem == duty
            }).ToList();

            result.Insert(0, new SelectListItem
            {
                Text = "請選擇",
                Value = ""
            });

            return PartialView(result);
        }
    }
}