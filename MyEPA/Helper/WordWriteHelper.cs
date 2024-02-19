using MyEPA.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Xceed.Document.NET;
using Xceed.Words.NET;
using Image = Xceed.Document.NET.Image;

namespace MyEPA.Helper
{
    public enum WordWriteDataTypeEnum
    {
        Table = 1,
        Image = 2,
        Str = 3,
        DocX = 4,
    }
    public class WordWriteModel
    {
        /// <summary>
        /// 資料格式
        /// </summary>
        public WordWriteDataTypeEnum DataType { get; set; }
        /// <summary>
        /// 資料標題
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 資料
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// HeadingType 設定
        /// </summary>
        public HeadingType HeadingType { get; set; }
        /// <summary>
        /// 是否換頁
        /// </summary>
        public bool IsPageBreak { get; set; }
        public Action<Table> TableAction { get; set; }
    }
    public class WordWriteHelper
    {
        
        Dictionary<HeadingType, int> FontSizeDic = new Dictionary<HeadingType, int>
        {
            { HeadingType.Heading1,24 },
            { HeadingType.Heading2,20 },
            { HeadingType.Heading3,16 },
            { HeadingType.Heading4,14 }
        };

        Dictionary<string, int> ColumnWidthDic = new Dictionary<string, int>
        {
            { "單位",35 },
            { "單位別",60 },
            { "職稱",45 },
            { "姓名",35 },
            { "總機",65 },
            { "分機",30 },
            { "電話",65 },
            { "傳真電話",65 },
            { "住宅電話",65 },
            { "行動電話",65 },
            { "傳真",65 },
            { "備註",35 },
            { "日期",120 },
            { "督導業務",240 },
            { "職務名稱",90 },
        };

        public Dictionary<string, int> GetColumnWidthDic()
        {
            return ColumnWidthDic;
        }

        List<WordWriteModel> _Pages = new List<WordWriteModel>();
        List<string> WordTitles = new List<string>();
        /// <summary>
        /// 在目錄之後的標題(目錄會產生)
        /// </summary>
        /// <param name="title"></param>
        /// <param name="headingType"></param>
        /// <param name="isPageBreak"></param>
        public void AddMainTitle(string title, HeadingType headingType, bool isPageBreak = true)
        {
            _Pages.Add(new WordWriteModel
            {
                DataType = WordWriteDataTypeEnum.Str,
                Data = null,
                Title = title,
                HeadingType = headingType,
                IsPageBreak = isPageBreak
            });
        }

        /// <summary>
        /// 在目錄之前的標題
        /// </summary>
        /// <param name="wordTitle"></param>
        public void AddWordTitle(string wordTitle)
        {
            WordTitles.Add(wordTitle);
        }
        public void AddImage(string imagePath, bool isPageBreak)
        {
            _Pages.Add(new WordWriteModel
            {
                DataType = WordWriteDataTypeEnum.Image,
                Data = imagePath,
                IsPageBreak = isPageBreak
            });
        }
        public void AddTable(WordWriteModel wordWrite)
        {
            wordWrite.DataType = WordWriteDataTypeEnum.Table;
            _Pages.Add(wordWrite);
        }
        public void AddDocX(DocX doc, bool isPageBreak = true)
        {
            _Pages.Add(new WordWriteModel
            {
                DataType = WordWriteDataTypeEnum.DocX,
                Data = doc,
                Title = string.Empty,
                IsPageBreak = isPageBreak
            });
        }
        
        public void AddTable<T>(string title, HeadingType type, List<T> inputData, bool isPageBreak = true)
        {
            _Pages.Add(new WordWriteModel
            {
                DataType = WordWriteDataTypeEnum.Table,
                Data = inputData,
                Title = title,
                HeadingType = type,
                IsPageBreak = isPageBreak,
                TableAction = table => 
                {
                    var properties = typeof(T).GetProperties().Select(e => e.GetDisplayName());
                    foreach (var item in properties.Select((Name,Index)=> (Name, Index)))
                    {
                        int width = ColumnWidthDic.GetValue(item.Name, 60);
                        table.SetColumnWidth(item.Index, width);
                    }
                    
                    foreach (var item in table.Paragraphs)
                    {
                        item.Font("標楷體").FontSize(8);
                    }
                }
            });
        }
        public Stream GetStreamResult()
        {
            Stream stream = new MemoryStream();

            using (DocX doc = DocX.Create(stream, DocumentTypes.Document))
            {
                //新增頁碼
                doc.AddFooters();
                Footer footer = doc.Footers.Odd;
                footer.PageNumbers = true;

                foreach (var wordTitle in WordTitles)
                {
                    doc.InsertParagraph(wordTitle).FontSize(24).Bold(true).Font("標楷體").Color(Color.Black).Alignment = Alignment.center;
                }                
                doc.InsertSectionPageBreak();

                SetTableOfContents(doc);
              
                foreach (var page in _Pages)
                {
                    switch (page.DataType)
                    {
                        case WordWriteDataTypeEnum.Str:
                            {
                                //標題
                                int fontSize = FontSizeDic.GetValue(page.HeadingType, 12);
                                Paragraph paragraph = doc.InsertParagraph(page.Title).FontSize(fontSize).Bold(true).Heading(page.HeadingType).Font("標楷體").Color(Color.Black);
                                if (page.HeadingType == HeadingType.Heading1)
                                {
                                    paragraph.Alignment = Alignment.center;
                                }
                            }
                            break;
                        case WordWriteDataTypeEnum.Image:
                            {
                                Image image = doc.AddImage((string)page.Data);
                                Picture picture = image.CreatePicture();
                                Paragraph paragraph = doc.InsertParagraph();
                                paragraph.AppendLine();
                                paragraph.AppendPicture(picture);
                                paragraph.Alignment = Alignment.center;
                            }
                            break;
                        case WordWriteDataTypeEnum.DocX:
                            {
                                doc.SetDefaultFont(new Xceed.Document.NET.Font("標楷體"));
                                doc.InsertDocument((DocX)page.Data);
                            }
                            break;
                        case WordWriteDataTypeEnum.Table:
                            {
                                //標題
                                int fontSize = FontSizeDic.GetValue(page.HeadingType, 12);
                                Paragraph paragraph = doc.InsertParagraph(page.Title).FontSize(fontSize).Bold(true).Heading(page.HeadingType).Font("標楷體").Color(Color.Black);
                                //標題置中
                                if (page.HeadingType == HeadingType.Heading1)
                                {
                                    paragraph.Alignment = Alignment.center;
                                }
                                //內容
                                if (page.Data != null)
                                {
                                   doc.InsertTable((IEnumerable<dynamic>)page.Data, page.TableAction);
                                }
                            }
                            break;
                    }
                    //換頁
                    if (page.IsPageBreak)
                    {
                        doc.InsertSectionPageBreak();
                    }
                }
                
                doc.SaveAs(stream);
                stream.Position = 0;
                return stream;
            }
        }

        /// <summary>
        /// 目錄設定
        /// </summary>
        private void SetTableOfContents(DocX doc)
        {
            doc.DifferentFirstPage = false;
            //宣告目錄內容
            //https://doc.xceed.com/xceed-document-libraries-for-net/Xceed.Document.NET~Xceed.Document.NET.TableOfContentsSwitches.html
            var tocSwitches = new Dictionary<TableOfContentsSwitches, string>()
                                {
                                   //Heading1 標題階層1-3
                                  { TableOfContentsSwitches.O, "1-3"},
                                  { TableOfContentsSwitches.U, ""},
                                  { TableOfContentsSwitches.Z, ""},
                                  { TableOfContentsSwitches.H, ""},
                                };
            doc.InsertTableOfContents("目錄", tocSwitches);
            //換頁
            doc.InsertSectionPageBreak();
        }
    }
}