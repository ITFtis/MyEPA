using MyEPA.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;

namespace MyEPA.Helper
{
    public static class ValidateCodeHelper
    {
        private static List<SolidBrush> GetDarkList()
        {
            var brushs = (typeof(Brushes).GetProperties())
                .Select(e => ((SolidBrush)e.GetValue(null, null)))
                .Where(e => IsDarkColor(e))
                .ToList();
            return brushs;

        }
        private static List<SolidBrush> GetLightList()
        {
            var brushs = (typeof(Brushes).GetProperties())
                .Select(e => ((SolidBrush)e.GetValue(null, null)))
                .Where(e => IsDarkColor(e) == false && (e).Color.Name.Contains("White") == false)
                .ToList();
            return brushs;

        }
        private static bool IsDarkColor(SolidBrush brush)
        {
            var sum = brush.Color.R * 0.299 + brush.Color.G * 0.587 + brush.Color.B * 0.114;
            return sum < 150;
        }
        public static ValidateCodeModel GetValidateCode(int length = 5)
        {
            var brushs = GetDarkList();
            
            ValidateCodeModel result = new ValidateCodeModel 
            {
                Code = RandomHelper.GetRand(length)
            };

            int width = length * 25;
            int height = length * 8;
            int spacing = (int)(width * 0.9 / length);
            //定義一個畫板
            MemoryStream ms = new MemoryStream();
            using (Bitmap map = new Bitmap(width, height))
            {
                //畫筆,在指定畫板畫板上畫圖
                //g.Dispose();
                using (Graphics g = Graphics.FromImage(map))
                {
                    g.Clear(Color.White);
                    int i = 0;
                    foreach (var item in result.Code)
                    {
                        Brush brush = brushs[RandomHelper.GetRandIntegerNumbers(0, brushs.Count - 1)];
                        //g.DrawString(item.ToString(), new Font("黑體", 18.0F), brushs[randNumber], new Point(4 + i * spacing, 8));

                        RectangleF rc = RectangleF.FromLTRB(i * spacing, 0, width, height);

                        StringFormat format = new StringFormat();

                        format.LineAlignment = StringAlignment.Center;

                        int angle = RandomHelper.GetRandIntegerNumbers(-45, 45);

                        DrawString(g, item.ToString(), new Font("黑體", 18.0F), brush, rc, format, angle);
                        i++;
                    }
                   
                    //繪製干擾線(數字代表幾條)
                    PaintInterLine(g, 5, map.Width, map.Height);
                }
                map.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            result.Image = ms.GetBuffer();
            return result;
        }
        private static void PaintInterLine(Graphics g, int num, int width, int height)
        {
            var brushs = GetLightList();
            Random r = new Random();
            int startX, startY, endX, endY;
            for (int i = 0; i < num; i++)
            {
                startX = r.Next(0, width);
                startY = r.Next(0, height);
                endX = r.Next(0, width);
                endY = r.Next(0, height);
                int randNumber = RandomHelper.GetRandIntegerNumbers(0, brushs.Count - 1);
                g.DrawLine(new Pen(brushs[randNumber]), startX, startY, endX, endY);
            }
        }

        /// <summary>  
        /// 绘制根据矩形旋转文本  
        /// </summary>  
        /// <param name="s">文本</param>  
        /// <param name="font">字体</param>  
        /// <param name="brush">填充</param>  
        /// <param name="layoutRectangle">局部矩形</param>  
        /// <param name="format">布局方式</param>  
        /// <param name="angle">角度</param>  
        public static void DrawString(Graphics g, string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format, float angle)
        {
            // 求取字符串大小  
            SizeF size = g.MeasureString(s, font);

            // 根据旋转角度，求取旋转后字符串大小  
            SizeF sizeRotate = ConvertSize(size, angle);

            // 根据旋转后尺寸、布局矩形、布局方式计算文本旋转点  
            PointF rotatePt = GetRotatePoint(sizeRotate, layoutRectangle, format);

            // 重设布局方式都为Center  
            StringFormat newFormat = new StringFormat(format);
            newFormat.Alignment = StringAlignment.Center;
            newFormat.LineAlignment = StringAlignment.Center;

            // 绘制旋转后文本  
            DrawString(g, s, font, brush, rotatePt, newFormat, angle);
        }

        /// <summary>  
        /// 绘制根据点旋转文本，一般旋转点给定位文本包围盒中心点  
        /// </summary>  
        /// <param name="s">文本</param>  
        /// <param name="font">字体</param>  
        /// <param name="brush">填充</param>  
        /// <param name="point">旋转点</param>  
        /// <param name="format">布局方式</param>  
        /// <param name="angle">角度</param>  
        public static void DrawString(Graphics _graphics, string s, Font font, Brush brush, PointF point, StringFormat format, float angle)
        {
            // Save the matrix  
            Matrix mtxSave = _graphics.Transform;

            Matrix mtxRotate = _graphics.Transform;
            mtxRotate.RotateAt(angle, point);
            _graphics.Transform = mtxRotate;

            _graphics.DrawString(s, font, brush, point, format);

            // Reset the matrix  
            _graphics.Transform = mtxSave;
        }
        private static SizeF ConvertSize(SizeF size, float angle)
        {
            Matrix matrix = new Matrix();
            matrix.Rotate(angle);

            // 旋转矩形四个顶点  
            PointF[] pts = new PointF[4];
            pts[0].X = -size.Width / 2f;
            pts[0].Y = -size.Height / 2f;
            pts[1].X = -size.Width / 2f;
            pts[1].Y = size.Height / 2f;
            pts[2].X = size.Width / 2f;
            pts[2].Y = size.Height / 2f;
            pts[3].X = size.Width / 2f;
            pts[3].Y = -size.Height / 2f;
            matrix.TransformPoints(pts);

            // 求取四个顶点的包围盒  
            float left = float.MaxValue;
            float right = float.MinValue;
            float top = float.MaxValue;
            float bottom = float.MinValue;

            foreach (PointF pt in pts)
            {
                // 求取并集  
                if (pt.X < left)
                    left = pt.X;
                if (pt.X > right)
                    right = pt.X;
                if (pt.Y < top)
                    top = pt.Y;
                if (pt.Y > bottom)
                    bottom = pt.Y;
            }

            SizeF result = new SizeF(right - left, bottom - top);
            return result;
        }
        private static PointF GetRotatePoint(SizeF size, RectangleF layoutRectangle, StringFormat format)
        {
            PointF pt = new PointF();

            switch (format.Alignment)
            {
                case StringAlignment.Near:
                    pt.X = layoutRectangle.Left + size.Width / 2f;
                    break;
                case StringAlignment.Center:
                    pt.X = (layoutRectangle.Left + layoutRectangle.Right) / 2f;
                    break;
                case StringAlignment.Far:
                    pt.X = layoutRectangle.Right - size.Width / 2f;
                    break;
                default:
                    break;
            }

            switch (format.LineAlignment)
            {
                case StringAlignment.Near:
                    pt.Y = layoutRectangle.Top + size.Height / 2f;
                    break;
                case StringAlignment.Center:
                    pt.Y = (layoutRectangle.Top + layoutRectangle.Bottom) / 2f;
                    break;
                case StringAlignment.Far:
                    pt.Y = layoutRectangle.Bottom - size.Height / 2f;
                    break;
                default:
                    break;
            }

            return pt;
        }

    }
}