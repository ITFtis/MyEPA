using iTextSharp.text;
using iTextSharp.text.pdf;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xceed.Document.NET;
using Xceed.Words.NET;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyEPA.Services
{
    public partial class ContactManualReportService
    {
        UsersRepository UsersRepository = new UsersRepository();
        CityRepository CityRepository = new CityRepository();
        ContactManualService ContactManualService = new ContactManualService();
        ContactManualOnDutyService ContactManualOnDutyService = new ContactManualOnDutyService();
        ContactManualEPARoleService ContactManualEPARoleService = new ContactManualEPARoleService();
        ContactManualSuperviseRepository ContactManualSuperviseRepository = new ContactManualSuperviseRepository();
        ContactManualDepartmentRepository ContactManualDepartmentRepository = new ContactManualDepartmentRepository();
        ContactManualRecycleService ContactManualRecycleService = new ContactManualRecycleService();
        ContactManualFileDataService ContactManualFileDataService = new ContactManualFileDataService();
        ContactManualDownloadRecordRepository ContactManualDownloadRecordRepository = new ContactManualDownloadRecordRepository();
        public Stream ReportOfPDF(UserBriefModel userBrief)
        {
            var word = Report(userBrief);

            //轉換成PDF
            string pdfPath = PDFHelper.ConvertToToPDF(word);

            var user = UsersRepository.Get(userBrief.UserId);
            string text = user.UserName;
            string password = user.MobilePhone?.Replace("-", "");

            Stream outStream = WatermarkAndPassword(pdfPath, text, password);

            var model = new ContactManualDownloadRecordModel 
            {
                MobilePhone = password,
                Name = user.Name,
                UserId = user.Id
            };
            //下載紀錄
            ContactManualDownloadRecordRepository.Create(userBrief, model);

            return outStream;
        }
        public Stream ReportOfPDFNew(UserBriefModel userBrief)
        {
            //var word = Report(userBrief);
            //2022/12/24
            //轉換成PDF
            //string pdfPath = PDFHelper.ConvertToToPDF(word);
            //var path = "~";

            //var fullPath = Path.GetFullPath(path);
            var fullPath = System.Web.HttpContext.Current.Server.MapPath("/");
            string pdfPath = string.Format("{0}/環境部111年度環境污染事故(含春節期間)緊急應變摘要及通聯手冊.pdf", fullPath);
            string pdfPath_ = string.Format("{0}/環境部111年度環境污染事故(含春節期間)緊急應變摘要及通聯手冊_.pdf", fullPath);
            var user = UsersRepository.Get(userBrief.UserId);
            string dastr = DateTime.Now.ToDateString("yyyy/MM/dd");
            string text = user.UserName + " 僅供公務使用";
            string password = user.MobilePhone?.Replace("-", "");
            File.Copy(pdfPath, pdfPath_, true);
            Stream outStream = WatermarkAndPassword(pdfPath_, text, password);

            var model = new ContactManualDownloadRecordModel
            {
                MobilePhone = password,
                Name = user.Name,
                UserId = user.Id
            };
            //下載紀錄
            ContactManualDownloadRecordRepository.Create(userBrief, model);

            return outStream;
        }
        public Stream Report(UserBriefModel userBrief)
        {
            WordWriteHelper helper = new WordWriteHelper();

            helper.AddWordTitle("環境部 111年度環境污染事故");
            helper.AddWordTitle("(含春節期間)緊急應變摘要及通聯手冊");
            helper.AddMainTitle("壹、本屬環境污染事故主政單位及地方環保局長通聯名冊", HeadingType.Heading1);
            AddEPATable(helper, $"一、{ContactManualTypeEnum.EPA.GetDescription()}", ContactManualTypeEnum.EPA, HeadingType.Heading2);
            helper.AddTable("二、地方環保局長通聯名冊", HeadingType.Heading2, GetEPB(ContactManualTypeEnum.EPB));
            helper.AddMainTitle("貳、各業務單位春節因應環境污染事故緊急應變通連表", HeadingType.Heading1);
            
            //一、綜計處
            AddGeneralPlanningDepartment(helper);
            //二、空保處
            AddAirDepartment(helper);
            //三、水保處
            AddWaterDepartment(helper);
            //四、廢管處
            AddWasteDepartment(helper);
            //五、管考處
            AddControlAssessmentDepartment(helper);
            //六、監資處
            AddSupervisionDepartment(helper);
            //七、環境管理署
            AddEnvironmentDepartment(helper);
            //八、秘書處
            AddSecretaryRoomTable(helper);
            //九、新聞公關組
            AddNewsTable(helper);
            //十、回收基管會
            AddRecycleDepartment(helper);
            //十一、土汙基管會-土壤及地下水環境污染事故緊急應變通聯表
            AddSoilPollutionDepartment(helper);
            //十二、環檢所
            AddEnvironmentalTable(helper);
            //十三、化學物質管理署
            AddChemicalDepartment(helper);
            //新增檔案上傳
            AddFiles(helper);

            //轉出 Word
            Stream stream = helper.GetStreamResult();
           
            return stream;
        }

        private static void AddTaiwanImage(WordWriteHelper helper)
        {
            string imagePath  = System.Web.Hosting.HostingEnvironment.MapPath("~\\Content\\Taiwan.png");
            
            helper.AddImage(imagePath, true);
        }

        /// <summary>
        /// 加入浮水印
        /// </summary>
        /// <param name="pdfPath"></param>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static Stream WatermarkAndPassword(string pdfPath, string text, string password)
        {
            string fileName = DateTimeHelper.GetCurrentTime().ToString("yyyyMMddHHmmssfff");
            string outFilePath = UploadFileHelper.GetServerMapPath("ContactManualReport", $"WatermarkAndPassword{fileName}.pdf");

            FileStream outStream = new FileStream(outFilePath, FileMode.Create);

            PdfReader pdfReader = new PdfReader(pdfPath);

            PdfStamper pdfStamper = new PdfStamper(pdfReader, outStream);

            int total = pdfReader.NumberOfPages + 1;
            Rectangle psize = pdfReader.GetPageSize(1);
            float width = psize.Width;
            float height = psize.Height;
            PdfContentByte waterMarkContent;
            BaseFont font = BaseFont.CreateFont("C:\\Windows\\fonts\\mingliu.ttc,0", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            PdfGState gs = new PdfGState();
            pdfStamper.SetEncryption(true, password, "secret", PdfWriter.ALLOW_SCREENREADERS);  /*寫入密碼*/

            for (int i = 1; i < total; i++)
            {
                //在內容上方加水印（下方加水印參考上面圖片程式碼做法）
                waterMarkContent = pdfStamper.GetOverContent(i);
                //透明度
                gs.FillOpacity = 0.3f;
                waterMarkContent.SetGState(gs);
                //寫入文字
                waterMarkContent.BeginText();
                waterMarkContent.SetColorFill(BaseColor.GRAY);
                waterMarkContent.SetFontAndSize(font, 50);
                waterMarkContent.SetTextMatrix(0, 0);

                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 120, height - 200, 45);
                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 450, height - 200, 45);

                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 120, height - 400, 45);
                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 450, height - 400, 45);

                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 120, height - 600, 45);
                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 450, height - 600, 45);

                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 120, height - 800, 45);
                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 450, height - 800, 45);

                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 120, height - 1000, 45);
                waterMarkContent.ShowTextAligned(Element.ALIGN_CENTER, text, width - 450, height - 1000, 45);

                waterMarkContent.EndText();
            }
            pdfStamper.Close();
            pdfReader.Close();

            UploadFileHelper.DeleteFileByPath(pdfPath);

            Stream streamResult = new FileStream(outFilePath, FileMode.Open);

            Stream result = new MemoryStream();

            streamResult.CopyTo(result);

            streamResult.Close();

            UploadFileHelper.DeleteFileByPath(outFilePath);

            result.Position = 0;

            return result;
        }

        private void AddFiles(WordWriteHelper helper)
        {
            List<ContactManualDepartmentModel> departments = ContactManualDepartmentRepository.GetList().OrderBy(e => e.Sort).ToList();

            Action<SourceTypeEnum> AddFileByType = (type) =>
            {
                var contactManualFiles =
                                ContactManualFileDataService.GetListBySource(type)
                                .ToDictionary(e => e.SourceId, e => e.RealFileName);

                foreach (var item in departments)
                {
                    string fileName = contactManualFiles.GetValue(item.Id);
                    if (string.IsNullOrWhiteSpace(fileName) == false)
                    {
                        AddDocXByFileName(helper, fileName);
                    }
                }
            };
            helper.AddMainTitle("參、各業務單位春節因應環境污染事故緊急應變摘要", HeadingType.Heading1);
            AddFileByType(SourceTypeEnum.ContactManual);
            helper.AddMainTitle("肆、各業務單位春節因應環境污染事故緊急應變作業流程圖", HeadingType.Heading1);
            AddFileByType(SourceTypeEnum.ContactManualFlow);
        }
        /// <summary>
        /// 十三、化學物質管理署
        /// </summary>
        /// <param name="helper"></param>
        private void AddChemicalDepartment(WordWriteHelper helper)
        {
            helper.AddMainTitle("十三、化學物質管理署－毒災、毒化物管理、環境用藥緊急應變通聯表", HeadingType.Heading2);
            //14-1 毒性化學物質災害事故之緊急應變通聯表
            helper.AddMainTitle("13-1 毒性化學物質災害事故之緊急應變通聯表", HeadingType.Heading3);
            //環境部化學物質管理署毒災事故應變計畫主管單位
            AddEPARoleTable(helper, ContactManualTypeEnum.EPARoleChemicalPoison);
            //環境部24小時勤情表
            AddEPA24OnDuty(helper, ContactManualTypeEnum.EPA24OnDutyChemical);
            //各縣市環保局春節期間毒性化學物質災害事故緊急通聯表
            AddEPBTable(helper, ContactManualTypeEnum.EPBChemical);
            //14-2 環境用藥重大消費事件緊急應變通聯表
            helper.AddMainTitle("13-2 環境用藥重大消費事件緊急應變通聯表", HeadingType.Heading3);
            //環境部化學物質管理署環境用藥重大消費事件緊急應變計畫主管單位
            AddEPARoleTable(helper, ContactManualTypeEnum.EPARoleChemicalDrug);
            //各地方環保局春節期間環境用藥重大消費事件緊急應變通聯表
            AddEPBTable(helper, ContactManualTypeEnum.EPBChemicalDrug);
            //春節期間化學物質管理署環境污染事故督導責任區域劃分表
            AddSuperviseTable(helper, "化學物質管理署", ContactManualTypeEnum.EPASuperviseChemical);
        }

        private void AddEPA24OnDuty(WordWriteHelper helper, ContactManualTypeEnum type)
        {
            var contactManuals = ContactManualService.GetListByType(type);
            var result = contactManuals.ConvertToModel<ContactManualViewModel, ContactManual24OnDutyViewModel>();
            helper.AddTable(type.GetDescription(), HeadingType.Heading4, result);
        }
        /// <summary>
        /// 十二、環檢所
        /// </summary>
        /// <param name="helper"></param>
        private void AddEnvironmentalTable(WordWriteHelper helper)
        {
            helper.AddMainTitle("十二、環檢所－相關檢驗事項緊急應變通聯表", HeadingType.Heading2);
            //環境部國家環境研究院災害防救緊急應變聯絡資料
            AddEPAOther(helper, ContactManualTypeEnum.EPAEnvironmentalInspection);
        }
        /// <summary>
        /// 十一、土汙基管會-土壤及地下水環境污染事故緊急應變通聯表
        /// </summary>
        /// <param name="helper"></param>
        private void AddSoilPollutionDepartment(WordWriteHelper helper)
        {
            helper.AddMainTitle("十一、土汙基管會-土壤及地下水環境污染事故緊急應變通聯表", HeadingType.Heading2);
            //環境部土汙基管會土壤及地下水環境污染事故應變計畫主管單位
            AddEPARoleTable(helper, ContactManualTypeEnum.EPASoilPollution);
            //各縣市環保局春節期間土壤及地下水環境污染事故緊急通聯表
            AddEPBTable(helper, ContactManualTypeEnum.EPBSoilPollution);
            //春節期間土汙基管會事故督導責任區域劃分表
            AddSuperviseTable(helper, "土汙基管會", ContactManualTypeEnum.EPASuperviseSoilPollution);
        }

        /// <summary>
        /// 十、回收基管會
        /// </summary>
        /// <param name="helper"></param>
        private void AddRecycleDepartment(WordWriteHelper helper)
        {
            helper.AddMainTitle("十、回收基管會－應回收廢棄物回收處理業環境污染事故緊急應變通聯表", HeadingType.Heading2);
            AddEPARoleTable(helper, ContactManualTypeEnum.EPARoleRecycle);
            //回收基管會回收處理業重大異常事件緊急連絡通話
            AddRecycleTable(helper, ContactManualTypeEnum.EPARecycle);
            //各地方環保局春節期間應回收廢棄物回收處理業危機處理及環保重大事件應變緊急連絡表
            AddEPBTable(helper, ContactManualTypeEnum.EPBRecycle);
            //應回收廢棄物回收處理業危機處理及環保重大事件應變通報聯絡人員名冊(稽核認證團體)
            AddRecycleTable(helper, ContactManualTypeEnum.EPARecycleEF);
            //春節期間回收機管會環境汙閃事故督導責任區域劃分表
            AddSuperviseTable(helper, "回收基管會", ContactManualTypeEnum.EPASuperviseRecycle);
        }
        /// <summary>
        /// 應回收廢棄物回收處理業危機處理及環保重大事件應變通報聯絡人員名冊
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="type"></param>
        private void AddRecycleTable(WordWriteHelper helper, ContactManualTypeEnum type)
        {
            var recycle = ContactManualRecycleService.GetReportList(type);
            helper.AddTable(type.GetDescription(), HeadingType.Heading4, recycle);
        }

        /// <summary>
        /// 九、新聞公關組
        /// </summary>
        /// <param name="helper"></param>
        private void AddNewsTable(WordWriteHelper helper)
        {
            AddEPAOtherHeading2(helper, ContactManualTypeEnum.EPANewsPublicRelationsTeam);
        }
        /// <summary>
        /// 八、秘書處
        /// </summary>
        /// <param name="helper"></param>
        private void AddSecretaryRoomTable(WordWriteHelper helper)
        {
            helper.AddMainTitle("八、秘書處－辦公大樓緊急事件應變通聯表", HeadingType.Heading2);
            //環境部秘書處辦公大樓緊急事件主管單位
            AddEPAOther(helper, ContactManualTypeEnum.EPASecretaryRoom);
        }

        private void AddEPATable(WordWriteHelper helper,string title ,ContactManualTypeEnum type, HeadingType headingType)
        {
            helper.AddTable(title, headingType, GetEPA(type));
        }

        /// <summary>
        /// 六、監資處
        /// </summary>
        /// <param name="helper"></param>
        private void AddSupervisionDepartment(WordWriteHelper helper)
        {
            AddSupervisionDepartmentOne(helper);
            AddSupervisionDepartmentTwo(helper);
        }
        /// <summary>
        /// 6-2
        /// </summary>
        /// <param name="helper"></param>
        private void AddSupervisionDepartmentTwo(WordWriteHelper helper)
        {
            helper.AddMainTitle("6-2 電腦機房及資訊系統事故緊急應變通聯表", HeadingType.Heading3);
            //環境部監資處電腦機房及各項資訊系統運轉事故緊急應變人員名冊
            AddEPARoleTable(helper, ContactManualTypeEnum.EPARoleSupervisionInformation);
            //委外載入2
            AddSupervisionOutsourcing(helper, SupervisionSourceEnum.Two);
        }
        /// <summary>
        /// 載入委外檔案
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="source"></param>
        private void AddSupervisionOutsourcing(WordWriteHelper helper, SupervisionSourceEnum source)
        {
            ContactManualFileDataUploadViewModel fileData =
                            ContactManualFileDataService
                            .GetBySource(SourceTypeEnum.Supervision, source.ToInteger());

            if (fileData != null)
            {
                AddDocXByFileName(helper, fileData.RealFileName);
            }
        }
        /// <summary>
        /// 6-1
        /// </summary>
        /// <param name="helper"></param>
        private void AddSupervisionDepartmentOne(WordWriteHelper helper)
        {
            helper.AddMainTitle("六、監資處－空品監測站安全維護及電腦機房及資訊系統事故緊急應變通連表", HeadingType.Heading2);
            helper.AddMainTitle("6-1 空品監測站安全維護緊急應變通連表", HeadingType.Heading3);
            //環境部監資處空氣品質監測站安全維護緊急應變人員名冊
            AddEPARoleTable(helper, ContactManualTypeEnum.EPARoleSupervisionAirQuality);
            //監資處春節期間空氣品質污染指標預報值班人員通聯表
            AddEPAOnDutyTable(helper, ContactManualTypeEnum.EPAOnDutySupervision);
            //監資處空品監測站各區安全維護緊急應變小組人員名冊
            AddEPARoleTable(helper, ContactManualTypeEnum.EPARoleSupervisionAir);
            //委外載入
            AddSupervisionOutsourcing(helper, SupervisionSourceEnum.One);
            //春節期間監資處環境污染事件督導責任區域劃分表
            AddSuperviseTable(helper, "監資處", ContactManualTypeEnum.EPASuperviseSupervision);   
        }

        private static void AddDocXByFileName(WordWriteHelper helper, string realFileName)
        {
            if (UploadFileHelper.IsExistsByFileName(realFileName) == false)
            {
                return;
            }
            Stream file = UploadFileHelper.GetFileStream(realFileName);
            DocX docx = DocX.Load(file);
            helper.AddDocX(docx);
        }

        /// <summary>
        /// 五、管考處
        /// </summary>
        /// <param name="helper"></param>
        private void AddControlAssessmentDepartment(WordWriteHelper helper)
        {
            helper.AddMainTitle("五、管考處－重大緊急公害糾紛事件通聯表", HeadingType.Heading2);
            //環境部管考處重大緊急公害糾紛事件主管單位
            AddEPARoleTable(helper, ContactManualTypeEnum.EPARoleControlAssessment);
            //各地方環保局春節期間重大緊急公害糾紛事件通聯表
            AddEPBTable(helper, ContactManualTypeEnum.EPBControlAssessment);
            //春節期間管考處重大緊急公害糾紛事件督導責任區域劃分表
            AddSuperviseTable(helper, "管考處", ContactManualTypeEnum.EPASuperviseControlAssessment);
        }
        /// <summary>
        /// 七、環境管理署
        /// </summary>
        /// <param name="helper"></param>
        private void AddEnvironmentDepartment(WordWriteHelper helper)
        {
            helper.AddMainTitle("七、環境管理署－天然災害、因應流感疫情爆發事件緊急應變通連表", HeadingType.Heading2);

            helper.AddMainTitle("7-1 天然災害事故緊急應變通連表", HeadingType.Heading3);
            //環境部環境管理署天然災害應變計畫主管單位
            AddEPARoleTable(helper, ContactManualTypeEnum.EPARoleEnvironmentDisaster);
            //各地方環保局春節期間天然災害事故緊急通聯表
            AddEPBTable(helper, ContactManualTypeEnum.EPBEnvironmentDisaster);

            helper.AddMainTitle("7-2 春節因應流感疫情爆發之環境緊急應變通連表", HeadingType.Heading3);
            //環境部春節期間流感疫情爆發之環境清理應變計畫主管單位
            AddEPARoleTable(helper, ContactManualTypeEnum.EPARoleEnvironmentInfluenza);
            //各地方環保局春節期間流感疫情爆發之環境清理緊急通聯表
            AddEPBTable(helper, ContactManualTypeEnum.EPBEnvironmentInfluenza);

            helper.AddMainTitle("7-3 一般廢棄物處理設備設施緊急應變通連表", HeadingType.Heading3);
            //環境部環境管理署廢棄物處理事故緊急應變計畫主管單位
            AddEPARoleTable(helper, ContactManualTypeEnum.EPARoleTeam);
            //環境管理署春節期間一般廢棄物處理設備設施緊急應變通聯表
            AddEPATable(helper, ContactManualTypeEnum.EPATeam.GetDescription(), ContactManualTypeEnum.EPATeam, HeadingType.Heading4);
            //各地方環保局春節期間一般廢棄物處理設備設施緊急應變通聯表
            AddEPBTable(helper, ContactManualTypeEnum.EPBTeam);

            //春節期間環境管理署環境污染事故督導責任區域劃分表
            AddSuperviseTable(helper, "環境管理署", ContactManualTypeEnum.EPASuperviseTeam);
        }
        /// <summary>
        /// 四、廢管處
        /// </summary>
        /// <param name="helper"></param>
        private void AddWasteDepartment(WordWriteHelper helper)
        {
            helper.AddMainTitle("四、廢管處－一般廢棄物環境污染事故緊急應變通連表", HeadingType.Heading2);
            //環境部廢管處一般廢棄物緊急事故應變計畫主管單位
            AddEPARoleTable(helper, ContactManualTypeEnum.EPARoleWaste);
            //各地方環保局春節期間一般廢棄物處理緊急應變通聯表
            AddEPBTable(helper, ContactManualTypeEnum.EPBWaste);
            //春節期間廢管處環境污染事故督導責任區域劃分表
            AddSuperviseTable(helper, "廢管處", ContactManualTypeEnum.EPASuperviseWaste);
        }
        /// <summary>
        /// 三、水保處
        /// </summary>
        /// <param name="helper"></param>
        private void AddWaterDepartment(WordWriteHelper helper)
        {
            helper.AddMainTitle("三、水保處－河川水污染、飲用水重大事故緊急應變通連表", HeadingType.Heading2);
            helper.AddMainTitle("3-1 河川水污染事故緊急應變通連表", HeadingType.Heading3);
            //環境部水保處河川水污染事故緊應變計畫主管單位
            AddEPARoleTable(helper, ContactManualTypeEnum.EPARoleWaterSecurityRiver);
            //各地方環保局春節期間河川污染應變通報名冊緊急通聯表
            AddEPBTable(helper, ContactManualTypeEnum.EPBWaterSecurityRiver);
            helper.AddMainTitle("3-2 飲用水重大事故緊急應變通聯表", HeadingType.Heading3);
            //環境部水保處飲用水重大事故應變計畫主管單位
            AddEPARoleTable(helper, ContactManualTypeEnum.EPARoleWaterSecurityDrinkingWater);
            //各地方環保局春節期間飲用水重大事故緊急通聯表
            AddEPBTable(helper, ContactManualTypeEnum.EPBWaterSecurityDrinkingWater);
            //春節期間水保處環境污染事故督導責任區域劃分表
            AddSuperviseTable(helper, "水保處", ContactManualTypeEnum.EPASuperviseWaterSecurity);
        }
        /// <summary>
        /// 二、空保處
        /// </summary>
        /// <param name="helper"></param>
        private void AddAirDepartment(WordWriteHelper helper)
        {
            helper.AddMainTitle("二、空保處－環境影響評估案件緊急應變通連表", HeadingType.Heading2);
            //環境部空保處空氣污染緊急事故應變計畫主管單位
            AddEPARoleTable(helper, ContactManualTypeEnum.EPARoleAirSecurity);
            //各地方環保局春節期間空氣污染事故緊急通連表
            AddEPBTable(helper, ContactManualTypeEnum.EPBAirSecurity);
            //春節期間空保處環境污染事故督導責任區域劃分表
            AddSuperviseTable(helper, "空保處", ContactManualTypeEnum.EPASuperviseAirSecurity);
        }
        /// <summary>
        /// 一、綜計處
        /// </summary>
        /// <param name="helper"></param>
        private void AddGeneralPlanningDepartment(WordWriteHelper helper)
        {
            helper.AddMainTitle("一、綜計處－環境影響評估案件緊急應變通連表", HeadingType.Heading2);
            helper.AddTable("環境部環境保護司環境影響評估案件緊急應變主管單位", HeadingType.Heading4, GetEPAOther(ContactManualTypeEnum.EPAGeneralPlanning));
            AddSuperviseTable(helper, "綜計處", ContactManualTypeEnum.EPASuperviseGeneralPlanning);
        }
        private void AddEPAOtherHeading2(WordWriteHelper helper, ContactManualTypeEnum type)
        {
            helper.AddTable($"九、{type.GetDescription()}", HeadingType.Heading2, GetEPAOther(type));
        }
        private void AddEPAOther(WordWriteHelper helper, ContactManualTypeEnum type)
        {
            helper.AddTable(type.GetDescription(), HeadingType.Heading4, GetEPAOther(type));
        }
        /// <summary>
        /// 新增值班表
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="manualTypeEnum"></param>
        private void AddEPAOnDutyTable(WordWriteHelper helper, ContactManualTypeEnum manualTypeEnum)
        {
            helper.AddTable(manualTypeEnum.GetDescription(), HeadingType.Heading4, GetEPAOnDuty(manualTypeEnum));
        }
        /// <summary>
        /// 取得值班表
        /// </summary>
        /// <param name="manualTypeEnum"></param>
        /// <returns></returns>
        private List<ContactManualOnDutyReportViewModel> GetEPAOnDuty(ContactManualTypeEnum manualTypeEnum)
        {
            return ContactManualOnDutyService.GetListByOnDutyType(manualTypeEnum)
                .ConvertToModel<ContactManualOnDutyViewModel, ContactManualOnDutyReportViewModel>((input,ouput) => ouput.DateStr = input.Date.ToString("yyyy/MM/dd"));
        }
        /// <summary>
        /// 新增環保局 Table
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="manualTypeEnum"></param>
        private void AddEPBTable(WordWriteHelper helper, ContactManualTypeEnum manualTypeEnum)
        {
            helper.AddTable(manualTypeEnum.GetDescription(), HeadingType.Heading4, GetEPB(manualTypeEnum));
        }
        /// <summary>
        /// 新增環境部角色 Table
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="manualTypeEnum"></param>
        private void AddEPARoleTable(WordWriteHelper helper, ContactManualTypeEnum manualTypeEnum)
        {
            helper.AddTable(manualTypeEnum.GetDescription(), HeadingType.Heading4, GetEPARole(manualTypeEnum));
        }
        /// <summary>
        /// 取得環境部角色 Table
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<ContactManualEPARoleReportViewModel> GetEPARole(ContactManualTypeEnum type)
        {
            return ContactManualEPARoleService.GetList(type)
                .ConvertToModel<ContactManualEPARoleViewModel, ContactManualEPARoleReportViewModel>();
        }
        /// <summary>
        /// 新增監督區域 Table
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="departmentName"></param>
        /// <param name="type"></param>
        private void AddSuperviseTable(WordWriteHelper helper, string departmentName, ContactManualTypeEnum type, bool isPageBreak = false)
        {
            var supervise = GetEPASupervise(type, departmentName);
            var dic = helper.GetColumnWidthDic();
            helper.AddTable(new WordWriteModel
            {
                Title = type.GetDescription(),
                HeadingType = HeadingType.Heading4,
                Data = supervise,
                IsPageBreak = isPageBreak,
                TableAction = table =>
                {
                    var properties = typeof(ContactManualEPASuperviseReportViewModel).GetProperties().Select(e => e.GetDisplayName());

                    foreach (var item in properties.Select((Name, Index) => (Name, Index)))
                    {
                        int width = dic.GetValue(item.Name, 60);
                        table.SetColumnWidth(item.Index, width);
                    }

                    foreach (var item in table.Paragraphs)
                    {
                        item.Font("標楷體").FontSize(8);
                    }

                    if (supervise.Count > 1)
                    {
                        table.MergeCellsInColumn(0, 1, supervise.Count);
                        table.MergeCellsInColumn(1, 1, supervise.Count);
                    }
                }
            });
            AddTaiwanImage(helper);
        }
        /// <summary>
        /// 責任區域劃分表
        /// </summary>
        /// <returns></returns>
        private List<ContactManualEPASuperviseReportViewModel> GetEPASupervise(ContactManualTypeEnum typeEnum, string departmentName)
        {
            ContactManualDepartmentModel department =
                ContactManualDepartmentRepository.GetListByFilter(new ContactManualDepartmentParameter
                {
                    Name = departmentName
                }).SingleOrDefault();
            if(department == null)
            {
                return new List<ContactManualEPASuperviseReportViewModel>();
            }
            //TODO 重覆單位會造成異常
            Dictionary<int,string> supervises = ContactManualSuperviseRepository.GetList().ToDictionary(e => e.DepartmentId, e => e.Describe);

            return ContactManualService.GetListBySourceId(typeEnum, department.Id)
                .ConvertToModel<ContactManualViewModel, ContactManualEPASuperviseReportViewModel>((input,outpue)=> 
                {
                    outpue.Supervise = supervises.GetValue(department.Id);
                });
        }
        /// <summary>
        /// 取得環境部其他表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<ContactManualEPAOtherReportViewModel> GetEPAOther(ContactManualTypeEnum type)
        {
            return ContactManualService.GetListByType(type)
                .ConvertToModel<ContactManualViewModel, ContactManualEPAOtherReportViewModel>();
        }
        /// <summary>
        /// 地方環保局長通聯名冊
        /// </summary>
        /// <returns></returns>
        private List<ContactManualEPBReportViewModel> GetEPB(ContactManualTypeEnum typeEnum)
        {
            var citys = CityRepository.GetList().ToDictionary(e => e.Id, e => e.City);
            return ContactManualService.GetListByType(typeEnum)
                .ConvertToModel<ContactManualViewModel, ContactManualEPBReportViewModel>((input,output) => 
                {
                    output.CityName = citys.GetValue(input.SourceId);
                });
        }
        /// <summary>
        /// 本署環境污染事故主政單位通聯名冊
        /// </summary>
        /// <returns></returns>
        private List<ContactManualEPAReportViewModel> GetEPA(ContactManualTypeEnum type)
        {
            return ContactManualService.GetListByType(type)
                .OrderBy(e => e.DepartmentName)
                .ConvertToModel<ContactManualViewModel, ContactManualEPAReportViewModel>()
                .ToList();
        }
    }
}