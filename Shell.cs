using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using Projzo.Model;
using static Projzo.Interop.ThumbnailProvider;


namespace Projzo.Interop
{

    public enum HResult : int
    {
        S_OK = 0,
        S_FALSE = 1,
        E_ABORT = -2147467260,
        E_ACCESSDENIED = -2147024891,
        E_FAIL = -2147467259,
        E_HANDLE = -2147024890,
        E_INVALIDARG = -2147024809,
        E_NOINTERFACE = -2147467262,
        E_NOTIMPL = -2147467263,
        E_OUTOFMEMORY = -2147024882,
        E_POINTER = -2147467261,
        E_UNEXPECTED = -2147418113,
        CO_E_SERVER_EXEC_FAILURE = -2146959355,
        REGDB_E_CLASSNOTREG = -2147221164
    }


    // Defines the values used with the IShellFolder::GetDisplayNameOf and IShellFolder::SetNameOf 
    // methods to specify the type of file or folder names used by those methods
    [Flags]
    public enum SHGNO
    {
        NORMAL = 0x0000,
        INFOLDER = 0x0001,
        FOREDITING = 0x1000,
        FORADDRESSBAR = 0x4000,
        FORPARSING = 0x8000
    }

    // The attributes that the caller is requesting, when calling IShellFolder::GetAttributesOf
    [Flags]
    public enum SFGAO : uint
    {
        BROWSABLE = 0x8000000,
        CANCOPY = 1,
        CANDELETE = 0x20,
        CANLINK = 4,
        CANMONIKER = 0x400000,
        CANMOVE = 2,
        CANRENAME = 0x10,
        CAPABILITYMASK = 0x177,
        COMPRESSED = 0x4000000,
        CONTENTSMASK = 0x80000000,
        DISPLAYATTRMASK = 0xfc000,
        DROPTARGET = 0x100,
        ENCRYPTED = 0x2000,
        FILESYSANCESTOR = 0x10000000,
        FILESYSTEM = 0x40000000,
        FOLDER = 0x20000000,
        GHOSTED = 0x8000,
        HASPROPSHEET = 0x40,
        HASSTORAGE = 0x400000,
        HASSUBFOLDER = 0x80000000,
        HIDDEN = 0x80000,
        ISSLOW = 0x4000,
        LINK = 0x10000,
        NEWCONTENT = 0x200000,
        NONENUMERATED = 0x100000,
        READONLY = 0x40000,
        REMOVABLE = 0x2000000,
        SHARE = 0x20000,
        STORAGE = 8,
        STORAGEANCESTOR = 0x800000,
        STORAGECAPMASK = 0x70c50008,
        STREAM = 0x400000,
        VALIDATE = 0x1000000
    }

    // Determines the type of items included in an enumeration. 
    // These values are used with the IShellFolder::EnumObjects method
    [Flags]
    public enum SHCONTF
    {
        FOLDERS = 0x0020,
        NONFOLDERS = 0x0040,
        INCLUDEHIDDEN = 0x0080,
        INIT_ON_FIRST_NEXT = 0x0100,
        NETPRINTERSRCH = 0x0200,
        SHAREABLE = 0x0400,
        STORAGE = 0x0800,
    }

    public enum SIGDN : int
    {
        SIGDN_NORMALDISPLAY = 0x0,
        SIGDN_PARENTRELATIVEPARSING = unchecked((int)0x80018001),
        SIGDN_DESKTOPABSOLUTEPARSING = unchecked((int)0x80028000),
        SIGDN_PARENTRELATIVEEDITING = unchecked((int)0x80031001),
        SIGDN_DESKTOPABSOLUTEEDITING = unchecked((int)0x8004C000),
        SIGDN_FILESYSPATH = unchecked((int)0x80058000),
        SIGDN_URL = unchecked((int)0x80068000),
        SIGDN_PARENTRELATIVEFORADDRESSBAR = unchecked((int)0x8007C001),
        SIGDN_PARENTRELATIVE = unchecked((int)0x80080001)
    }


    [Flags]
    public enum ShellExecuteMaskFlags : uint
    {
        SEE_MASK_DEFAULT = 0x00000000,
        SEE_MASK_CLASSNAME = 0x00000001,
        SEE_MASK_CLASSKEY = 0x00000003,
        SEE_MASK_IDLIST = 0x00000004,
        SEE_MASK_INVOKEIDLIST = 0x0000000c,   // Note SEE_MASK_INVOKEIDLIST(0xC) implies SEE_MASK_IDLIST(0x04)
        SEE_MASK_HOTKEY = 0x00000020,
        SEE_MASK_NOCLOSEPROCESS = 0x00000040,
        SEE_MASK_CONNECTNETDRV = 0x00000080,
        SEE_MASK_NOASYNC = 0x00000100,
        SEE_MASK_FLAG_DDEWAIT = SEE_MASK_NOASYNC,
        SEE_MASK_DOENVSUBST = 0x00000200,
        SEE_MASK_FLAG_NO_UI = 0x00000400,
        SEE_MASK_UNICODE = 0x00004000,
        SEE_MASK_NO_CONSOLE = 0x00008000,
        SEE_MASK_ASYNCOK = 0x00100000,
        SEE_MASK_HMONITOR = 0x00200000,
        SEE_MASK_NOZONECHECKS = 0x00800000,
        SEE_MASK_NOQUERYCLASSSTORE = 0x01000000,
        SEE_MASK_WAITFORINPUTIDLE = 0x02000000,
        SEE_MASK_FLAG_LOG_USAGE = 0x04000000,
    }

    public enum SHGFI : int
    {
        SHGFI_ICON = 0x000000100,
        SHGFI_SYSICONINDEX = 0x000004000
    }


    [ComImport, Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItem
    {
        [PreserveSig()]
        HResult BindToHandler(IntPtr pbc, ref Guid bhid, ref Guid riid, ref IntPtr ppv);
        HResult GetParent(ref IShellItem ppsi);
        HResult GetDisplayName(SIGDN sigdnName, ref StringBuilder ppszName);
        HResult GetAttributes(uint sfgaoMask, ref uint psfgaoAttribs);
        HResult Compare(IShellItem psi, uint hint, ref int piOrder);
    }

   
    public enum KnownFolder
    {
        Downloads
    }


    [Flags]
    public enum DropEffects : uint
    {
        DROPEFFECT_NONE = 0,
        DROPEFFECT_COPY	= 1,
        DROPEFFECT_MOVE	= 2,
        DROPEFFECT_LINK = 4,
        DROPEFFECT_SCROLL = 0x80000000
    }


    [Flags]
    public enum ShellFileOperations : uint
    {
        FO_MOVE = 0x0001,
        FO_COPY = 0x0002,
        FO_DELETE = 0x0003,
        FO_RENAME = 0x0004
    }


    public enum ShellFileOperationFlags : uint
    {
        FOF_MULTIDESTFILES         = 0x0001,
        FOF_CONFIRMMOUSE           = 0x0002,
        FOF_SILENT                 = 0x0004, // don't display progress UI (confirm prompts may be displayed still)
        FOF_RENAMEONCOLLISION      = 0x0008,  // automatically rename the source files to avoid the collisions
        FOF_NOCONFIRMATION         = 0x0010,  // don't display confirmation UI, assume "yes" for cases that can be bypassed, "no" for those that can not
        FOF_WANTMAPPINGHANDLE      = 0x0020,  // Fill in SHFILEOPSTRUCT.hNameMappings
        FOF_ALLOWUNDO              = 0x0040,  // enable undo including Recycle behavior for IFileOperation::Delete()
        FOF_FILESONLY              = 0x0080,  // only operate on the files (non folders), both files and folders are assumed without this
        FOF_SIMPLEPROGRESS         = 0x0100,  // means don't show names of files
        FOF_NOCONFIRMMKDIR         = 0x0200,  // don't dispplay confirmatino UI before making any needed directories, assume "Yes" in these cases
        FOF_NOERRORUI              = 0x0400,  // don't put up error UI, other UI may be displayed, progress, confirmations
        FOF_NOCOPYSECURITYATTRIBS  = 0x0800,  // dont copy file security attributes (ACLs)
        FOF_NORECURSION            = 0x1000,  // don't recurse into directories for operations that would recurse
        FOF_NO_CONNECTED_ELEMENTS  = 0x2000,  // don't operate on connected elements ("xxx_files" folders that go with .htm files)
        FOF_WANTNUKEWARNING        = 0x4000,  // during delete operation, warn if object is being permanently destroyed instead of recycling (partially overrides FOF_NOCONFIRMATION)
        FOF_NO_UI = (FOF_SILENT | FOF_NOCONFIRMATION | FOF_NOERRORUI | FOF_NOCONFIRMMKDIR)
    }


    public class ClipboardFormats
    {
        public const string CFSTR_SHELLIDLIST = "Shell IDList Array";
        public const string CFSTR_SHELLIDLISTOFFSET = "Shell Object Offsets";                // CF_OBJECTPOSITIONS
        public const string CFSTR_NETRESOURCES      = "Net Resource";                        // CF_NETRESOURCE
        public const string CFSTR_FILEDESCRIPTORA   = "FileGroupDescriptor";                 // CF_FILEGROUPDESCRIPTORA
        public const string CFSTR_FILEDESCRIPTORW   = "FileGroupDescriptorW";                // CF_FILEGROUPDESCRIPTORW
        public const string CFSTR_FILEDESCRIPTOR = CFSTR_FILEDESCRIPTORW;                // CF_FILEGROUPDESCRIPTORW
        public const string CFSTR_FILECONTENTS      = "FileContents";                        // CF_FILECONTENTS
        public const string CFSTR_FILENAMEA         = "FileName";                            // CF_FILENAMEA
        public const string CFSTR_FILENAMEW         = "FileNameW";                           // CF_FILENAMEW
        public const string CFSTR_FILENAME         = CFSTR_FILENAMEW;                           // CF_FILENAMEW
        public const string CFSTR_PRINTERGROUP      = "PrinterFriendlyName";                 // CF_PRINTERS
        public const string CFSTR_FILENAMEMAPA      = "FileNameMap";                         // CF_FILENAMEMAPA
        public const string CFSTR_FILENAMEMAPW      = "FileNameMapW";                        // CF_FILENAMEMAPW
        public const string CFSTR_FILENAMEMAP = CFSTR_FILENAMEMAPW;                        // CF_FILENAMEMAPW
        public const string CFSTR_SHELLURL          = "UniformResourceLocator";
        public const string CFSTR_INETURLA = CFSTR_SHELLURL;
        public const string CFSTR_INETURLW          = "UniformResourceLocatorW";
        public const string CFSTR_INETURL = CFSTR_INETURLW;
        public const string CFSTR_PREFERREDDROPEFFECT = "Preferred DropEffect";
        public const string CFSTR_PERFORMEDDROPEFFECT = "Performed DropEffect";
        public const string CFSTR_PASTESUCCEEDED    = "Paste Succeeded";
        public const string CFSTR_INDRAGLOOP        = "InShellDragLoop";
        public const string CFSTR_MOUNTEDVOLUME     = "MountedVolume";
        public const string CFSTR_PERSISTEDDATAOBJECT = "PersistedDataObject";
        public const string CFSTR_TARGETCLSID       = "TargetCLSID";                         // HGLOBAL with a CLSID of the drop target
        public const string CFSTR_LOGICALPERFORMEDDROPEFFECT = "Logical Performed DropEffect";
        public const string CFSTR_AUTOPLAY_SHELLIDLISTS = "Autoplay Enumerated IDList Array";    // (HGLOBAL with LPIDA;
        public const string CFSTR_UNTRUSTEDDRAGDROP = "UntrustedDragDrop";                   //  DWORD
        public const string CFSTR_FILE_ATTRIBUTES_ARRAY = "File Attributes Array";               // (FILE_ATTRIBUTES_ARRAY format on HGLOBAL;
        public const string CFSTR_INVOKECOMMAND_DROPPARAM= "InvokeCommand DropParam";             // (HGLOBAL with LPWSTR;
        public const string CFSTR_SHELLDROPHANDLER  = "DropHandlerCLSID";                    // (HGLOBAL with CLSID of drop handler;
        public const string CFSTR_DROPDESCRIPTION   = "DropDescription";                     // (HGLOBAL with DROPDESCRIPTION;
        public const string CFSTR_ZONEIDENTIFIER    = "ZoneIdentifier";   
    }

    public class Shell
    {

        public const string IShellItem2Guid = "7E9FB0D3-919F-4307-AB2E-9B1860310C93";


        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public int dwAttributes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = Shell.MAX_PATH)] public char[] szDisplayName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)] public char[] szTypeName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ICONINFO
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public uint wFunc;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pFrom;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pTo;
            public ushort fFlags;
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszProgressTitle;
        }


        public static Dictionary<KnownFolder, Guid> KnownFolders = new()
        {
            [KnownFolder.Downloads] = new ("374DE290-123F-4565-9164-39C4925E467B")
        };

        public const int MAX_PATH = 260;

        private static IntPtr _systemImageList= IntPtr.Zero;

        private static Dictionary<string, int> _folderIconIndexes = new Dictionary<string, int>();

        //map file extension to file icon
        private static Dictionary<string, WriteableBitmap> _fileIcons = new Dictionary<string, WriteableBitmap>();
        private static int _tmpFileIconIndex = -1;

        // Retrieves the IShellFolder interface for the desktop folder, which is the root of the Shell's namespace.
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetDesktopFolder(out IntPtr ppshf);

        // Takes a STRRET structure returned by IShellFolder::GetDisplayNameOf, converts it to a string, and places the result in a buffer. 
        [DllImport("shlwapi.dll", EntryPoint = "StrRetToBuf", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Int32 StrRetToBuf(IntPtr pstr, IntPtr pidl, StringBuilder pszBuf, int cchBuf);

        [DllImport("Shell32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern HResult SHCreateItemFromParsingName(string pszPath, IntPtr pbc, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IShellItem ppv);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool DestroyIcon([In] IntPtr hIcon);

        [DllImport("shell32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, int uFlags);

        [DllImport("shell32.dll", SetLastError = true, CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern string SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO pIconInfo);


        [DllImport("comctl32.dll", SetLastError = true, CharSet=CharSet.Unicode)]
        public static extern IntPtr ImageList_GetIcon(IntPtr hImgList, int i, uint flags);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true, ThrowOnUnmappableChar = true)]
        public static extern int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);

        [DllImport("shlwapi.dll", EntryPoint = "PathGetArgsW", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr PathGetArgsW([MarshalAs(UnmanagedType.LPTStr)] string path);



        [DllImport("kernel32.dll")]
        // just initialize it prior to call the function as
        // char[] lpBuffer = new char[nBufferLength];
        public static extern uint GetLogicalDriveStrings(uint nBufferLength, [Out] char[] lpBuffer);


        //the above PathGetArgsW returns pointer to string. Marshalling on pointer to string doesn't work here as per documentations
        //so we have the below function to do it in the correct way.
        public static string PathGetArgs(string path)
        {
            IntPtr p = PathGetArgsW(path);

            if (p != IntPtr.Zero)
            {
                return Marshal.PtrToStringUni(p);
            }

            return "";
        }

        public static void PathSplit(string path, out string appPath, out string arguments)
        {
            arguments = Shell.PathGetArgs(path);

            StringBuilder appPathBuilder = new StringBuilder(path);
            Shell.PathRemoveArgs(appPathBuilder);

            appPath = appPathBuilder.ToString();
        }

        public static string PathRemoveFileName(string path)
        {
            path = Path.TrimEndingDirectorySeparator(path);

            int idx = path.LastIndexOf(Path.DirectorySeparatorChar);
            if (idx != -1)
            {
                path = path.Substring(0, idx);
            }

            return path;
        }

        public static string PathAddQuotes(string path)
        {
            string retPath = "";

            path = path.Trim();

            if (path.StartsWith('\"'))
            {
                retPath = path;
            }
            else
            {
                retPath = string.Format("\"{0}\"", path);
            }

            return retPath;
        }

        public static string PathAddDirectorySeparator(string path)
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar))
            {
                path += Path.DirectorySeparatorChar;
            }

            return path;
        }


        [DllImport("shlwapi.dll", EntryPoint = "PathRemoveArgsW", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern void PathRemoveArgs([MarshalAs(UnmanagedType.LPTStr)] System.Text.StringBuilder lpszPath);

        private static int GetIconIndex(string path, out IntPtr hImgList)
        {
            int iconIndex = -1;

            Shell.SHFILEINFO psfi = new Shell.SHFILEINFO();

            hImgList = Shell.SHGetFileInfo(path, 0
                , ref psfi
                , Marshal.SizeOf(typeof(Shell.SHFILEINFO)), (int)SHGFI.SHGFI_SYSICONINDEX);

            if (hImgList != IntPtr.Zero)
            {
                iconIndex = psfi.iIcon;
            }

            return iconIndex;
        }

        private static void InitializeFolderIconIndexes()
        {
            string path = Path.GetTempPath();
            int iconIndex = -1;
            IntPtr hImgList = IntPtr.Zero;


            iconIndex = GetIconIndex(path, out hImgList);

            if (hImgList != IntPtr.Zero)
            {
                //all other folders.. this is the icon
                _folderIconIndexes["."] = iconIndex;
            }

            path = Environment.GetFolderPath(Environment.SpecialFolder.System);
            if (path.Contains(':'))
            {
                path = path.Substring(0, path.IndexOf(':') + 1);

                iconIndex = GetIconIndex(path, out hImgList);

                if (hImgList != IntPtr.Zero)
                {
                    //all other folders.. this is the icon
                    _folderIconIndexes[":"] = iconIndex;
                }

            }

            Environment.SpecialFolder[] folderTypes = new Environment.SpecialFolder[]
            {
                    Environment.SpecialFolder.Desktop,
                    Environment.SpecialFolder.MyDocuments,
                    Environment.SpecialFolder.MyMusic,
                    Environment.SpecialFolder.MyPictures,
                    Environment.SpecialFolder.MyVideos,
            };


            //above special folders
            foreach (var folderType in folderTypes)
            {

                path = Environment.GetFolderPath(folderType);

                iconIndex = GetIconIndex(path, out hImgList);

                if (hImgList != IntPtr.Zero)
                {
                    string trimmedPath = path.TrimEnd('\\').ToLower();

                    //all other folders.. this is the icon
                    _folderIconIndexes[trimmedPath] = iconIndex;
                }
            }


            //downloads folder
            path = SHGetKnownFolderPath(KnownFolders[KnownFolder.Downloads], 0, IntPtr.Zero);

            iconIndex = GetIconIndex(path, out hImgList);

            if (hImgList != IntPtr.Zero)
            {

                string trimmedPath = path.TrimEnd('\\').ToLower();

                //all other folders.. this is the icon
                _folderIconIndexes[trimmedPath] = iconIndex;
            }

            //we have to set this at the earlier when initializing folders
            if (_systemImageList == IntPtr.Zero)
            {
                _systemImageList = hImgList;
            }

        }

        public static int GetFolderIconIndex(string folderPath)
        {
           
            //if  nothing is added, then initialize
            if (_folderIconIndexes.Count == 0)
            {
                InitializeFolderIconIndexes();
            }


            int iconIndex = -1;

            string trimmedPath = folderPath.TrimEnd('\\').ToLower();

            if (_folderIconIndexes.ContainsKey(trimmedPath))
            {
                iconIndex = _folderIconIndexes[trimmedPath];
            }
            else
            {
                //check if drive letter or regular folder
                if (folderPath.Length <= 3)
                {
                    if (_folderIconIndexes.ContainsKey(":"))
                    {
                        iconIndex = _folderIconIndexes[":"];
                    }
                }
                else
                {
                    if (_folderIconIndexes.ContainsKey("."))
                    {
                        iconIndex = _folderIconIndexes["."];
                    }
                }
            }
            

            return iconIndex;
        }


        //this ignores remote and CDROM paths
        public static bool IsPathLocal(string path)
        {

            if (String.IsNullOrEmpty(path))
            {
                return false;
            }

            if (new Uri(path).IsUnc)
            {
                return false;
            }

            DriveInfo di = new DriveInfo(path);
            if ( (di.DriveType == DriveType.Fixed)
                || (di.DriveType == DriveType.Removable)
                || (di.DriveType == DriveType.Ram) )
            {
                return true;
            }

            return false;
        }

        public static bool IsPathRemote(string path)
        {

            if (String.IsNullOrEmpty(path))
            {
                return false;
            }
            if (new Uri(path).IsUnc)
            {
                return true;
            }
            if (new DriveInfo(path).DriveType == DriveType.Network)
            {
                return true;
            }

            return false;
        }

        public static WriteableBitmap GetFolderIcon(string path)
        {
            int iconIndex = -1;

            //IsPathLocal and other directory functions works correctly when there is a backslash at the end of a drive letter
            if (!string.IsNullOrEmpty(path))
            {
                if (!path.EndsWith('\\')) path = path + "\\";
            }
            
            if (IsPathLocal(path) && Directory.Exists(path))
            {
                iconIndex = GetFolderIconIndex(path);
            }
            else
            {
                if (_folderIconIndexes.ContainsKey("."))
                {
                    iconIndex = _folderIconIndexes["."];
                }
            }

            if (iconIndex >= 0)
            {
                return ExtractIcon(iconIndex, _systemImageList);
            }

            return null;
        }


        public static WriteableBitmap GetFileIcon(string path)
        {

            if (_tmpFileIconIndex == -1)
            {
                IntPtr hImgList = IntPtr.Zero;

                string tmpPath = Path.GetTempFileName();
                _tmpFileIconIndex = GetIconIndex(tmpPath, out hImgList);

                //delete the temp file
                File.Delete(tmpPath);

                //we will come here first when listing files, so we have to set it the image list first
                if (_systemImageList == IntPtr.Zero)
                {
                    _systemImageList = hImgList;
                }

            }

            WriteableBitmap bmp = null;
            string ext = Path.GetExtension(path);
            string loweredPath = path.ToLower();


            if (!string.IsNullOrEmpty(ext))
            {
                ext = ext.ToLower();

                bool contained = (ext == ".exe");

                if (!contained && _fileIcons.ContainsKey(ext))
                {
                    bmp = _fileIcons[ext];
                }
                else if (contained && _fileIcons.ContainsKey(loweredPath) )
                {
                    bmp = _fileIcons[loweredPath];
                }
                else
                {
                    if (IsPathLocal(path) && File.Exists(path))
                    {
                        bmp = ExtractIcon(path);

                        if (bmp != null)
                        {
                            if (contained)
                            {
                                _fileIcons[loweredPath] = bmp;
                            }
                            else
                            {
                                _fileIcons[ext] = bmp;
                            }
                        }
                    }
                }
                
            }
            
            //if the file doesn't exist and if we don't have it in extension map then use temp file icon index
            if ((bmp == null) && (_tmpFileIconIndex >= 0))
            {
                bmp = ExtractIcon(_tmpFileIconIndex, _systemImageList);
            }

            return bmp;
        }


        public static WriteableBitmap ExtractIcon(string path)
        {
            WriteableBitmap bmp = null;

            Shell.SHFILEINFO psfi = new Shell.SHFILEINFO();
            Shell.ICONINFO pIconInfo = new Shell.ICONINFO();

            IntPtr hImgList = Shell.SHGetFileInfo(path, 0
                , ref psfi
                , System.Runtime.InteropServices.Marshal.SizeOf(typeof(Shell.SHFILEINFO)), (int)SHGFI.SHGFI_SYSICONINDEX);


            if (hImgList != IntPtr.Zero)
            {
                bmp = ExtractIcon(psfi.iIcon, hImgList);
            }

            return bmp;
        }


        private static WriteableBitmap ExtractIcon(int iconIndex, IntPtr hImgList)
        {
            
            if (hImgList == IntPtr.Zero) return null;

            WriteableBitmap bmp = null;
            Shell.ICONINFO pIconInfo = new Shell.ICONINFO();

            IntPtr hIcon = ImageList_GetIcon(hImgList, iconIndex, 0 /*ILD_NORMAL*/);

            if (hIcon != IntPtr.Zero)
            {
                if (true == Shell.GetIconInfo(hIcon, out pIconInfo))
                {
                    bmp = Gdi.GetCaptureWriteableBitmap(pIconInfo.hbmColor);
                }

                DestroyIcon(hIcon);
            }
            
            return bmp;
        }
    }
}
