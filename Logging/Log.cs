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
        private static readonly string logFilename = "log.txt";
        private static readonly string dirrectoryPath = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\logs";
        private readonly string path = Path.Combine(dirrectoryPath, logFilename);

        public Log()
        {
            if (!Directory.Exists(dirrectoryPath))
                Directory.CreateDirectory(dirrectoryPath);
        }

        private Object key = new Object();
        public void Write(string s)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(path, true))
                {
                    lock (key)
                    {
                        file.WriteLine(s);
                        file.Close();
                    }
                }
            }
            catch
            {
                //
            }
        }
        public void Write(Exception ex)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(path, true))
                {
                    lock (key)
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
            catch
            {
                //
            }
        }
    }
}
