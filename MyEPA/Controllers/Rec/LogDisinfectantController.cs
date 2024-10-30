using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
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
        public ActionResult Index(int? diasterId = null,
                                    int? selectDDLCt = null, int? selectServiceLifeDay = null)
        {
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

            //(閥值)正確災害編號，舊資料使用預設
            int YDiasterId = LogDisinfectantService.GetYDiasterId((int)diasterId);
            LogDisinfectantFilterParameter filter = new LogDisinfectantFilterParameter()
            {
                DiasterIds = YDiasterId.ToListCollection(),
                Ct = selectDDLCt,
                ServiceLifeDay = selectServiceLifeDay,
            };

            IEnumerable<LogDisinfectantViewModel> iquery = LogDisinfectantService.GetLogDisinfectantCurrentByFilter(filter);
            iquery = iquery.OrderByDescending(a => a.Id);
            List<LogDisinfectantViewModel> result = iquery.ToList();

            if (YDiasterId != LogDisinfectantService.iniDiasterId)
            {
                //可更新閥值
                ViewBag.CanUpdate = true;
            }
            else
            {
                //不可更新閥值
                ViewBag.CanUpdate = false;
            }

            //querystring
            ViewBag.DiasterId = diasterId;
            ViewBag.selectDDLCt = selectDDLCt;
            ViewBag.selectServiceLifeDay = selectServiceLifeDay;

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