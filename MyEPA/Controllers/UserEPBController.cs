using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models.FilterParameter;
using MyEPA.Models.SearchViewModel;
using MyEPA.Services;
using System.Linq;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class UserEPBController : LoginBaseController
    {
        UsersService UsersService = new UsersService();
        public ActionResult GetSelectListItem(UserEPBSearchViewModel searchModel, int? id)
        {
            UsersBriefFilterParameter usersFilter = new UsersBriefFilterParameter
            {
                CityIds = searchModel?.SearchCityId?.ToListCollection(),
                DutyIds = DutyEnum.EPB.ToInteger().ToListCollection(),
            };

            var result = UsersService.GetUsersByFilter(usersFilter).Select(e => new SelectListItem
            {
                Text = $"{e.Name}_{e.City}",
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
