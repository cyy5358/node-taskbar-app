using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Win32Interop.WinHandles.Internal;
using System.Runtime.InteropServices;
using System.IO;


namespace Win32Interop.WinHandles
{
    /// <summary> Extension methods for <see cref="WindowHandle"/> </summary>
    public static class WindowHandleExtensions
    {
        /// <summary> Check if the given window handle is currently visible. </summary>
        /// <param name="windowHandle"> The window to act on. </param>
        /// <returns> true if the window is visible, false if not. </returns>
        public static bool IsVisible(this WindowHandle windowHandle)
        {

            return (NativeMethods.GetWindowLongA(windowHandle.RawPtr, -16) & 0x10000000L) > 0;
            //return NativeMethods.IsWindowVisible(windowHandle.RawPtr);
        }

        /// <summary> Gets the Win32 class name of the given window. </summary>
        /// <param name="windowHandle"> The window handle to act on. </param>
        /// <returns> The class name of the passed in window. </returns>
        public static string GetClassName(this WindowHandle windowHandle)
        {
            int size = 255;
            int actualSize = 0;
            StringBuilder builder;
            do
            {
                builder = new StringBuilder(size);
                actualSize = NativeMethods.GetClassName(windowHandle.RawPtr, builder, builder.Capacity);
                size *= 2;
            } while (actualSize == size - 1);

            return builder.ToString();
        }

        /// <summary> Gets the text associated with the given window handle. </summary>
        /// <param name="windowHandle"> The window handle to act on. </param>
        /// <returns> The window text. </returns>
        public static string GetWindowTitle(this WindowHandle windowHandle)
        {
            int size = NativeMethods.GetWindowTextLength(windowHandle.RawPtr);
            if (size > 0)
            {
                var builder = new StringBuilder(size + 1);
                NativeMethods.GetWindowText(windowHandle.RawPtr, builder, builder.Capacity);
                return builder.ToString();
            }

            return String.Empty;
        }

        public static Rect GetWindowRect(this WindowHandle windowHandle)
        {
            NativeMethods.GetWindowRect(windowHandle.RawPtr, out Rect rect1);
            return rect1;
        }

        public static uint GetWindowThreadProcessId(this WindowHandle windowHandle)
        {
            NativeMethods.GetWindowThreadProcessId(windowHandle.RawPtr, out uint ID);
            return ID;
        }

        public static string ProcessExist(this int hProcess)
        {
            bool i = NativeMethods.GetExitCodeProcess((IntPtr)hProcess, out uint ret);
            if (i)
            {
                return String.Format("True + {0}", ret);
            }
            else
            {
                return String.Format("False + {0}", ret);
            }
        }

        public static string GetProcessName(this WindowHandle hwnd)
        {
            IntPtr hProcess = NativeMethods.GetProcessHandleFromHwnd(hwnd.RawPtr);
            if (hProcess == IntPtr.Zero)
            {
                return WinErrors.GetSystemMessage();
                //return "";
            }
            StringBuilder stringBuilder = new StringBuilder(10240);
            var result = NativeMethods.GetModuleFileNameExW((IntPtr)hProcess, (IntPtr)null, stringBuilder, 10240);
            if (result == 0)
            {
                return "";
            }
            return Path.GetFileNameWithoutExtension(stringBuilder.ToString());

        }

        public static string ProcessName(this int processId)
        {
            var hProcess = NativeMethods.OpenProcess(0x0010 | 0x0400 | 0xFFFF, false, processId);
            if (hProcess == 0)
            {
                return "";
            }
            StringBuilder stringBuilder = new StringBuilder(1024);
            var result = NativeMethods.GetModuleFileNameExW((IntPtr)hProcess, (IntPtr)null, stringBuilder, 1024);
            if (result == 0)
            {
                return "";
            }
            return Path.GetFileNameWithoutExtension(stringBuilder.ToString());
        }

        //public static extern IntPtr GetProcessHandleFromHwnd(IntPtr hwnd); 
    }
}