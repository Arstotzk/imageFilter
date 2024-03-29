﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace imageFilter.Filters
{
    class Dilatation : Filter
    {
        public PropertyValue Size = new PropertyValue();
        public Dilatation()
        {
            this.Name = "Дилатация";
            this.Size.Name = "Размер";
            this.Size.Value = 5;
        }
        public override Mat Apply(Mat inputMat)
        {
            Mat outputMat = new Mat();
            Mat element = Cv2.GetStructuringElement(MorphShapes.Ellipse,
                new OpenCvSharp.Size(this.Size.Value * 2 + 1, this.Size.Value * 2 + 1),
                new Point(this.Size.Value, this.Size.Value));
            Cv2.Dilate(inputMat, outputMat, element);
            return outputMat;
        }
    }
}
