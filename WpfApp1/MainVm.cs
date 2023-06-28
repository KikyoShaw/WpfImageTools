using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows;

namespace WpfApp1
{
    public class MainVm
    {
        private static readonly Lazy<MainVm> Lazy = new Lazy<MainVm>(() => new MainVm());
        public static MainVm Instance => Lazy.Value;

        private MainVm()
        {
        }

        /// <summary>
        /// 添加水印
        /// </summary>
        /// <param name="inputImagePath"></param>
        /// <param name="outputImagePath"></param>
        /// <param name="watermarkText"></param>
        public void AddWatermarkToImage(string inputImagePath, string outputImagePath, string watermarkText)
        {
            using Image image = Image.FromFile(inputImagePath);
            using Bitmap bitmap = new Bitmap(image);
            using Graphics graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Font font = new Font("Arial", 30, System.Drawing.FontStyle.Bold, GraphicsUnit.Pixel)) //字体
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(128, 255, 255, 255))) //颜色
            {
                SizeF textSize = graphics.MeasureString(watermarkText, font);
                PointF position = new PointF(image.Width - textSize.Width - 20, image.Height - textSize.Height - 20); //水印位置
                graphics.DrawString(watermarkText, font, brush, position);
            }

            bitmap.Save(outputImagePath, ImageFormat.Jpeg);
        }

    }
}
