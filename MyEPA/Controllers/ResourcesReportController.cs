using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ResourcesReportController : LoginBaseController
    {
        ResourcesReportService ReportService = new ResourcesReportService();
        public ActionResult Index(string orderBy = null,bool isDesc = false)
        {
            var AAA = ReportService.GetResourcesCityReport(orderBy, isDesc);
            ViewBag.IsDesc = isDesc;
            return View(ReportService.GetResourcesCityReport(orderBy,isDesc));
        }

        public ActionResult Town(int cityId, string orderBy = null, bool isDesc = false)
        {
            return View(ReportService.GetResourcesTownReport(cityId, orderBy, isDesc));
        }

        public ActionResult DownPDF(string orderBy = null, bool isDesc = false)
        {
            var model = ReportService.GetResourcesCityReport(orderBy, isDesc);

           
            return File(GeneratePDF(model, "資源通報", GetIgnoreFields()));
        }
        private List<string> GetIgnoreFields()
        {
            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("CityId");
            ignoreFields.Add("VehicleUpdateTime");
            ignoreFields.Add("UsersConfirmTime");
            ignoreFields.Add("VehicleConfirmTime");
            ignoreFields.Add("VolunteerUpdateTime");
            ignoreFields.Add("VolunteerConfirmTime");
            ignoreFields.Add("ToiletUpdateTime");
            ignoreFields.Add("ToiletConfirmTime");
            ignoreFields.Add("DumpUpdateTime");
            ignoreFields.Add("DumpConfirmTime");
            ignoreFields.Add("PestUpdateTime");
            ignoreFields.Add("PestConfirmTime");
            ignoreFields.Add("DisinfectantUpdateTime");
            ignoreFields.Add("DisinfectantConfirmTime");
            ignoreFields.Add("DisinfectorUpdateTime");
            ignoreFields.Add("DisinfectorConfirmTime");
            ignoreFields.Add("UsersUpdateTime");

            return ignoreFields;
        }
        public ActionResult DownODS(string orderBy = null, bool isDesc = false)
        {
            var model = ReportService.GetResourcesCityReport(orderBy, isDesc);

            return File(GenerateODS(model, "資源通報", GetIgnoreFields()));
        }
    }
}