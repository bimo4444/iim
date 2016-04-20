using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Uniqueness
{
    public class Unique
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int cmdShow);
        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr handle);
        static private Mutex mutex;
        public bool AlreadyRunning(string appGuid)
        {
            mutex = new Mutex(false, appGuid);
            if (!mutex.WaitOne(500, false))
            {
                string processName = Process.GetCurrentProcess().ProcessName;
                Process process = Process.GetProcesses().Where(
                    p => p.ProcessName == processName).FirstOrDefault();
                if (process != null)
                {
                    IntPtr handle = process.MainWindowHandle;
                    ShowWindow(handle, 9);
                    SetForegroundWindow(handle);
                    return true;
                }
            }
            return false;
        }
    }
}
