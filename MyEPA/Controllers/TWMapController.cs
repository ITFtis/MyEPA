using MyEPA.Models.TWMapModels;
using MyEPA.Services;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class TWMapController : Controller
    {
        TWMapService TWMapService = new TWMapService();
        public ActionResult Coordinate(string searchWord)
        {
            var model = TWMapService.Coordinate(searchWord);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}