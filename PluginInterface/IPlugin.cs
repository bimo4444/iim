using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginInterface
{
    public interface IPlugin : IDisposable
    {
        IPluginHost Host { set; }
        void Init();
    }
    public interface IPluginHost
    {
        string ConnectionString { get; }
        int ConnectionTimeout { get; }
    }
}
