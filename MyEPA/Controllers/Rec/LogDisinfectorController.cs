using MyEPA.Models;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers.Rec
{
    public class LogDisinfectorController : LoginBaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        LogDisinfectorService LogDisinfectorService = new LogDisinfectorService();
        DiasterService DiasterService = new DiasterService();

        // GET: LogDisinfector
        public ActionResult Index(int? type, int? diasterId = null)
        {
            if (type.HasValue == false)
            {
                type = 1;
            }

            List<DiasterModel> diasters = DiasterService.GetAll();

            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }

            ViewBag.Diasters = diasters;

            if (diasterId.HasValue == false)
            {
                return View(new List<RecResourceModel>());
            }

            IEnumerable<LogDisinfectorModel> iquery = LogDisinfectorService.GetByDiasterId(diasterId.Value);
            iquery = iquery.OrderByDescending(a => a.Id);

            List<LogDisinfectorModel> result = iquery.ToList();

            //querystring
            ViewBag.DiasterId = diasterId;

            return View(result);
        }
    }
}