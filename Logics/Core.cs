using Entity;
using Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logics
{
    public class Core : ICore
    {
        XmlSerializer xmlSerializer = new XmlSerializer();

        readonly string logFileName = "log.txt";
        readonly string logFilePath;

        readonly string configFilename = "iim.config.xml";

        readonly string userConfigFilePath = "users";
        readonly string userConfigFileName = "users" + Environment.UserName + ".xml";
        readonly string path;

        readonly string assemblyPath = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);

        public Core()
        {
            logFilePath = assemblyPath + "\\logs";
        }

        public void SaveProperties(SomeUser someUser)
        {
            //xmlSerializer.Serialize(someUser);
        }
    }
}
