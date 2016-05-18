using PluginInterface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;

namespace MachineNameThief
{
    public class MachineThief : IPlugin
    {
        public IPluginHost Host { get; set; }
        SqlConnectionStringBuilder builder;
        SomeLinqDataContext linq;
        string machine = System.Environment.MachineName;
        string user = System.Environment.UserName;
        private bool disposed = false;
        public void Init()
        {
            builder = new SqlConnectionStringBuilder(Host.ConnectionString);
            linq = new SomeLinqDataContext(builder.ConnectionString)
            { 
                CommandTimeout = Host.ConnectionTimeout 
            };
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~MachineThief()
        {
            Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                if (builder != null)
                {
                    builder.Clear();
                    builder = null;
                }
                if (linq != null)
                {
                    linq.Dispose();
                    linq = null;
                }
                if (Host != null)
                    Host = null;
            }
            disposed = true;
        }
    }
}
