using System;
using System.Runtime.InteropServices;
using Windows.Foundation;

namespace Projzo.Interop
{
    public class Dpi
    {
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint GetDpiForWindow(IntPtr hwnd);

        [DllImport("User32.dll", SetLastError =true, CharSet = CharSet.Unicode)]
        public static extern uint GetDpiForSystem();


        private static double GetScaleFactor()
        {
            uint dpi = GetDpiForSystem();
            return ((double)dpi) / 96;
        }

        private static double GetUnscaleFactor()
        {
            uint dpi = GetDpiForSystem();
            return ((double)96) / dpi;
        }


        public static Point ScalePoint(Point pt)
        {
            double scaleFactor = GetScaleFactor();

            return new Point { X = pt.X * scaleFactor, Y = pt.Y * scaleFactor };

        }

        public static Rect ScaleRect(Rect rect)
        {
            double scaleFactor = GetScaleFactor();

            return new Rect { 
                X = rect.X * scaleFactor,
                Y = rect.Y * scaleFactor,
                Width = (rect.Width) * scaleFactor,
                Height = (rect.Height) * scaleFactor,
            };
            
        }

        public static RECT ScaleRECT(RECT rect)
        {
            double scaleFactor = GetScaleFactor();

            return new RECT
            {
                left = (int)(rect.left * scaleFactor),
                top = (int)(rect.top * scaleFactor),
                right = (int)(rect.right * scaleFactor),
                bottom = (int)(rect.bottom * scaleFactor)
            };

        }


        public static Point UnscalePoint(Point pt)
        {
            double scaleFactor = GetUnscaleFactor();

            return new Point { X = pt.X * scaleFactor, Y = pt.Y * scaleFactor };

        }

        public static Rect UnscaleRect(Rect rect)
        {
            double scaleFactor = GetUnscaleFactor();

            return new Rect
            {
                X = rect.X * scaleFactor,
                Y = rect.Y * scaleFactor,
                Width = (rect.Width) * scaleFactor,
                Height = (rect.Height) * scaleFactor,
            };

        }

        public static RECT UnscaleRECT(RECT rect)
        {
            double scaleFactor = GetUnscaleFactor();

            return new RECT
            {
                left = (int)(rect.left * scaleFactor),
                top = (int)(rect.top * scaleFactor),
                right = (int)(rect.right * scaleFactor),
                bottom = (int)(rect.bottom * scaleFactor)
            };

        }

    }


}
