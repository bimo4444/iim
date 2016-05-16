using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginInterface
{
    public interface IPlugin
    {
        void Init();
        void Dispose();
    }
}
