using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logics
{
    public class Config
    {
        public int ConnectionTimeOut { get; set; }
        public string ConnectionString { get; set; }
        public bool UsingWcfService { get; set; }
        public string WcfServiceAddress { get; set; }

        public Config()
        {
            ConnectionTimeOut = 300;
            ConnectionString = 
                "Data Source=SRVGALDB2;Initial Catalog=GalAMM_test;Integrated Security=True";
            UsingWcfService = true;
            WcfServiceAddress = "http://192.168.0.13:666/test";
        }
    }
}
