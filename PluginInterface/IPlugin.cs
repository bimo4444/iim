using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginInterface
{
    public interface IPlugin
    {
        IPluginHost Host { set; }
        void Init();
        void Dispose();
    }
    public interface IPluginHost
    {
        string ConnectionString { get; }
        int ConnectionTimeout { get; }
    }
}
