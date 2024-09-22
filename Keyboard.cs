using Microsoft.UI.Input;
using System;
using System.Runtime.InteropServices;
using System.Text;
using Windows.System;

namespace Projzo.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AcceleratorEntry
    {
        public byte IsVirtual;
        public ushort Key;
        public ushort Command;

        [DllImport("user32.dll", CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        public static extern IntPtr CreateAcceleratorTable([In, MarshalAs(UnmanagedType.LPArray)] AcceleratorEntry[] paccel, int cAccel);
    }

    public class Keyboard
    {
        static public bool IsKeyDown(Windows.System.VirtualKey key)
        {
            return InputKeyboardSource.GetKeyStateForCurrentThread(key).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down);
        }

        static public bool IsKeyUp(Windows.System.VirtualKey key)
        {
            return InputKeyboardSource.GetKeyStateForCurrentThread(key).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.None);
        }


        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState, 
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)]
            StringBuilder receivingBuffer, int bufferSize, uint flags);

        
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetKeyboardState(byte[] lpKeyState);

        public static string ToUnicode(VirtualKey key)
        {
            StringBuilder sb = new StringBuilder(256);
            byte[] keyboardState = new byte[256];

            bool ret = GetKeyboardState(keyboardState);

            ToUnicode((uint)key, 0, keyboardState, sb, 256, 0);

            return sb.ToString();
        }
    }
}
