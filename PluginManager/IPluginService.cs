using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginManager
{
    public interface IPluginService : IDisposable
    {
        void Init(string path);
    }
}
