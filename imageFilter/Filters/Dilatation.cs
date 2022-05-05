using System;
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
    }
}
