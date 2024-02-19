using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Services;

namespace MyEPA.Controllers
{
    public class CleanerController : LoginBaseController
    {
        TownService TownService = new TownService();
        NoticeService NoticeService = new NoticeService();
        NewsService NewsService = new NewsService();
        DisinfectorService DisinfectorService = new DisinfectorService();
        DisinfectantService DisinfectantService = new DisinfectantService();
        public ActionResult C3x1Users(int? townId = null)
        {
            var user = GetUserBrief();
           
            UsersBriefFilterParameter filter = new UsersBriefFilterParameter();
            switch (_Duty)
            {
                case DutyEnum.Cleaning:
                    filter.DutyIds = DutyEnum.Cleaning.ToInteger().ToListCollection();
                    filter.TownIds = user.TownId.ToListCollection();
                    filter.CityIds = user.CityId.ToListCollection();
                    break;
                case DutyEnum.EPB:
                    filter.CityIds = user.CityId.ToListCollection();
                    if(townId.HasValue)
                    {
                        filter.TownIds = townId.Value.ToListCollection();
                    }
                    ViewBag.Towns = TownService.GetByCityId(user.CityId);
                    ViewBag.TownId = townId;
                    break;
                default:
                    break;
            }
            ViewBag.Data = new UsersService().GetUsersByFilter(filter);
            ViewBag.Positions = new PositionService().GetAll();
            return View("~/Views/Cleaner/C3x1Users.cshtml");
        }

        public ActionResult C1d11(string duty)
        {
            ViewBag.PostList = NoticeService.GetAll();
            ViewBag.Title= "最新公告";
            ViewBag.PostType = "notice";
            ViewBag.City = Session["AuthenticateCity"].ToString().Trim();
            ViewBag.Town = Session["AuthenticateTown"].ToString().Trim();
            return View();
        }

        public ActionResult C1d12(string duty)
        {
            ViewBag.PostList = NewsService.GetAll();
            ViewBag.Title= "新聞報導";
            ViewBag.PostType = "news";
            ViewBag.City = Session["AuthenticateCity"].ToString().Trim();
            ViewBag.Town = Session["AuthenticateTown"].ToString().Trim();
            return View();
        }

        public ActionResult C3x1()
        {
            var user = GetUserBrief();

            ResourcesConfirmUpdateTimeDataFilterParameter filter = new ResourcesConfirmUpdateTimeDataFilterParameter();
            
            switch (_Duty)
            {
                case DutyEnum.Cleaning:
                    filter.CityIds = user.CityId.ToListCollection();
                    filter.TownIds = user.TownId.ToListCollection();
                    break;
                case DutyEnum.EPB:
                    filter.CityIds = user.CityId.ToListCollection();
                    break;
                default:
                    break;
            }
            List<ResourcesConfirmUpdateTimeDataModel> result = new ResourcesReportService().GetResourcesConfirmUpdateTimeDatas(filter);

            var district = new DistrictService().GetByDistrictName($"{user.City}{user.Town}");
         
            if (result is null)
            { }
            else
            {
                ViewBag.ConfirmUpdateTimeData = new ResourcesConfirmUpdateTimeDataModel()
                {
                    DisinfectantConfirmTime = result.Max(e => e.DisinfectantConfirmTime),
                    DisinfectantUpdateTime = result.Max(e => e.DisinfectantUpdateTime),
                    DisinfectorConfirmTime = result.Max(e => e.DisinfectorConfirmTime),
                    DisinfectorUpdateTime = result.Max(e => e.DisinfectorUpdateTime),
                    DumpConfirmTime = result.Max(e => e.DumpConfirmTime),
                    DumpUpdateTime = result.Max(e => e.DumpUpdateTime),
                    IncineratorConfirmTime = result.Max(e => e.IncineratorConfirmTime),
                    IncineratorUpdateTime = result.Max(e => e.IncineratorUpdateTime),
                    LandfillConfirmTime = result.Max(e => e.LandfillConfirmTime),
                    LandfillUpdateTime = result.Max(e => e.LandfillUpdateTime),
                    PestConfirmTime = result.Max(e => e.PestConfirmTime),
                    PestUpdateTime = result.Max(e => e.PestUpdateTime),
                    ToiletConfirmTime = result.Max(e => e.ToiletConfirmTime),
                    ToiletUpdateTime = result.Max(e => e.ToiletUpdateTime),
                    UserConfirmTime = result.Max(e => e.UserConfirmTime),
                    UserUpdateTime = result.Max(e => e.UserUpdateTime),
                    VehicleConfirmTime = result.Max(e => e.VehicleConfirmTime),
                    VehicleUpdateTime = result.Max(e => e.VehicleUpdateTime),
                    VolunteerConfirmTime = result.Max(e => e.VolunteerConfirmTime),
                    VolunteerUpdateTime = result.Max(e => e.VolunteerUpdateTime),
                    DistrictConfirmTime = district?.ConfirmTime,
                    DistrictUpdateTime = district?.UpdateTime
                };
            }
            return PartialView();
        }

        public ActionResult C3x1District(string message = null)
        {
            if (string.IsNullOrWhiteSpace(message) == false)
            {
                ViewBag.Message = message;
            }
            string City = Session["AuthenticateCity"].ToString();
            string Town = Session["AuthenticateTown"].ToString();
            ViewBag.City = City;
            ViewBag.Town = Town;
            string DistrictName = City + Town;

            DistrictModel district = new DistrictService().GetByDistrictName(DistrictName);

            if (district != null)
            {
                ViewBag.Id = district.Id;
                ViewBag.DistrictName = district.DistrictName;
                ViewBag.CoverArea = district.CoverArea;
                ViewBag.Address = district.Address;
                ViewBag.Phone = district.Phone;
                ViewBag.Fax = district.Fax;
                ViewBag.Human = district.Human;
                ViewBag.OutHuman = district.OutHuman;
                ViewBag.ReadyHuman = district.ReadyHuman;
                ViewBag.CleanCapacity = district.CleanCapacity;
            }
            
            return View();
        }

        public ActionResult C3x1Disinfector(int? townId = null)
        {
            var user = GetUserBrief();

            DisinfectorFilterParameter filter = new DisinfectorFilterParameter();
            if(user.Duty == DutyEnum.Cleaning)
            {
                filter.CityIds = user.CityId.ToListCollection();
                filter.TownIds = user.TownId.ToListCollection();
            }
            else if (user.Duty == DutyEnum.EPB)
            {
                filter.CityIds = user.CityId.ToListCollection();
                if (townId.HasValue)
                {
                    filter.TownIds = townId.Value.ToListCollection();
                }
                ViewBag.Towns = TownService.GetByCityId(user.CityId);
                ViewBag.TownId = townId;
            }
            else if(user.Duty == DutyEnum.EPA)
            {
                filter.CityIds = user.CityId.ToListCollection();
            }
            
            var disinfector = DisinfectorService.GetByFilter(filter);

            string Town = Session["AuthenticateTown"].ToString();
            ViewBag.City = user.City;
            ViewBag.Town = user.Town;
            ViewBag.Data = disinfector;
            ViewBag.Msg = string.Empty;
            ViewBag.UseTypes = ExtensionsOfEnum.GetEnumAllValue<DisinfectorUseTypeEnum>();
            ViewBag.Names = ExtensionsOfEnum.GetEnumAllValue<DisinfectorNameEnum>();
            return View();
        }

        public ActionResult C3x1Disinfectant(int? townId = null)
        {
            var user = GetUserBrief();

            DisinfectantFilterParameter filter = new DisinfectantFilterParameter();
            if (user.Duty == DutyEnum.Cleaning)
            {
                filter.CityIds = user.CityId.ToListCollection();
                filter.TownIds = user.TownId.ToListCollection();
            }
            else if (user.Duty == DutyEnum.EPB)
            {
                filter.CityIds = user.CityId.ToListCollection();
                if(townId.HasValue)
                {
                    filter.TownIds = townId.Value.ToListCollection();
                }
                ViewBag.TownId = townId;
                ViewBag.Towns = TownService.GetByCityId(user.CityId);
            }
            else if (user.Duty == DutyEnum.EPA)
            {
                filter.CityIds = user.CityId.ToListCollection();
            }

            var disinfectants = DisinfectantService.GetByFilter(filter);

            ViewBag.City = user.City;
            ViewBag.Town = user.Town;
            ViewBag.Data = disinfectants;
            ViewBag.Msg = string.Empty;
            ViewBag.UseTypes = ExtensionsOfEnum.GetEnumAllValue<DisinfectantUseTypeEnum>();
            ViewBag.Names = ExtensionsOfEnum.GetEnumAllValue<DisinfectantNameEnum>();
            return View();
        }

        public ActionResult C3x1Landfill(string town = null)
        {
            var user = GetUserBrief();

            TownFilterParameter filter = new TownFilterParameter
            {
                CityIds = user.CityId.ToListCollection()
            };

            if (_Duty == DutyEnum.Cleaning)
            {
                town = user.Town;
                filter.Ids = user.TownId.ToListCollection();
            }

            List<LandfillModel> landfills = new LandfillService().GetByFilter(new LandfillFilterParameter
            {
                City = user.City,
                Town = town
            }).ToList();

            ViewBag.Towns = new TownService().GetListByFilter(filter);
            ViewBag.Town = town;
            ViewBag.City = user.City;
            ViewBag.Data = landfills;
            ViewBag.Msg = string.Empty;
            ViewBag.Xpos = landfills.Select(e => e.Xpos).FirstOrDefault();
            ViewBag.Ypos = landfills.Select(e => e.Ypos).FirstOrDefault();
            return View();
        }

        public ActionResult C3x1Incinerator(string town = null)
        {
            var user = GetUserBrief();

            TownFilterParameter filter = new TownFilterParameter
            {
                CityIds = user.CityId.ToListCollection()
            };

            if (_Duty == DutyEnum.Cleaning)
            {
                town = user.Town;
                filter.Ids = user.TownId.ToListCollection();
            }

            List<IncineratorModel> result = new IncineratorService().GetByFilter(new IncineratorFilterParameter
            {
                City = user.City,
                Town = town
            }).ToList();

            ViewBag.Towns = new TownService().GetListByFilter(filter);
            ViewBag.Town = town;
            ViewBag.City = user.City;
            ViewBag.Data = result;
            ViewBag.Msg = string.Empty;
            ViewBag.Xpos = result.Select(e => e.Xpos).FirstOrDefault();
            ViewBag.Ypos = result.Select(e => e.Ypos).FirstOrDefault();
            return View();
        }

        public ActionResult C3x1Pest()
        {
            string City = Session["AuthenticateCity"].ToString();
            string Town = Session["AuthenticateTown"].ToString();
            PestModel Item = new PestModel();
            LinkedList<PestModel> ItemList = new LinkedList<PestModel>();        
            ItemList = Item.Show(City);

            ViewBag.City = City;
            ViewBag.Town = Town;
            ViewBag.Data = ItemList;
            ViewBag.Msg = string.Empty;
            return View("~/Views/Cleaner/C3x1Pest.cshtml");
        }

        public ActionResult C3x1Toilet(int? townId)
        {
            var user = GetUserBrief();
            ToiletFilterParameter filter = new ToiletFilterParameter();
            switch (_Duty)
            {
                case DutyEnum.Cleaning:
                    filter.TownIds = user.TownId.ToListCollection();
                    filter.CityIds = user.CityId.ToListCollection();
                    break;
                case DutyEnum.EPB:
                    filter.CityIds = user.CityId.ToListCollection();
                    if (townId.HasValue)
                    {
                        filter.TownIds = townId.Value.ToListCollection();
                    }
                    ViewBag.Towns = TownService.GetByCityId(user.CityId);
                    ViewBag.TownId = townId;
                    break;
                default:
                    break;
            }
            List<ToiletModel> toilets = new ToiletRepository().GetByFilter(filter);

            ViewBag.City = user.City;
            ViewBag.Town = user.Town;
            ViewBag.Data = toilets;
            ViewBag.Msg = string.Empty;
            ViewBag.TownId = townId;
            ViewBag.Towns = TownService.GetByCityId(user.CityId);
            return View();
        }

        public ActionResult C3x1b()
        {
            return View();
        }

        public ActionResult C3x1Dump(int? townId = null)
        {
            var user = GetUserBrief();

            TownFilterParameter filter = new TownFilterParameter
            {
                CityIds = user.CityId.ToListCollection()
            };

            if (_Duty == DutyEnum.Cleaning)
            {
                townId = GetUserTownId();
                filter.Ids = townId.Value.ToListCollection();
            }

            List<DumpModel> landfills = new DumpService().GetByFilter(new DumpFilterParameter
            {
                CityIds = user.CityId.ToListCollection(),
                TownIds = townId.HasValue ? townId.Value.ToListCollection() : new List<int>()
            }).ToList();


            ViewBag.City = user.City;
            ViewBag.Towns = new TownService().GetListByFilter(filter);
            ViewBag.TownId = townId;

            ViewBag.Data = landfills;
            ViewBag.Msg = string.Empty;
            return View();
        }


        public ActionResult C3x2(string duty)
        {
            if (Convert.ToString(Session["AuthenticateDuty"]) == "清潔隊")
            {
                ViewBag.Duty = "清潔隊";
                ViewBag.Id = Session["AuthenticateId"].ToString();
                return View();
            }
            else { return View("~/Views/Home/Login.cshtml"); }
        }

       

        public ActionResult Cleaner()
        {
            ViewBag.City = Session["AuthenticateCity"].ToString();
            ViewBag.Town = Session["AuthenticateTown"].ToString();
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Message()
        {
            ViewBag.Msg = "Sorry, not yet.";
            return View();
        }

        public ActionResult Report()
        {
            return View();
        }
    }
}