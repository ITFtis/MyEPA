using MyEPA.Services;
using System.Linq;
using System.Web.Mvc;
namespace MyEPA.Controllers
{
    public class PositionController : LoginBaseController
    {
        PositionService PositionService = new PositionService();
        public ActionResult GetSelectListItem(int? id)
        {
            return GetPositionSelectListItem(id);
        }

        public ActionResult GetSearchSelectListItem(int? id)
        {
            return GetPositionSelectListItem(id);
        }
        private ActionResult GetPositionSelectListItem(int? id)
        {
            var result = PositionService.GetAll().OrderBy(e => e.Rank).Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Id.ToString(),
                Selected = id == e.Id
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
