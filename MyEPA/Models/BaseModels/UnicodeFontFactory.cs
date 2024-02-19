using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

namespace MyEPA.Models
{
    public class UnicodeFontFactory : FontFactoryImp
    {
        //arial unicode MS是完整的unicode字型。
        private static readonly string arialFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arialuni.ttf");
        //標楷體
        private static readonly string 標楷體Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "KAIU.TTF");

        public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color, bool cached)
        {
            //可用Arial或標楷體，自己選一個
            BaseFont baseFont = BaseFont.CreateFont(標楷體Path, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            return new Font(baseFont, size, style, color);
        }
    }
}