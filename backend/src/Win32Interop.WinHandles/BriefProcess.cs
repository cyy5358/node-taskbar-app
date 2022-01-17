using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Win32Interop.WinHandles
{
    //[JsonObject(MemberSerialization.OptIn)]
    class BriefProcess
    {
        public string MainWindowTitle;
        public string ProcessName;
        public uint ProcessId; // process id

        [JsonIgnore]
        public string icon;
        [JsonIgnore]
        private WindowHandle hWnd;

        public BriefProcess(WindowHandle hWnd)
        {
            this.MainWindowTitle = hWnd.GetWindowTitle();
            this.ProcessName = hWnd.GetProcessName();
            this.ProcessId = hWnd.GetWindowThreadProcessId();
            this.icon = string.Format("{0}/__img__/{1}.jpg",Directory.GetCurrentDirectory(),this.ProcessId);
            this.hWnd = hWnd; 
        }

        public void save_image()
        {
            if (!Directory.Exists(Path.GetDirectoryName(this.icon)))
            {
                Console.Error.WriteLine("File save failed {0}, parent dir not exist",Path.GetFullPath(this.icon));
                return;
            }
            try
            {
                IconGetter.GetSmallWindowIcon(this.hWnd.RawPtr).Save(this.icon);
            }catch (Exception ex)
            {
                Console.Error.WriteLine("File save failed {0}, {1}",Path.GetFullPath(this.icon),ex.Message);  
            }
        }
    }
}
