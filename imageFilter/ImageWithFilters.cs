using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using OpenCvSharp;


namespace imageFilter
{
    class ImageWithFilters
    {
        private BitmapImage bitmapImage { get; set; }
        private ObservableCollection<Filter> filters { get; set; }
        public ImageWithFilters(BitmapImage _bitmapImage, ObservableCollection<Filter> _filters) 
        {
            bitmapImage = _bitmapImage;
            filters = _filters;
        }
        public BitmapSource ApllyFilters() 
        {
            Mat imgMat = BitmapImageToMat(bitmapImage);
            foreach (Filter filter in filters) 
            {
                imgMat = filter.Apply(imgMat);
            }
            return MatToBitmapSource(imgMat);
        }
        private Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        private BitmapSource BitmapToBitmapImage(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapSource retval;

            try
            {
                retval = (BitmapSource)Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             Int32Rect.Empty,
                             BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }
        private Mat BitmapImageToMat(BitmapImage bitmapImage) 
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToMat(BitmapImageToBitmap(bitmapImage));
        }
        private BitmapSource MatToBitmapSource(Mat mat) 
        {
            return BitmapToBitmapImage(OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mat));
        }
    }
}
