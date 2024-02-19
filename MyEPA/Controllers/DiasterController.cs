using MyEPA.Services;
using System.Linq;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class DiasterController : LoginBaseController
    {
        DiasterService DiasterService = new DiasterService();
        public ActionResult GetSelectListItem(int? id = null)
        {
            var result = DiasterService.GetAll().Select(e => new SelectListItem
            {
                Text = e.DiasterName,
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