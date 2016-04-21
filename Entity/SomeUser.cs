using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class UserConfig
    {
        public UserConfig()
        {
            ls = new List<Store>();
        }

        public List<Store> ls { get; set; }
        public bool PartyGrouping { get; set; }
        public bool OrderRPGrouping { get; set; }
        public bool Zeros { get; set; }
        public bool TaskGrouping { get; set; }
        public bool Minus { get; set; }
        public bool StatGrouping { get; set; }
    }
}
