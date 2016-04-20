using Entity;
using Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logics
{
    public class Core : ICore
    {
        XmlSerializer xmlSerializer = new XmlSerializer();

        private readonly string logFilename = "log.txt";
        private readonly string dirrectoryPath = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\logs";
        private string path = Path.Combine(dirrectoryPath, logFilename);

        public Core()
        {

        }

        public void SaveProperties(SomeUser someUser)
        {
            xmlSerializer.Serialize(someUser);
        }
    }
}
