using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public class Log : ILog
    {
        private static object key = new Object();
        private readonly string logFilename;
        private readonly string dirrectoryPath = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
        private readonly string path;

        public Log()
            : this(null, null) { }
        public Log(string dirrectoryName, string fileName)
        {
            dirrectoryPath += "\\" + (dirrectoryName ?? "logs");
            logFilename = fileName != null ? fileName + ".txt" : "log.txt";
            path = dirrectoryPath + "\\" + logFilename;
            if (!Directory.Exists(dirrectoryPath))
                Directory.CreateDirectory(dirrectoryPath);
        }

        public void Write(string s)
        {
            try
            {
                lock (key)
                {
                    using (StreamWriter file = new StreamWriter(path, true))
                    {
                        file.WriteLine("****************************************************************" +
                            "*****************************************************************************");
                        file.WriteLine(String.Format("{0} {1} {2}",
                            DateTime.Now.ToString(),
                            System.Environment.MachineName,
                            System.Environment.UserName));
                        file.WriteLine(s);
                        file.Close();
                    }
                }
            }
            catch { }
        }
        public void Write(Exception ex)
        {
            try
            {
                lock (key)
                {
                    using (StreamWriter file = new StreamWriter(path, true))
                    {
                        file.WriteLine("****************************************************************" +
                            "*****************************************************************************");
                        file.WriteLine(String.Format("{0} {1} {2}",
                            DateTime.Now.ToString(),
                            System.Environment.MachineName,
                            System.Environment.UserName));
                        file.WriteLine(ex.Message);
                        file.WriteLine(ex.Source);
                        file.WriteLine(ex.StackTrace);
                        file.Close();
                    }
                }
            }
            catch { }
        }
    }
}
