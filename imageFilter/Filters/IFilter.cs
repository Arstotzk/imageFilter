using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace imageFilter
{
    interface IFilter
    {
        BitmapSource Apply(BitmapImage bitmapImage);
    }
}
