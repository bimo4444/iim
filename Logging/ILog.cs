using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public interface ILog
    {
        void Write(string s);
        void Write(Exception x);
    }
}
