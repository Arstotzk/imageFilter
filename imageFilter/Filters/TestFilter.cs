using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace imageFilter.Filters
{
    public class TestFilter : Filter, IFilter
    {
        public TestFilter()
        {
            this.Name = "Метод Оцу";
        }
        public BitmapSource Apply(BitmapImage bitmapImage)
        {
            var gray = OpenCvSharp.Extensions.BitmapConverter.ToMat(BitmapImage2Bitmap(bitmapImage));
            var gray2 = new Mat();
            Cv2.CvtColor(gray,gray2, ColorConversionCodes.BGR2GRAY);
            var binary = new Mat();
            Cv2.Threshold(gray2, binary, 0, 255, ThresholdTypes.Otsu);
            return Bitmap2BitmapImage(OpenCvSharp.Extensions.BitmapConverter.ToBitmap(binary));
        }
    }
}
