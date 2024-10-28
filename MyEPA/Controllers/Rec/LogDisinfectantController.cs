using MyEPA.Models;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers.Rec
{
    public class LogDisinfectantController : LoginBaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        LogDisinfectantService LogDisinfectantService = new LogDisinfectantService();
        DiasterService DiasterService = new DiasterService();

        // GET: LogDisinfectant
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

            IEnumerable<LogDisinfectantModel> iquery = LogDisinfectantService.GetByDiasterId(diasterId.Value);
            iquery = iquery.OrderByDescending(a => a.Id);

            List<LogDisinfectantModel> result = iquery.ToList();

            if (result.Count == 0)
            {
                //閥值預設值
                iquery = LogDisinfectantService.GetByDiasterId(LogDisinfectantService.iniDiasterId);
                iquery = iquery.OrderByDescending(a => a.Id);
                result = iquery.ToList();

                //是否可更新閥值
                ViewBag.CanUpdate = false;
            }
            else
            {
                //是否可更新閥值
                ViewBag.CanUpdate = true;
            }

            //querystring
            ViewBag.DiasterId = diasterId;

            return View(result);
        }

        public ActionResult Update(int diasterId)
        {
            AdminResultModel result = null;

            if (diasterId == 0)
            {
                result = new AdminResultModel() { IsSuccess = false, ErrorMessage = "diasterId不可為0" + diasterId };
                return JsonResult(result);
            }

            try
            {
                //閥值資料建置(LogDisinfectantModel)
                LogDisinfectantService LogDisinfectantService = new LogDisinfectantService();

                //刪除
                LogDisinfectantService.DeleteByDiasterId(diasterId);

                //新增
                var user = GetUserBrief();
                LogDisinfectantService.Create(user, diasterId);

                result = new AdminResultModel() { IsSuccess = true };
            }
            catch (Exception ex)
            {
                result = new AdminResultModel() { IsSuccess = false, ErrorMessage = ex.Message };
            }

            return JsonResult(result);
        }
    }
}