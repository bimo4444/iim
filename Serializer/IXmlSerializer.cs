using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serializer
{
    public interface IXmlSerializer
    {
        T Deserialize<T>(string s);
        bool Serialize<T>(T t, string p, string f);
    }
}
