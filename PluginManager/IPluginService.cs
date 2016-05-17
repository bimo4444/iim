using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginManager
{
    public interface IPluginService : IDisposable
    {
        string ConnectionString { set; }
        int ConnectionTimeout { set; }
        void Init(string path);
    }
}
