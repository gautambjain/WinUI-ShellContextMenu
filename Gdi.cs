using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Runtime.InteropServices;
using Windows.Foundation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace Projzo.Interop
{

    // To add more gdi based apis, refer below link and project
    // https://github.com/castorix/WinUI3_MediaEngine/blob/master/CMediaEngine.cs
    public class Gdi
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAP
        {
            public int bmType;
            public int bmWidth;
            public int bmHeight;
            public int bmWidthBytes;
            public short bmPlanes;
            public short bmBitsPixel;
            public IntPtr bmBits;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPV5HEADER
        {
            public int bV5Size;
            public int bV5Width;
            public int bV5Height;
            public short bV5Planes;
            public short bV5BitCount;
            public int bV5Compression;
            public int bV5SizeImage;
            public int bV5XPelsPerMeter;
            public int bV5YPelsPerMeter;
            public int bV5ClrUsed;
            public int bV5ClrImportant;
            public int bV5RedMask;
            public int bV5GreenMask;
            public int bV5BlueMask;
            public int bV5AlphaMask;
            public int bV5CSType;
            public CIEXYZTRIPLE bV5Endpoints;
            public int bV5GammaRed;
            public int bV5GammaGreen;
            public int bV5GammaBlue;
            public int bV5Intent;
            public int bV5ProfileData;
            public int bV5ProfileSize;
            public int bV5Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CIEXYZTRIPLE
        {
            public CIEXYZ ciexyzRed;
            public CIEXYZ ciexyzGreen;
            public CIEXYZ ciexyzBlue;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CIEXYZ
        {
            public int ciexyzX;
            public int ciexyzY;
            public int ciexyzZ;
        }


        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool DeleteObject ([In] IntPtr hObject);


        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetObject(IntPtr hFont, int nSize, out BITMAP bm);

        public const int BI_BITFIELDS = 3;
        public const int DIB_RGB_COLORS = 0;

        [DllImport("User32", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("User32", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool DeleteDC(IntPtr hDC);


        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int FillRect(IntPtr hDC, [In] ref RECT lprc, IntPtr hbr);


        [DllImport("gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateSolidBrush(int crColor);


        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetDIBits(IntPtr hdc, IntPtr hbm, uint start, uint cLines, byte[] lpvBits, ref BITMAPV5HEADER lpbmi, uint usage);


        public static WriteableBitmap GetCaptureWriteableBitmap(IntPtr hBitmap)
        {
            WriteableBitmap writeableBitmap = null;

            if (hBitmap != IntPtr.Zero)
            {
                BITMAP bm;
                GetObject(hBitmap, Marshal.SizeOf(typeof(BITMAP)), out bm);
                int nWidth = bm.bmWidth;
                int nHeight = bm.bmHeight;
                BITMAPV5HEADER bi = new BITMAPV5HEADER();
                bi.bV5Size = Marshal.SizeOf(typeof(BITMAPV5HEADER));
                bi.bV5Width = nWidth;
                bi.bV5Height = -nHeight;
                bi.bV5Planes = 1;
                bi.bV5BitCount = 32;
                bi.bV5Compression = BI_BITFIELDS;
                bi.bV5AlphaMask = unchecked((int)0xFF000000);
                bi.bV5RedMask = 0x00FF0000;
                bi.bV5GreenMask = 0x0000FF00;
                bi.bV5BlueMask = 0x000000FF;

                IntPtr hDC = CreateCompatibleDC(IntPtr.Zero);
                IntPtr hBitmapOld = SelectObject(hDC, hBitmap);
                int nNumBytes = (int)(nWidth * 4 * nHeight);
                byte[] pPixels = new byte[nNumBytes];
                int nScanLines = GetDIBits(hDC, hBitmap, 0, (uint)nHeight, pPixels, ref bi, DIB_RGB_COLORS);

                writeableBitmap = new WriteableBitmap(nWidth, nHeight);
                writeableBitmap.PixelBuffer.AsStream().Write(pPixels, 0, pPixels.Length);

                SelectObject(hDC, hBitmapOld);
                DeleteDC(hDC);
            }

            return writeableBitmap;
        }

    }


}
