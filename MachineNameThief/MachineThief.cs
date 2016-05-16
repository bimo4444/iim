using PluginInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MachineNameThief
{
    public class MachineThief : IPlugin
    {
        string machine = System.Environment.MachineName;
        string user = System.Environment.UserName;

        public void Init()
        {
            SomeLinqDataContext linq = new SomeLinqDataContext();
            var v = linq.SecuritySystemUsers
                .Where(w => w.UserName == user && w.GCRecord == null)
                .Select(s => s)
                .FirstOrDefault();
            if (v == null || v.Компьютер == machine)
            {
                linq.Dispose();
                return;
            }
            v.Компьютер = machine;
            linq.SubmitChanges();
            linq.Dispose();
        }
        public void Dispose()
        {
            
        }
    }
}
