using MyEPA.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualDutyController : LoginBaseController
    {
        public ActionResult GetSelectListItem(ContactManualDutyEnum? duty)
        {
            List<ContactManualDutyEnum> dutys = new List<ContactManualDutyEnum>();
            dutys.Add(ContactManualDutyEnum.Administrator);
            dutys.Add(ContactManualDutyEnum.Business);
            dutys.Add(ContactManualDutyEnum.User);

            var result = dutys.Select(d => new SelectListItem
             {
                 Text = $"{d.GetDescription()}",
                 Value = d.ToIntegerString(),
                 Selected = duty == d
             }).ToList();

            result.Insert(0, new SelectListItem
            {
                Text = "請選擇",
                Value = null
            });

            return PartialView(result);
        }
    }
}
