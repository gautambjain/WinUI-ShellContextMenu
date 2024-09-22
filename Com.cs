using System;
using System.Runtime.InteropServices;

namespace Projzo.Interop
{


    [Flags]
    public enum ClassContext : uint
    {
        InprocServer = 0x1,
        InprocHandler = 0x2,
        Inproc = InprocServer | InprocHandler,
        LocalServer = 0x4

    }

    public class Com
    {
        public static Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");


        [DllImport("ole32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern HResult CoCreateInstance(ref Guid rclsid, IntPtr pUnkOuter, ClassContext dwClsContext, ref Guid riid, out IntPtr ppv);

    }



}
