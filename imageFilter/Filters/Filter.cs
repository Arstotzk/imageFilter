using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using System.Drawing;
using System.IO;

namespace imageFilter
{
    [Serializable]
    abstract public class Filter
    {
        public string Name { get; set; }

        public virtual Mat Apply(Mat inputMat) 
        {
            return inputMat;
        }
    }
}
