using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using MyEPA.Enums;
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
                //未指定災害
                var d = diasters.FirstOrDefault(e => e.Status == NormalActiveStatusEnum.Active.ToInteger());
                if (d != null)
                {
                    //預設災害啟動中
                    diasterId = d.Id;
                }
                else
                {
                    diasterId = diasters.Select(e => e.Id).FirstOrDefault();
                }
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
            //設備尚未刪除
            var equIds = iquery.Where(a => a.Type == 2).Select(a => a.Id);
            recs = recs.Where(a => equIds.Any(b => b == a.RecResourceIdHelp)).ToList();
            var countIdNeeds = recs.GroupBy(a => a.RecResourceIdNeed)
                                    .Select(a => new
                                    {
                                        Id = a.Key,
                                        Count = a.Sum(b => b.SetQuantity),
                                    }).ToList();            

            var user = GetUserBrief();

            ViewBag.Citys = CityService.GetAll();
            ////環衛組：視為admin
            //ViewBag.IsAdmin = user.Town.Trim() == "環境管理署".Trim() || user.IsAdmin;
            //ViewBag.IsTeam = user.Duty == Enums.DutyEnum.Team;
            //編輯(admin + 環管署),檢視(環境部)
            if (user.IsAdmin || user.Town.Equals("環境管理署"))
            {
                ViewBag.IsEdit = true;
            }
            else
            {
                ViewBag.IsEdit = false;
            }
            //三區只能檢視
            ViewBag.IsTab3Show = user.Duty == Enums.DutyEnum.EPA || user.Duty == Enums.DutyEnum.Team;
            ViewBag.User = user;
            ViewBag.CountIdNeeds = countIdNeeds;

            //代碼
            VehicleTypeRepository VehicleTypeRepository = new VehicleTypeRepository();
            DisinfectorTypeRepository DisinfectorTypeRepository = new DisinfectorTypeRepository();
            DisinfectantTypeRepository DisinfectantTypeRepository = new DisinfectantTypeRepository();
            ViewBag.VehicleTypeRepository = VehicleTypeRepository.GetList();
            ViewBag.DisinfectorTypeRepository = DisinfectorTypeRepository.GetList();
            ViewBag.DisinfectantTypeRepository = DisinfectantTypeRepository.GetList();

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

            //////信件通知
            ////if (type == 1)
            ////{
            ////    ToSendNeed(model);
            ////}
            ////else if (type == 2)
            ////{
            ////    ToSendHelp (model);
            ////}

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
            var datas = RecResourceService.Get(Id);
            if (datas.Type == 1)
            {
                //應變資源調度需求表(1)
                return GetPDF_1(datas);
            }
            else if (datas.Type == 2)
            {
                //應變資源提供調度表(2)
                return GetPDF_2(datas);
            }

            return null;
        }

        /// <summary>
        /// 應變資源調度需求表(1)
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult GetPDF_1(RecResourceModel datas)
        {
            //我要下載的檔案位置
            string filefolder = Server.MapPath("~/FileDatas/Template/");
            string fileName = fileName = "(範本)應變資源調度需求表.docx";

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
                        {"Reason", datas.Reason },
                        {"GatherDate", DateFormat.ToDate7(datas.GatherDate) },
                        {"GatherPlace", datas.GatherPlace },
                        {"CheckPerson", datas.CheckPerson },
                        {"CheckMobilePhone", datas.CheckMobilePhone },
                        {"COPerson", datas.COPerson },
                        {"COMobilePhone", datas.COMobilePhone },
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
                    ////List<RecResourceModel> sList = RecResourceService.GetByCityId(datas.CityId)
                    ////                                    .Where(a => a.Type == datas.Type)
                    ////                                    .Where(a => a.DiasterId == datas.DiasterId)
                    ////                                    .OrderByDescending(a => a.Id)
                    ////                                    .ToList();

                    //單筆列印(環衛組需求)
                    List<RecResourceModel> sList = datas.ToListCollection();

                    XWPFTable table = docx.Tables[0];

                    //範本(row5)：表格第5列(清單)
                    int refn = 4;

                    //範本(row6)：說明第6列(底部)
                    int temfn5 = 5;
                    XWPFTableRow rowDesc1 = table.Rows[temfn5];                   

                    //範本(row7)：說明第7列(底部)
                    int temfn6 = 6;
                    XWPFTableRow rowDesc2 = table.Rows[temfn6];                   

                    //範本(row8)：說明第8列(底部)
                    int temfn7 = 7;
                    XWPFTableRow rowDesc3 = table.Rows[temfn7];                   

                    //範本(row9)：說明第9列(底部)
                    int temfn8 = 8;
                    XWPFTableRow rowDesc4 = table.Rows[temfn8];

                    table.RemoveRow(temfn8);
                    table.RemoveRow(temfn7);
                    table.RemoveRow(temfn6);
                    table.RemoveRow(temfn5);

                    XWPFTableRow refRows = table.Rows[refn];

                    //清單資料
                    foreach (RecResourceModel s in sList)
                    {
                        Dictionary<int, string> sdic = new Dictionary<int, string>()
                        {
                            { 0, Code.GetOneRecItems(s.TypeItems, s.Items)},
                            { 1, s.Spec},
                            { 2, s.Quantity.ToString()},
                            { 3, s.Unit},
                            { 4, s.USDate.ToShortDateString() + " ~ " + s.UEDate.ToShortDateString()}
                        };

                        //null預設空值
                        for (int i = 0; i < sdic.Count; i++)
                        {
                            if (sdic[i] == null)
                                sdic[i] = "";
                        }

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

                                    //index超過
                                    if (index >= sdic.Count)
                                        continue;

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
                    table.RemoveRow(refn);

                    //補回底部說明1
                    table.AddRow(rowDesc1);

                    //補回底部說明2
                    table.AddRow(rowDesc2);

                    //補回底部說明3
                    table.AddRow(rowDesc3);

                    //補回底部說明4
                    table.AddRow(rowDesc4);
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

                //移除(範本)文字
                toPdfPath = toPdfPath.Replace("(範本)", "");

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
            return File(iStream, GetContentType("docx"), Path.GetFileName(toPdfPath));
        }

        /// <summary>
        /// 應變資源提供調度表(2)
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public ActionResult GetPDF_2(RecResourceModel datas)
        {
            //我要下載的檔案位置
            string filefolder = Server.MapPath("~/FileDatas/Template/");
            string fileName = "(範本)應變資源提供調度表.docx";

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
                                                        .Where(a => a.DiasterId == datas.DiasterId)
                                                        .OrderByDescending(a => a.Id)
                                                        .ToList();

                    XWPFTable table = docx.Tables[0];

                    //範本(row4)：表格第4列(清單)
                    int refn = 3;
                    XWPFTableRow refRows = table.Rows[refn];

                    //範本(row5)：說明第5列(底部)
                    int temfn = 4;
                    XWPFTableRow rowDesc1 = table.Rows[temfn];
                    table.RemoveRow(temfn);

                    //清單資料
                    foreach (RecResourceModel s in sList)
                    {
                        Dictionary<int, string> sdic = new Dictionary<int, string>()
                        {
                            { 0, Code.GetOneRecItems(s.TypeItems, s.Items)},
                            { 1, s.Spec},
                            { 2, s.Quantity.ToString()},
                            { 3, s.Unit},
                            { 4, DateFormat.ToDate7_2(s.VDate)},
                            { 5, s.USDate.ToShortDateString() + " ~ " + s.UEDate.ToShortDateString()},
                            { 6, DateFormat.ToDate7(s.GoDate)},
                        };

                        //null預設空值
                        for (int i = 0; i < sdic.Count; i++)
                        {
                            if (sdic[i] == null)
                                sdic[i] = "";
                        }

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

                                    //index超過
                                    if (index >= sdic.Count)
                                        continue;

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
                    table.RemoveRow(refn);

                    //補回底部說明1
                    table.AddRow(rowDesc1);
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

                //移除(範本)文字
                toPdfPath = toPdfPath.Replace("(範本)", "");

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
            return File(iStream, GetContentType("docx"), Path.GetFileName(toPdfPath));
        }

        private bool ToSendNeed(RecResourceModel model)
        {
            try
            {
                //(1)設定內容 (特休結算通知)
                string body = "";
                string path = AppConfig.HtmlTemplatePath + "1_調度需求.html";
                using (StreamReader reader = System.IO.File.OpenText(path))
                {
                    body = reader.ReadToEnd();
                }

                if (body == "")
                {
                    logger.Error("ToSend - body取出無內容：" + path);
                    return false;
                }

                string receiveAddr = "brianlin12345@gmail.com";
                string receiveName = "林正祥";

                string diasterName = DiasterService.GetByFilter(new DiasterFilterParameter
                {
                    Ids = model.DiasterId.ToListCollection()
                })
                .Select(e => e.DiasterName).FirstOrDefault();

                var user = GetUserBrief();
                body = body.Replace("[receiveName]", receiveName)
                        .Replace("[diasterName]", diasterName)
                        .Replace("[DateNow]", DateFormat.ToDate4(DateTime.Now))
                        .Replace("[Unit]", model.Unit)
                        .Replace("[Items]", Code.GetOneRecItems(model.TypeItems, model.Items))
                        .Replace("[Spec]", model.Spec)
                        .Replace("[Quantity]", model.Quantity.ToString())
                        .Replace("[UseDate]", DateFormat.ToDate4(model.USDate) + "~" + DateFormat.ToDate4(model.UEDate))
                        .Replace("[LoginUser]", user.Name)
                        .Replace("[ContactPerson]", model.ContactPerson)
                        .Replace("[ContactMobilePhone]", model.ContactMobilePhone)
                        .Replace("[Reason]", model.Reason);

                //(2)設定Helper
                EmailHelper email = new EmailHelper();
                email.MailServer = AppConfig.SMTPServer;
                email.MailPort = AppConfig.SMTPPort;
                if (AppConfig.EmailPassword != String.Empty)
                {
                    email.Account = AppConfig.EmailAddress;
                    email.Password = AppConfig.EmailPassword;
                }
                email.MailFrom = AppConfig.EmailAddress;
                email.MailFromName = AppConfig.EmailFromName;
                email.EnableSSL = false; //產基會Mail Server無設定SSL(根據驗證程序，遠端憑證是無效的) AppConfig.EnableSSL;
                string subject = "調度需求";  //row["sen_subject"].ToString();                
                bool isSend = AppConfig.SendEmail;
                email.Subject = subject;
                email.Body = body;

                //(3)收件人員
                //1 收件者(AddTo), 2 副本(AddCC), 3 密件副本(AddBCC)                                
                //收件者                
                string addr1 = AppConfig.TestEmailAddress != "" ? AppConfig.TestEmailAddress : receiveAddr;
                email.AddTo(addr1, receiveName);

                //副本
                foreach (string addr in AppConfig.EmailAddressCC.Split(','))
                {
                    if (addr != "")
                    {
                        email.AddCC(addr, "");
                    }
                }

                email.IsSendEmail = isSend;
                bool success = email.SendBySmtp();
                if (!success)
                {
                    logger.Error("ToSend - 信件寄發失敗:" + addr1);
                }
            }
            catch (Exception ex)
            {
                logger.Error("ToSend - 信件寄發錯誤");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
                return false;
            }

            return true;
        }

        private bool ToSendHelp(RecResourceModel model)
        {
            try
            {
                //(1)設定內容 (特休結算通知)
                string body = "";
                string path = AppConfig.HtmlTemplatePath + "1_可提供調度.html";
                using (StreamReader reader = System.IO.File.OpenText(path))
                {
                    body = reader.ReadToEnd();
                }

                if (body == "")
                {
                    logger.Error("ToSend - body取出無內容：" + path);
                    return false;
                }

                string receiveAddr = "brianlin12345@gmail.com";
                string receiveName = "林正祥";

                string diasterName = DiasterService.GetByFilter(new DiasterFilterParameter
                {
                    Ids = model.DiasterId.ToListCollection()
                })
                .Select(e => e.DiasterName).FirstOrDefault();

                var user = GetUserBrief();
                body = body.Replace("[receiveName]", receiveName)
                        .Replace("[diasterName]", diasterName)
                        .Replace("[DateNow]", DateFormat.ToDate4(DateTime.Now))
                        .Replace("[Unit]", model.Unit)
                        .Replace("[Items]", Code.GetOneRecItems(model.TypeItems, model.Items))
                        .Replace("[Spec]", model.Spec)
                        .Replace("[Quantity]", model.Quantity.ToString())
                        .Replace("[UseDate]", DateFormat.ToDate4(model.USDate) + "~" + DateFormat.ToDate4(model.UEDate))
                        .Replace("[LoginUser]", user.Name)
                        .Replace("[ContactPerson]", model.ContactPerson)
                        .Replace("[ContactMobilePhone]", model.ContactMobilePhone)
                        .Replace("[Reason]", model.Reason);

                //(2)設定Helper
                EmailHelper email = new EmailHelper();
                email.MailServer = AppConfig.SMTPServer;
                email.MailPort = AppConfig.SMTPPort;
                if (AppConfig.EmailPassword != String.Empty)
                {
                    email.Account = AppConfig.EmailAddress;
                    email.Password = AppConfig.EmailPassword;
                }
                email.MailFrom = AppConfig.EmailAddress;
                email.MailFromName = AppConfig.EmailFromName;
                email.EnableSSL = false; //產基會Mail Server無設定SSL(根據驗證程序，遠端憑證是無效的) AppConfig.EnableSSL;
                string subject = "可提供調度";  //row["sen_subject"].ToString();                
                bool isSend = AppConfig.SendEmail;
                email.Subject = subject;
                email.Body = body;

                //(3)收件人員
                //1 收件者(AddTo), 2 副本(AddCC), 3 密件副本(AddBCC)                                
                //收件者                
                string addr1 = AppConfig.TestEmailAddress != "" ? AppConfig.TestEmailAddress : receiveAddr;
                email.AddTo(addr1, receiveName);

                //副本
                foreach (string addr in AppConfig.EmailAddressCC.Split(','))
                {
                    if (addr != "")
                    {
                        email.AddCC(addr, "");
                    }
                }

                email.IsSendEmail = isSend;
                bool success = email.SendBySmtp();
                if (!success)
                {
                    logger.Error("ToSend - 信件寄發失敗:" + addr1);
                }
            }
            catch (Exception ex)
            {
                logger.Error("ToSend - 信件寄發錯誤");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
                return false;
            }

            return true;
        }
    }
}