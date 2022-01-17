using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Win32Interop.WinHandles.Internal;

namespace Win32Interop.WinHandles
{
    internal class Program
    {
        static void print_visible_wnd()
        {

            var allVisibleWindows = TopLevelWindowUtils.FindWindows(wh => wh.IsVisible());
            HashSet<uint> vs = new HashSet<uint>()
            {
                0
            };
            HashSet<string> excludedExePath = new HashSet<string>()
            {
                "TextInputHost",
                "MiScreenShareGuide"
            };
            LinkedList<BriefProcess> process_list = new LinkedList<BriefProcess>();

            // make unique
            foreach (var win in allVisibleWindows)
            {
                var p = new BriefProcess(win);
                if (p.MainWindowTitle == null || p.MainWindowTitle == String.Empty)
                {
                    continue;
                }
                if (vs.Add(p.ProcessId) && !excludedExePath.Contains(p.ProcessName))
                {
                    process_list.AddLast(p);
                    p.save_image();
                }
            }
            Console.Write(JsonConvert.SerializeObject(process_list));
        }
        static void make_wnd_visible(int process_id)
        {
            var mhdl = Process.GetProcessById(process_id).MainWindowHandle;
            if (mhdl == IntPtr.Zero)
            {
                Console.Error.WriteLine("Process mainWindowHandle not fonud");
            }
            else
            {
                NativeMethods.SwitchToThisWindow(mhdl, true);
            }


        }
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == string.Empty)
            {
                string target_dir = Path.Combine(Directory.GetCurrentDirectory(), "__img__");
                if (!Directory.Exists(target_dir)) Directory.CreateDirectory(target_dir);
                print_visible_wnd();
            }
            else
            {
                make_wnd_visible(int.Parse(args[0]));
            }
            return;
        }
    }
}