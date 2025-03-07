using System;
using System.Collections.Generic;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Models.Deds;
using System.IO;
using System.Dynamic;

namespace MyEPA.Controllers.Rec
{
    public class OpenContractNewDetailController : LoginBaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        OpenContractService OpenContractService = new OpenContractService();
        OpenContractDetailService OpenContractDetailService = new OpenContractDetailService();
        OpenContractDetailItemCategoryService OpenContractDetailItemCategoryService = new OpenContractDetailItemCategoryService();

        FileDataService FileDataService = new FileDataService();

        // GET: OpenContractNewDetail
        public ActionResult Index(string submitButton, int openContractId)
        {
            var datas = OpenContractDetailService.GetList(openContractId);

            //排序
            datas = datas.OrderByDescending(a => a.CreateDate)
                        .ToList();

            var result = datas;

            var openContract = OpenContractService.Get(openContractId);
            var user = GetUserBrief();

            if (submitButton == "ExportDetailList")
            {
                //匯出Excel
                if (result.Count == 0)
                {
                    ViewBag.Msg = "無資料匯出";
                }
                else
                {
                    string toPath = ExportDetailList(openContract.Name, result);
                    if (toPath != "")
                    {
                        //讀成串流
                        var iStream = new FileStream(toPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        //回傳出檔案
                        return File(iStream, GetContentType("xlsx"), Path.GetFileName(toPath));
                    }
                    else
                    {
                        ViewBag.Msg = "執行失敗：匯出合約細目清單";
                    }
                }
            }
            
            ViewBag.OpenContract = openContract;
            ViewBag.CanEdit = OpenContractService.CheckPermissions(user, openContract.CityId, openContract.TownId);

            return View(result);
        }

        public ActionResult Create(int openContractId)
        {
            ViewBag.OpenContractDetailItemCategorys = OpenContractDetailItemCategoryService.GetAll();

            //預設值
            var result = new OpenContractDetailModel()
            {
                OpenContractId = openContractId,
                Count = "0",
                Price = "0",
            };

            return View(result);
        }
        [HttpPost]
        public ActionResult Create(OpenContractDetailModel model, HttpPostedFileBase file)
        {
            OpenContractDetailService.Create(GetUserBrief(), model, GetUploadFiles());
            return RedirectToOpenContract(model.OpenContractId);
        }

        /// <summary>
        /// Create (下一步流程)
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateNext()
        {
            var openContract = Session["OpenContract"];
            if (openContract == null)
            {
                TempData["Msg"] = "新增失敗：無合約資料，請新增合約";
                return RedirectToAction("Index", "OpenContractNew");
            }

            ViewBag.OpenContractDetailItemCategorys = OpenContractDetailItemCategoryService.GetAll();

            //預設值
            var result = new OpenContractDetailModel()
            {
                OpenContractId = 0,
                Count = "0",
                Price = "0",
            };

            return View(result);
        }

        /// <summary>
        /// Create (下一步流程)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateNext(OpenContractDetailModel model, HttpPostedFileBase file)
        {
            //合約(第一步)
            OpenContractNextModel main = (OpenContractNextModel)Session["OpenContract"];            
            OpenContractService.Create(GetUserBrief(), main, main.Files);

            Session["OpenContract"] = null;

            //明細(第二步)
            model.OpenContractId = main.Id;
            OpenContractDetailService.Create(GetUserBrief(), model, GetUploadFiles());
            return RedirectToOpenContract(model.OpenContractId);
        }

        public ActionResult Edit(int id)
        {
            var result = OpenContractDetailService.Get(id);
            if (result == null)
            {
                return RedirectToOpenContract(result.OpenContractId);
            }

            ViewBag.OpenContractDetailItemCategorys = OpenContractDetailItemCategoryService.GetAll();

            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(OpenContractDetailModel model)
        {
            OpenContractDetailService.Update(GetUserBrief(), model, GetUploadFiles());

            var result = OpenContractDetailService.Get(model.Id);
            return RedirectToOpenContract(result.OpenContractId);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = OpenContractDetailService.Delete(id);
            return JsonResult(result);
        }

        /// <summary>
        /// 匯出合約細目清單
        /// </summary>
        /// <param name="name">合約名稱</param>
        /// <param name="datas">細目</param>
        /// <returns></returns>
        public string ExportDetailList(string name, List<OpenContractDetailViewModel> datas)
        {
            string result = "";

            try
            {
                string fileTitle = "合約細目清單";
                string folder = Server.MapPath("~/FileDatas/Temp/");

                //產出Dynamic資料 (給Excel)
                List<dynamic> list = new List<dynamic>();

                int serial = 1;
                foreach (var data in datas)
                {
                    dynamic f = new ExpandoObject();
                    f.序號 = serial;
                    serial++;
                    f.合約名稱 = name;
                    f.項目 = data.Items;
                    f.單位 = data.Unit;
                    f.數量 = data.Count;
                    f.單價 = data.Price;
                    f.金額 = data.Budge;

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
                logger.Error("執行錯誤：匯出Excel_合約細目");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        private RedirectToRouteResult RedirectToOpenContract(int openContractId)
        {
            return RedirectToAction("Index", "OpenContractNewDetail", new { openContractId = openContractId });
        }
    }
}