using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenCvSharp;

namespace imageFilter.Filters
{
    class Laplacian : Filter
    {
        public PropertyValue Size = new PropertyValue();
        public PropertyValue scale = new PropertyValue();
        public PropertyValue delta = new PropertyValue();
        public Laplacian()
        {
            this.Name = "Фильтр Лапласа";
            this.Size.Name = "Размер";
            this.Size.Value = 5;
            this.scale.Name = "Коэфициент масштабирования";
            this.scale.Value = 1;
            this.delta.Name = "Дельта";
            this.delta.Value = 0;
        }
        public override Mat Apply(Mat inputMat)
        {
            Mat outputMat = new Mat();
            Mat tmpMat = new Mat();
            int channels = inputMat.Channels();
            Mat gray = new Mat();
            if (channels == 4)
            { Cv2.CvtColor(inputMat, gray, ColorConversionCodes.BGR2GRAY); }
            else { gray = inputMat; }
            try
            {
                Cv2.Laplacian(gray, tmpMat,
                    MatType.CV_8U, (int)this.Size.Value,
                    this.scale.Value, this.delta.Value);
                Cv2.ConvertScaleAbs(tmpMat, outputMat);
                return tmpMat;
            }
            catch
            {
                return inputMat;
            }
        }
    }
}
