using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace imageFilter.Filters
{
    class Dilatation : Filter, IFilter
    {
        public PropertyValue Size = new PropertyValue();
        public Dilatation()
        {
            this.Name = "Дилатация";
            this.Size.Name = "Размер";
            this.Size.Value = 5;
        }
        public BitmapSource Apply(BitmapImage bitmapImage)
        {
            var gray = OpenCvSharp.Extensions.BitmapConverter.ToMat(BitmapImage2Bitmap(bitmapImage));
            var gray2 = new Mat();
            Cv2.CvtColor(gray, gray2, ColorConversionCodes.BGR2GRAY);
            var binary = new Mat();
            Cv2.Threshold(gray2, binary, 0, 255, ThresholdTypes.Otsu);
            return Bitmap2BitmapImage(OpenCvSharp.Extensions.BitmapConverter.ToBitmap(binary));
        }
    }
}
