using System;
using System.Runtime.InteropServices;


namespace Projzo.Interop
{

    internal static class Stream
    {

        [DllImport("shlwapi.dll", CallingConvention = CallingConvention.StdCall, PreserveSig = true, CharSet = CharSet.Unicode)]
        static extern HResult SHCreateStreamOnFileEx(string pszFile, StorageMode grfMode, uint dwAttributes, uint fCreate, IntPtr pstmTemplate, out IntPtr ppstm);

        [DllImport("shell32.dll", CallingConvention = CallingConvention.StdCall, PreserveSig = true, CharSet = CharSet.Unicode)]
        static extern HResult SHCreateItemFromParsingName(string pszPath, IntPtr pbc, ref Guid riid, out IntPtr ppv);

        static readonly Guid IShellItemIid = Guid.ParseExact("43826d1e-e718-42ee-bc55-a1e261c37bfe", "d");

        public static IntPtr IShellItemFromPath(string path)
        {
            IntPtr psi;
            Guid iid = IShellItemIid;
            var hr = SHCreateItemFromParsingName(path, IntPtr.Zero, ref iid, out psi);
            if ((int)hr < 0)
                return IntPtr.Zero;
            return psi;
        }

        public static IntPtr IStreamFromPath(string path)
        {
            IntPtr pstm;
            var hr = SHCreateStreamOnFileEx(path,
                StorageMode.Read | StorageMode.FailIfThere | StorageMode.ShareDenyNone,
                0, 0, IntPtr.Zero, out pstm);
            if ((int)hr < 0)
                return IntPtr.Zero;
            return pstm;
        }

        public static void ReleaseObject(IntPtr obj)
        {
            Marshal.Release(obj);
        }
    }


    [Flags]
    public enum StorageMode : uint
    {
        Read = 0x0,
        Write = 0x1,
        ReadWrite = 0x2,
        ShareDenyNone = 0x40,
        ShareDenyRead = 0x30,
        ShareDenyWrite = 0x20,
        ShareExclusive = 0x10,
        Priority = 0x40000,
        Create = 0x1000,
        Convert = 0x20000,
        FailIfThere = 0x0,
        Direct = 0x0,
        Transacted = 0x10000,
        NoScratch = 0x100000,
        NoSnapshot = 0x200000,
        Simple = 0x8000000,
        DirectSingleWriterMultipleReaders = 0x400000,
        DeleteOnRelease = 0x4000000
    }



    // Indicates the type of storage medium being used in a data transfer
    [Flags]
    public enum TYMED
    {
        ENHMF = 0x40,
        FILE = 2,
        GDI = 0x10,
        HGLOBAL = 1,
        ISTORAGE = 8,
        ISTREAM = 4,
        MFPICT = 0x20,
        NULL = 0
    }


    // A generalized global memory handle used for data transfer operations by the 
    // IAdviseSink, IDataObject, and IOleCache interfaces
    [StructLayout(LayoutKind.Sequential)]
    public struct STGMEDIUM
    {
        public TYMED tymed;
        public IntPtr hBitmap;
        public IntPtr hMetaFilePict;
        public IntPtr hEnhMetaFile;
        public IntPtr hGlobal;
        public IntPtr lpszFileName;
        public IntPtr pstm;
        public IntPtr pstg;
        public IntPtr pUnkForRelease;
    }


}
