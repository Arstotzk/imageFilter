using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenCvSharp;

namespace imageFilter.Filters
{
    class Gauss : Filter
    {
        public PropertyValue Size = new PropertyValue();
        public PropertyValue sigmaX = new PropertyValue();
        public PropertyValue sigmaY = new PropertyValue();
        public Gauss()
        {
            this.Name = "Размытие по Гауссу";
            this.Size.Name = "Размер";
            this.Size.Value = 5;
            this.sigmaX.Name = "Отклонение ядра по X";
            this.sigmaX.Value = 0;
            this.sigmaY.Name = "Отклонение ядра по Y";
            this.sigmaY.Value = 0;
        }
        public override Mat Apply(Mat inputMat)
        {
            Mat outputMat = new Mat();
            Cv2.GaussianBlur(inputMat, outputMat, 
                new Size(this.Size.Value * 2 + 1, this.Size.Value * 2 + 1), 
                this.sigmaX.Value, this.sigmaY.Value);
            return outputMat;
        }
    }
}
