using Xceed.Document.NET;
using Xceed.Words.NET;

namespace MyEPA.Helper
{
    public class WordTableHelper
    {
        static Table _tb = null;
        public Table GetTable()
        {
            _tb.AutoFit = AutoFit.Contents;
            return _tb;
        }
        public WordTableHelper(DocX doc, int row, int cell)
        {
            //定義長寬(因為標題所以要+1)
            _tb = doc.AddTable(row + 1, cell);
            _tb.Design = TableDesign.TableGrid;
            _tb.Alignment = Alignment.center;
        }
        private int cell = 0;
        private int row = 0;
        /// <summary>
        /// 設定cell值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetCellValue(string value)
        {
            _tb.Rows[row].Cells[cell].Paragraphs[0].Append(value);
            this.cell++;
        }
        public void SetRowValue(string value)
        {
            _tb.Rows[row].Cells[cell].Paragraphs[0].Append(value);
            this.row++;
        }
        /// <summary>
        /// 設定Cell位置
        /// </summary>
        /// <param name="cell"></param>
        public void SetPosCell(int cell)
        {
            this.cell = cell;
        }
        /// <summary>
        /// 設定Row位置
        /// </summary>
        /// <param name="row"></param>
        public void SetPosRow(int row)
        {
            this.row = row;
        }
        /// <summary>
        /// 設定位置
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="row"></param>
        public void SetPos(int cell, int row)
        {
            this.cell = cell;
            this.row = row;
        }
        /// <summary>
        /// 跳過幾個 cell
        /// </summary>
        /// <param name="cell"></param>
        public void SkipCell(int cell)
        {
            this.cell += cell;
        }
        /// <summary>
        /// 跳過幾個 row
        /// </summary>
        /// <param name="row"></param>
        public void SkipRow(int row)
        {
            this.row += row;
        }
        /// <summary>
        /// 設定cell值後，至下 n 個row (跳行)
        /// 且 cell 會回到第一個
        /// </summary>
        public void NextRow(int row = 1)
        {
            cell = 0;
            this.row += row;
        }
        /// <summary>
        /// 設定Row值,至下 n 個cell (跳欄)
        /// 且 row 會回到第一個
        /// </summary>
        /// <param name="cell"></param>
        public void NextCell(int cell = 1)
        {
            row = 0;
            this.cell += cell;
        }
    }
}