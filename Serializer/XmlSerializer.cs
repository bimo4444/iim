using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serializer
{
    public class XmlSerializer : IXmlSerializer
    {
        public T Deserialize<T>(string f)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer reader =
                    new System.Xml.Serialization.XmlSerializer(typeof(T));
                System.IO.StreamReader file = new System.IO.StreamReader(f);
                T u = (T)reader.Deserialize(file);
                file.Close();
                return u;
            }
            catch
            {
                return Activator.CreateInstance<T>();
            }
        }
        public bool Serialize<T>(T t, string p, string f)
        {
            try
            {
                if (!Directory.Exists(p))
                    Directory.CreateDirectory(p);

                var writer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                var wfile = new System.IO.StreamWriter(f);
                writer.Serialize(wfile, t);
                wfile.Close();
                return true;
            }
            catch(Exception x)
            {
                return false;
            }

        }
    }
}
