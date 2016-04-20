using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logics
{
    public class Config
    {
        public string ConnectionString { get; set; }
        public Config()
        {
            ConnectionString = "Data Source=SRVGALDB2;Initial Catalog=GalAMM_test;Integrated Security=True";
        }
    }
}
