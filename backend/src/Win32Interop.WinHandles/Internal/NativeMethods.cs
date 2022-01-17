using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Win32Interop.WinHandles.Internal
{
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
        override public string ToString()
        {
            return String.Format("[{0} {1} {2} {3}]",Left,Top,Right,Bottom);
        }
    }

    internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    /// <summary> Win32 methods. </summary>
    internal static class NativeMethods
    {
        public const bool EnumWindows_ContinueEnumerating = true;
        public const bool EnumWindows_StopEnumerating = false;

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr FindWindow(string sClassName, string sAppName);

        [DllImport("user32.dll")]
        internal static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern long GetWindowLongA(IntPtr hWnd,int index);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern int GetClassName(IntPtr hWnd,
                                                StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll")]
        internal static extern int GetWindowRect(IntPtr hwnd, out Rect lpRect);


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetExitCodeProcess(IntPtr hProcess, out uint ExitCode);

        [DllImport("Psapi.dll", CharSet = CharSet.Unicode)]
        public static extern uint GetModuleFileNameExW(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpszFileName, int nSize);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern int OpenProcess(uint  dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("Oleacc.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetProcessHandleFromHwnd(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

    }
}