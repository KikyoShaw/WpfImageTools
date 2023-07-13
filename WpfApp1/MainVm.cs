using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

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

        /// <summary>
        /// 拼接图片
        /// </summary>
        /// <param name="inputImagePath1"></param>
        /// <param name="inputImagePath2"></param>
        /// <param name="outputImagePath"></param>
        public void CombineImagesVertically(string inputImagePath1, string inputImagePath2, string outputImagePath)
        {
            // 1. 加载图像
            using Bitmap image1 = new Bitmap(inputImagePath1);
            using Bitmap image2 = new Bitmap(inputImagePath2);

            // 2. 创建结果图像
            int outputWidth = Math.Max(image1.Width, image2.Width);
            int outputHeight = image1.Height + image2.Height;
            using Bitmap outputImage = new Bitmap(outputWidth, outputHeight);

            // 3. 拼接图像
            using (Graphics graphics = Graphics.FromImage(outputImage)) //下面可以更改是横向拼接还是纵向拼接
            {
                // 画第一张图片
                graphics.DrawImage(image1, 0, 0, image1.Width, image1.Height);
                // 画第二张图片
                graphics.DrawImage(image2, 0, image1.Height, image2.Width, image2.Height);
            } //也可以控制拼接的位置

            // 4. 保存结果图像
            outputImage.Save(outputImagePath, ImageFormat.Png);
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="inputImagePath"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="outputImagePath"></param>
        public void ResizeImage(string inputImagePath, string outputImagePath, int maxWidth = 0, int maxHeight = 0)
        {
            // 1. 加载图像
            using Bitmap originalBitmap = new Bitmap(inputImagePath);

            int originalWidth = originalBitmap.Width;
            int originalHeight = originalBitmap.Height;

            if (maxWidth == 0)
                maxWidth = originalWidth;

            if(maxHeight == 0)
                maxHeight = originalHeight;

            float ratioX = (float)maxWidth / (float)originalWidth;
            float ratioY = (float)maxHeight / (float)originalHeight;

            float ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);

            using Bitmap resizedBitmap = new Bitmap(newWidth, newHeight);

            //裁剪操作
            using (Graphics graphics = Graphics.FromImage(resizedBitmap))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(originalBitmap, 0, 0, newWidth, newHeight);
            }

            // 保存结果图像
            resizedBitmap.Save(outputImagePath, ImageFormat.Png);
        }

        /// <summary>
        /// 按坐标点裁剪
        /// </summary>
        /// <param name="inputImagePath"></param>
        /// <param name="outputImagePath"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CropImage(string inputImagePath, string outputImagePath, int newWidth, int newHeight, int x, int y)
        {
            // 1. 加载图像
            using Bitmap originalBitmap = new Bitmap(inputImagePath);

            Rectangle cropArea = new Rectangle(x, y, newWidth, newHeight);
            using Bitmap croppedBitmap = originalBitmap.Clone(cropArea, originalBitmap.PixelFormat);

            //裁剪操作
            using MemoryStream croppedStream = new MemoryStream();
            croppedBitmap.Save(croppedStream, ImageFormat.Png);

            // 保存结果图像
            croppedBitmap.Save(outputImagePath, ImageFormat.Png);
        }

    }
}
