using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace imageFilter
{
    class ImageWithFilters
    {
        BitmapImage bitmapImage { get; set; }
        List<Filter> Filters { get; set; } 
    }
}
