using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication.Common
{
    public class FileHelper
    {
        /// <summary>
        /// 图片加水印文字
        /// </summary>
        /// <param name="oldpath">旧图片地址</param>
        /// <param name="text">水印文字</param>
        /// <param name="newpath">新图片地址</param>
        /// <param name="Alpha">透明度</param>
        /// <param name="fontsize">字体大小</param>
        public static void AddWaterText(string oldpath, string text, string newpath, int Alpha, int fontsize)
        {
            try
            {
                FileStream fs = new FileStream(oldpath, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                byte[] bytes = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();
                MemoryStream ms = new MemoryStream(bytes);
                Image imgPhoto = Image.FromStream(ms);
                int imgPhotoWidth = imgPhoto.Width;
                int imgPhotoHeight = imgPhoto.Height;

                Bitmap bmPhoto = new Bitmap(imgPhotoWidth, imgPhotoHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(bmPhoto.HorizontalResolution, bmPhoto.VerticalResolution);
                //bmPhoto.SetResolution(72, 72);
                Graphics gbmPhoto = Graphics.FromImage(bmPhoto);
                //gif背景色
                gbmPhoto.Clear(Color.FromName("white"));
                gbmPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                gbmPhoto.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gbmPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, imgPhotoWidth, imgPhotoHeight), 0, 0, imgPhotoWidth, imgPhotoHeight, GraphicsUnit.Pixel);
                Font font = null;
                SizeF crSize = new SizeF();
                font = new Font("宋体", fontsize, FontStyle.Bold);

                string[] txtArr = text.Split('$');
                for(int i=0;i<txtArr.Length;i++)
                {
                    //测量指定区域
                    crSize = gbmPhoto.MeasureString(txtArr[i], font);
                    float x = imgPhotoWidth - crSize.Width;
                    float y = imgPhotoHeight - crSize.Height * (i + 1);
                    StringFormat StrFormat = new StringFormat();
                    StrFormat.Alignment = StringAlignment.Center;

                    //画两次制造透明效果
                    SolidBrush semiTransBrushOne = new SolidBrush(Color.FromArgb(Alpha, 56, 56, 56));
                    gbmPhoto.DrawString(txtArr[i], font, semiTransBrushOne, x + 1, y + 1);

                    SolidBrush semiTransBrushTwo = new SolidBrush(Color.FromArgb(Alpha, 176, 176, 176));
                    gbmPhoto.DrawString(txtArr[i], font, semiTransBrushTwo, x, y);
                }
                bmPhoto.Save(newpath, System.Drawing.Imaging.ImageFormat.Jpeg);
                gbmPhoto.Dispose();
                imgPhoto.Dispose();
                bmPhoto.Dispose();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}