using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Services;
using MyEPA.ViewModels;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.Formula;
using NPOI.XWPF.UserModel;
using RestSharp;
using Spire.Xls.Core.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls.WebParts;
using Xceed.Pdf;
using Xceed.Words.NET;
using Document = Microsoft.Office.Interop.Word.Document;

namespace MyEPA.Controllers
{
    public class RecResourceController : LoginBaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        RecResourceService RecResourceService = new RecResourceService();
        DiasterService DiasterService = new DiasterService();        
        CityService CityService = new CityService();
        RecResourceSetService RecResourceSetService = new RecResourceSetService();

        // GET: RecResource
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

            IEnumerable<RecResourceModel> iquery = RecResourceService.GetByDiasterId(diasterId.Value);
            
            ////iquery = iquery.Where(a => a.Type == type);
            iquery = iquery.OrderByDescending(a => a.Id);
            List<RecResourceModel> result = iquery.ToList();

            //總計已調度
            var IdNeeds = iquery.Where(a => a.Type == 1).Select(a => a.Id).ToList();
            var recs = RecResourceSetService.GetByRecResourceIdNeed(IdNeeds);
            var countIdNeeds = recs.GroupBy(a => a.RecResourceIdNeed)
                                    .Select(a => new
                                    {
                                        Id = a.Key,
                                        Count = a.Sum(b => b.SetQuantity),
                                    }).ToList();            

            var user = GetUserBrief();

            ViewBag.Citys = CityService.GetAll();
            ////環衛組：視為admin
            ViewBag.IsAdmin = user.Town.Trim() == "環境督察總隊".Trim() || GetIsAdmin();
            ViewBag.User = user;
            ViewBag.CountIdNeeds = countIdNeeds;

            //querystring
            ViewBag.DiasterId = diasterId;
            ViewBag.Type = type;

            return View(result);
        }

        public ActionResult Create(int type = 0, int diasterId = 0)
        {
            if (type == 0 || diasterId == 0)
            {                
                return RedirectToAction("Index");
            }

            string diasterName = DiasterService.GetByFilter(new DiasterFilterParameter
            {
                Ids = diasterId.ToListCollection()
            })
                .Select(e => e.DiasterName).FirstOrDefault();
            
            ViewBag.DiasterName = diasterName;
            ViewBag.Citys = SysFunc.GetCitysRecResource(GetUserBrief());

            //querystring
            ViewBag.Type = type;
            ViewBag.DiasterId = diasterId;

            return View();
        }

        [HttpPost]
        public ActionResult Create(int type, RecResourceModel model)
        {
            RecResourceService.Create(GetUserBrief(), model);
            return RedirectToAction("Index", new { type = model.Type, diasterId = model.DiasterId });
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index");
            }

            var result = RecResourceService.Get(id.Value);
            if (result == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Citys = SysFunc.GetCitysRecResource(GetUserBrief());

            //querystring
            ViewBag.Type = result.Type;

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(RecResourceModel model)
        {
            RecResourceService.Update(GetUserBrief(), model);

            //返回原有tab
            var entity = RecResourceService.Get(model.Id);

            return RedirectToAction("Index", new { type = entity.Type, diasterId = entity.DiasterId });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = RecResourceService.Delete(id);
            return JsonResult(result);
        }

        [HttpPost]
        public ActionResult UpdateStatus(int id, int status)
        {
            AdminResultModel result = RecResourceService.UpdateStatus(id, status);
            return JsonResult(result);
        }

        public ActionResult DownPDF(int Id)
        {
            //我要下載的檔案位置
            string filefolder = Server.MapPath("~/FileDatas/Template/");
            string fileName = "";

            var datas = RecResourceService.Get(Id);
            if (datas.Type == 1)
            {
                fileName = "應變資源調度需求表.docx";
            }
            else if (datas.Type == 2)
            {
                fileName = "應變資源提供調度表.docx";
            }
            
            string path = filefolder + fileName;

            //取得檔案名稱
            string filename = System.IO.Path.GetFileName(path);

            StringBuilder sb = new StringBuilder();
            string toFolder = Server.MapPath("~/FileDatas/Temp/");
            string toPdfName = fileName + DateTime.Now.ToString("yyyy-MM-dd") + ".pdf";
            string toPdfPath = toFolder + toPdfName;

            using (FileStream stream = System.IO.File.OpenRead(path))
            {
                XWPFDocument docx = new XWPFDocument(stream);

                Dictionary<string, string> textDic = new Dictionary<string, string>()
                    {
                        {"City",  CityService.GetAll().Where(a => a.Id == datas.CityId).FirstOrDefault().City},
                        {"CreateDate", datas.CreateDate.ToShortDateString() },
                        {"ContactPerson", datas.ContactPerson },
                        {"ContactMobilePhone", datas.ContactMobilePhone },
                        {"Reason", datas.Reason }
                };

                foreach (XWPFTable table in docx.Tables)
                {                                        
                    foreach (XWPFTableRow row in table.Rows)
                    {
                        foreach (XWPFTableCell cell in row.GetTableCells())     //遍歷每一行中的每一列
                        {
                            foreach (var paragraph in cell.Paragraphs)  // 遍歷當前表格里的所有（段落）段
                            {                                
                                foreach (var texts in textDic)
                                {
                                    try
                                    {
                                        var repStr = "[$" + texts.Key + "$]";
                                        if (paragraph.Text.Contains(repStr))
                                            paragraph.ReplaceText(repStr, texts.Value);  // 替换段落中的文字
                                    }
                                    catch (Exception ex)
                                    {
                                        // 不处理
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                }

                //讀取表格
                if (1 == 1)
                {
                    List<RecResourceModel> sList = RecResourceService.GetByCityId(datas.CityId)
                                                        .Where(a => a.Type == datas.Type)
                                                        .OrderByDescending(a => a.Id)
                                                        .ToList();

                    XWPFTable table = docx.Tables[0];

                   
                    int refn = 0;
                    if (datas.Type == 1)
                    {
                        //表格第5列(清單)
                        refn = 4;                        
                    }
                    else if (datas.Type == 2)
                    {
                        //表格第4列(清單)
                        refn = 3;                        
                    }

                    XWPFTableRow refRows = table.Rows[refn];

                    //清單資料
                    int count = 0;
                    foreach (RecResourceModel s in sList)
                    {                        
                        Dictionary<int, string> sdic = new Dictionary<int, string>()
                        {
                            { 0, s.Items},
                            { 1, s.Spec},
                            { 2, s.Quantity.ToString()},
                            { 3, s.Unit},
                            { 4, s.USDate.ToShortDateString() + "~" + s.UEDate.ToShortDateString()}
                        };
                        
                        CT_Row ctrow = refRows.GetCTRow();
                        CT_Row targetRow = new CT_Row();

                        int index = 0;
                        foreach (CT_Tc item in ctrow.Items)
                        {
                            CT_Tc addTc = targetRow.AddNewTc();
                            addTc.tcPr = item.tcPr;

                            IList<CT_P> list_p = item.GetPList();

                            foreach (var p in list_p)
                            {
                                CT_P addP = addTc.AddNewP();
                                addP.pPr = p.pPr;//段落樣式
                                IList<CT_R> list_r = p.GetRList();
                                foreach (CT_R r in list_r)
                                {
                                    CT_R addR = addP.AddNewR();
                                    addR.rPr = r.rPr;//run樣式，包含字體
                                    List<CT_Text> list_text = r.GetTList();

                                    //設定Text內容
                                    string text = sdic[index];
                                    for (int i = 0; i < text.Length; i++)
                                    {
                                        CT_Text addText = addR.AddNewT();
                                        addText.Value = text.Substring(i, 1);
                                    }

                                    ////foreach (CT_Text text in list_text)
                                    ////{
                                    ////    CT_Text addText = addR.AddNewT();
                                    ////    addText.space = text.space;
                                    ////    addText.Value = text.Value;
                                    ////}
                                }

                                index++;
                            }
                        }                        

                        //新增資料行
                        XWPFTableRow mrow = new XWPFTableRow(targetRow, table);
                        table.AddRow(mrow);
                    }

                    //刪除字型保留列(xx)
                    if (count == 0)
                    {
                        //refRows
                        table.RemoveRow(refn);
                    }
                    count++;
                }

                if (!Directory.Exists(toFolder))
                {
                    Directory.CreateDirectory(toFolder);
                }

                string toFileName = fileName + DateTime.Now.ToString("yyyy-MM-dd") + ".docx";
                string toPath = toFolder + toFileName;

                FileStream xlsFile = new FileStream(toPath, FileMode.Create, FileAccess.Write);
                docx.Write(xlsFile);
                xlsFile.Close();
                docx.Close();

                //////讀成串流
                ////var tmpStream = new FileStream(toPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                //////回傳出檔案
                ////return File(tmpStream, GetContentType("docx"), toFileName);

                // 轉換成pdf
                var app = new Microsoft.Office.Interop.Word.Application();
                // 開啟 Word 文件
                Document doc = app.Documents.Open(toPath);
                // 轉換為 PDF
                doc.ExportAsFixedFormat(toPdfPath, WdExportFormat.wdExportFormatPDF);
                //doc.Close();
                ((Microsoft.Office.Interop.Word._Document)doc).Close(false);
                ((Microsoft.Office.Interop.Word._Application)app).Quit(false);
            }

            //讀成串流
            var iStream = new FileStream(toPdfPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            //回傳出檔案
            return File(iStream, GetContentType("docx"), toPdfName);
        }

        
    }
}