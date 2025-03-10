using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using System.IO;

namespace MyEPA.Controllers.Rec
{
    public class OpenContractNewController : LoginBaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        OpenContractService OpenContractService = new OpenContractService();
        ResourceTypeService ResourceTypeService = new ResourceTypeService();
        CityService CityService = new CityService();
        TownService TownService = new TownService();

        FileDataService FileDataService = new FileDataService();

        // GET: OpenContractNew
        public ActionResult Index(string submitButton, int? cityId, int? townId, int? resourceTypeId, int? yearRange)
        {            
            bool isEffective = Request.QueryString["isEffective"] == null ? false : bool.Parse(Request.QueryString["isEffective"].ToString());
            bool isEPB = Request.QueryString["isEPB"] == null ? false : bool.Parse(Request.QueryString["isEPB"].ToString());

            OpenContractFilterParameter filter = new OpenContractFilterParameter
            {
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : null,
                TownIds = townId.HasValue ? townId.Value.ToListCollection() : null,
                ResourceTypeIds = resourceTypeId.HasValue ? resourceTypeId.Value.ToListCollection() : null,
                YearRange = yearRange,
                IsEffective = isEffective,
                IsEPB = isEPB,
            };

            //編輯或其他的錯誤訊息
            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"];
            }

            var user = GetUserBrief();
            var duty = user.Duty;

            switch (duty)
            {
                case DutyEnum.EPA:
                case DutyEnum.Corps:
                case DutyEnum.Team:
                    break;
                default:
                    filter.CityIds = user.CityId.ToListCollection();
                    break;
            }

            var result = OpenContractService.GetCountListByFilter(filter);

            //是否有編輯權限
            result.ForEach(p => p.CanEdit = OpenContractService.CheckPermissions(user, p.CityId, p.TownId));

            //排序
            result = result.OrderByDescending(a => a.CanEdit)
                        .ThenByDescending(a => a.CreateDate)
                        .ToList();

            if (submitButton == "ExportList")
            {
                //匯出Excel
                if (result.Count == 0)
                {
                    ViewBag.Msg = "無資料匯出";
                }
                else
                {                    
                    string toPath = ExportList(result);
                    if (toPath != "")
                    {
                        //讀成串流
                        var iStream = new FileStream(toPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        //回傳出檔案
                        return File(iStream, GetContentType("xlsx"), Path.GetFileName(toPath));
                    }
                    else
                    {
                        ViewBag.Msg = "執行失敗：匯出合約清單";
                    }
                }
            }
            
            ViewBag.Types = ResourceTypeService.GetList();
            ViewBag.Citys = CityService.GetCitysF1(user);
            //ViewBag.Towns = TownService.GetAll();

            //querystring
            ViewBag.YearRange = yearRange;
            ViewBag.ResourceTypeId = resourceTypeId;
            ViewBag.CityId = cityId;
            ViewBag.TownId = townId;
            ViewBag.IsEffective = isEffective;
            ViewBag.IsEPB = isEPB;
            ViewBag.User = user;

            return View(result);
        }

        public ActionResult Create()
        {
            var types = ResourceTypeService.GetList();

            ViewBag.Types = ResourceTypeService.GetList();

            return View(new OpenContractModel()
            {
                OContractDateBegin = DateTimeHelper.GetCurrentTime(),
                OContractDateEnd = DateTimeHelper.GetCurrentTime(),
                KeyInDate = DateTimeHelper.GetCurrentTime(),
            });
        }

        ////[HttpPost]
        ////public ActionResult Create(OpenContractModel model, HttpPostedFileBase file)
        ////{
        ////    OpenContractService.Create(GetUserBrief(), model, GetUploadFiles());
        ////    return RedirectToIndex();
        ////}

        //(主表)合約下一步
        [HttpPost]
        public ActionResult Next(OpenContractNextModel model)
        {
            Session["OpenContract"] = null;

            model.Files = GetUploadFiles();

            Session["OpenContract"] = model;
            return RedirectToAction("CreateNext", "OpenContractNewDetail");
        }

        public ActionResult Edit(int id)
        {
            var types = ResourceTypeService.GetList();
            var user = GetUserBrief();

            //////Copy或其他的錯誤訊息
            ////if (TempData["Msg"] != null)
            ////{
            ////    ViewBag.Msg = TempData["Msg"];
            ////}

            var result = OpenContractService.Get(id);
            if (result == null)
            {
                return RedirectToIndex();
            }

            ViewBag.Types = ResourceTypeService.GetList();
            ViewBag.User = user;
            ViewBag.CanEdit = OpenContractService.CheckPermissions(user, result.CityId, result.TownId);

            ViewBag.CoverFiles = FileDataService.GetBySource(SourceTypeEnum.OpenContractCover, id);
            ViewBag.SealFiles = FileDataService.GetBySource(SourceTypeEnum.OpenContractSeal, id);            
            ViewBag.DetailFiles = FileDataService.GetBySource(SourceTypeEnum.OpenContractDetail, id);

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(OpenContractModel model, HttpPostedFileBase file)
        {
            bool done = OpenContractService.Update(GetUserBrief(), model, GetUploadFiles());
            if(!done)
            {
                TempData["Msg"] = OpenContractService.ErrorMessage;
            }

            return RedirectToAction("index");
        }
        
        /// <summary>
        /// 複製來源主約Id
        /// </summary>
        /// <param name="copyId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Copy(int copyId)
        {
            var user = GetUserBrief();

            try
            {
                //建置並取得主約Id
                var id = OpenContractService.CopyOpenContractById(user, copyId);

                //細目
                OpenContractDetailService OpenContractDetailService = new OpenContractDetailService();
                var details = OpenContractDetailService.GetList2(copyId);
                foreach (var detail in details)
                {
                    detail.OpenContractId = id;
                    OpenContractDetailService.Create(GetUserBrief(), detail, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error("複製來源主約錯誤(copyId)：" + copyId);
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);

                return Json(new { result = false, errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = OpenContractService.Delete(GetUserBrief(), id);
            return JsonResult(result);
        }

        /// <summary>
        /// 匯出合約清單
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string ExportList(List<OpenContractCountModel> datas)
        {
            string result = "";

            try
            {
                string fileTitle = "開口合約清單";
                string folder = Server.MapPath("~/FileDatas/Temp/");

                //產出Dynamic資料 (給Excel)
                List<dynamic> list = new List<dynamic>();

                int serial = 1;
                foreach (var data in datas)
                {
                    dynamic f = new ExpandoObject();
                    f.序號 = serial;
                    serial++;                    
                    f.承辦縣市 = data.CityName;
                    f.承辦鄉鎮 = data.TownName;
                    f.合約種類 = data.ResourceTypeName;
                    f.合約名稱 = data.Name;
                    f.簽約日期 = DateFormat.ToDate4(data.KeyInDate);
                    f.合約起始 = DateFormat.ToDate4(data.OContractDateBegin);
                    f.合約截止 = DateFormat.ToDate4(data.OContractDateEnd);
                    f.合約廠商 = data.Fac;
                    f.負責人 = data.Owner;
                    f.聯絡電話 = data.TEL;
                    f.行動電話 = data.MobileTEL;
                    f.跨縣市支援 = data.IsSupportCity == true ? "是" : "否";                    

                    f.SheetName = fileTitle;//sheep.名稱;
                    list.Add(f);
                }

                //查無符合資料表數
                if (list.Count == 0)
                {                    
                    return "";
                }

                List<string> titles = new List<string>();

                //"0":不調整width,"1":自動調整長度(效能差:資料量多),"2":字串長度調整width,"3":字串長度調整width(展開)
                int autoSizeColumn = 2;

                //產出excel
                string fileName = ExcelSpecHelper.GenerateExcelByLinqF1(fileTitle, titles, list, folder, autoSizeColumn);

                string path = folder + fileName;

                result = path;
            }
            catch (Exception ex)
            {
                logger.Error("執行錯誤：匯出Excel_開口合約");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        private RedirectToRouteResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}