using Microsoft.Office.Interop.Word;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Services;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Document = Microsoft.Office.Interop.Word.Document;

namespace MyEPA.Controllers.Rec
{
    public class RecResourceSetController : LoginBaseController
    {
        RecResourceSetService RecResourceSetService = new RecResourceSetService();
        RecResourceService RecResourceService = new RecResourceService();
        DiasterService DiasterService = new DiasterService();        
        CityService CityService = new CityService();
                
        // GET: RecResourceSet
        ////////public ActionResult Index()
        ////////{
        ////////    return View();
        ////////}

        /// <summary>
        /// 調度需求Id
        /// </summary>
        /// <param name="recResourceId"></param>
        /// <returns></returns>
        public ActionResult List(int type, int diasterId, int recResourceId = 0)
        {
            //調度配置
            string diasterName = DiasterService.GetByFilter(new DiasterFilterParameter
            {
                Ids = diasterId.ToListCollection()
            })
                .Select(e => e.DiasterName).FirstOrDefault();

            RecResourceModel RecResourceNeed = RecResourceService.Get(recResourceId);
            ViewBag.RecResourceNeed = RecResourceNeed;

            ViewBag.DiasterName = diasterName;
            ViewBag.Citys = SysFunc.GetCitysRecResource(GetUserBrief());
            ViewBag.RecResourceId = recResourceId;

            var result = GetMasterList(diasterId, recResourceId, RecResourceNeed);

            //////querystring
            ViewBag.Type = type;
            ViewBag.DiasterId = diasterId;

            return View(result);
        }

        ////[HttpPost]
        ////public ActionResult Edit(RecResourceViewModel obj)
        ////{
        ////    AdminResultModel result = Update(obj);
        ////    return JsonResult(result);
        ////}

        [HttpPost]
        public ActionResult Save(List<RecResourceViewModel> objs)
        {
            AdminResultModel result = null;
            foreach (var obj in objs)
            {
                result = Update(obj);

                if (!result.IsSuccess)
                {
                    return JsonResult(result);
                }
            }

            return JsonResult(result);
        }

        private AdminResultModel Update(RecResourceViewModel obj)
        {
            try
            {
                //可提供調度
                var help = RecResourceService.Get(obj.RecResourceIdHelp);
                if (help == null)
                {
                    return new AdminResultModel() { IsSuccess = false, ErrorMessage = "取調度資料失敗(RecResourceId)：" + obj.RecResourceIdHelp };
                }

                RecResourceSetModel entity = new RecResourceSetModel();
                var entitys = RecResourceSetService.GetByNeedHelp(obj.RecResourceIdNeed, obj.RecResourceIdHelp);
                if (entitys.Count == 0)
                {
                    //新增
                    entity = new RecResourceSetModel();
                }
                else
                {
                    //修改
                    entity = entitys.First();
                }

                entity.RecResourceIdNeed = obj.RecResourceIdNeed;
                entity.RecResourceIdHelp = obj.RecResourceIdHelp;
                entity.SetQuantity = obj.SetQuantity;
                entity.SetCityId = help.CityId;
                entity.SetContactPerson = help.ContactPerson;
                entity.SetContactMobilePhone = help.ContactMobilePhone;
                entity.SetItems = help.Items;
                entity.SetSpec = help.Spec;
                entity.SetUnit = help.Unit;

                if (entitys.Count == 0)
                {
                    //新增
                    RecResourceSetService.Create(GetUserBrief(), entity);
                }
                else
                {
                    //修改
                    RecResourceSetService.Update(GetUserBrief(), entity);
                }
            }
            catch (Exception ex)
            {
                return new AdminResultModel() { IsSuccess = false, ErrorMessage = ex.Message};
            }

            AdminResultModel result = new AdminResultModel()
            {
                IsSuccess = true,
            };

            return result;
        }

        public ActionResult DownPDF(int Id)
        {
            //我要下載的檔案位置
            string filefolder = Server.MapPath("~/FileDatas/Template/");
            string fileName = "應變資源調度審核情形.docx";

            var RecResourceNeed = RecResourceService.Get(Id);

            //調度配置
            string diasterName = DiasterService.GetByFilter(new DiasterFilterParameter
            {
                Ids = RecResourceNeed.DiasterId.ToListCollection()
            })
                .Select(e => e.DiasterName).FirstOrDefault();

            var citys = SysFunc.GetCitysRecResource(GetUserBrief());

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
                        {"diasterName", diasterName },
                        {"City", citys.Where(a => a.Id == RecResourceNeed.CityId).FirstOrDefault().City },
                        {"CreateDate", RecResourceNeed.CreateDate.ToShortDateString() },
                        {"Items", Code.GetRecItems().Where(a => a.Key == RecResourceNeed.Items).FirstOrDefault().Value },
                        {"Spec", RecResourceNeed.Spec },
                        {"ContactPerson", RecResourceNeed.ContactPerson },
                        {"ContactMobilePhone", RecResourceNeed.ContactMobilePhone },
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
                    int recResourceId = Id;
                    List<RecResourceViewModel> sList = GetMasterList(RecResourceNeed.DiasterId, recResourceId, RecResourceNeed);

                    XWPFTable table = docx.Tables[0];

                    //表格第7列(清單)
                    int refn = 6;

                    XWPFTableRow refRows = table.Rows[refn];

                    //清單資料
                    int count = 0;
                    int serial = 0;
                    foreach (RecResourceViewModel s in sList)
                    {
                        serial++;

                        Dictionary<int, string> sdic = new Dictionary<int, string>()
                        {
                            { 0, serial.ToString() },
                            { 1, s.SetQuantity.ToString() },
                            { 2,  citys.Where(a => a.Id == s.CityId).FirstOrDefault().City },
                            { 3, s.ContactPerson + "\n" + s.ContactMobilePhone },                            
                            { 4, Code.GetRecItems().Where(a => a.Key == s.Items).FirstOrDefault().Value },
                            { 5, s.Spec },
                            { 6, s.Quantity.ToString() },
                            { 7, DateFormat.ToDate4(s.USDate) + " ~ " + DateFormat.ToDate4(s.UEDate) },                            
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
    
        //取得List主表資料
        private List<RecResourceViewModel> GetMasterList(int diasterId, int recResourceId, RecResourceModel RecResourceNeed)
        {
            //(可提供資源為主表)設定數量(input)
            IEnumerable<RecResourceModel> iquery = RecResourceService.GetByDiasterId(diasterId);

            iquery = iquery.Where(a => a.Type == 2)
                            .Where(a => a.Items == RecResourceNeed.Items);

            iquery = iquery.OrderByDescending(a => a.Id);

            List<RecResourceModel> helps = iquery.ToList();

            //主表：Copy helps
            List<RecResourceViewModel> result = RecResourceViewModel.Copy(2, helps);

            //已調度
            List<RecResourceSetModel> sets = RecResourceSetService.GetByRecResourceIdNeed(new List<int> { recResourceId });

            //關聯已調度數量
            foreach (var r in result)
            {
                var v = sets.Where(a => a.RecResourceIdHelp == r.RecResourceIdHelp).FirstOrDefault();
                if (v != null)
                    r.SetQuantity = v.SetQuantity;
            }

            return result;
        }
    }
}