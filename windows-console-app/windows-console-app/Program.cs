using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace windows_console_app
{
    public class ProcessInfo
    {
        public int ProcessId { get; set; }

        public string MainWindowTitle { get; set; }

        public string ProcessName { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 0 && args[0] != "")
                {
                    FocusHandler.SwitchToWindow(Int32.Parse(args[0]));
                }
                else
                {
                    Console.WriteLine(GetForegroundProcessInfo());
                }
                

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static object GetForegroundProcessInfo()
        {
            ConcurrentQueue<string> json = new ConcurrentQueue<string>();
            var processes = Process.GetProcesses();
            var task = new Task[processes.Length];
            for(int i=0; i<processes.Length; ++i)
            {
                var p = processes[i];
                task[i] = new Task(() => {
                    var pinfo = new ProcessInfo
                    {
                        MainWindowTitle = p.MainWindowTitle,
                        ProcessName = p.ProcessName,
                        ProcessId = p.Id
                    };
                    if (pinfo.MainWindowTitle == null || pinfo.MainWindowTitle.Length == 0)
                    {
                        return;
                    }
                    else
                    {
                        json.Enqueue(JsonConvert.SerializeObject(pinfo));
                    }
                });
                task[i].Start();
            }
            foreach(var t in task)
            {
                t.Wait();
            }
            return
                "[" + string.Join(",",json) +"]";
        }
    }
}
