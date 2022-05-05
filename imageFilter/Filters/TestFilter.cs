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
    public class TestFilter : Filter
    {
        public TestFilter()
        {
            this.Name = "Метод Оцу";
        }
        public override Mat Apply(Mat inputMat)
        {
            int channels = inputMat.Channels();
            Mat gray = new Mat();
            if (channels == 4)
            { Cv2.CvtColor(inputMat, gray, ColorConversionCodes.BGR2GRAY); }
            else { gray = inputMat; }
            Mat binary = new Mat();
            Cv2.Threshold(gray, binary, 0, 255, ThresholdTypes.Otsu);
            return binary;
        }
    }
}
