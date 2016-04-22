using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Entity
{
    public class UserConfig
    {
        public UserConfig()
        {
            StoresList = new List<Store>();
        }
        public List<Store> StoresList { get; set; }
        
        public bool PartyGrouping { get; set; }
        public bool OrderRPGrouping { get; set; }
        public bool Zeros { get; set; }
        public bool TaskGrouping { get; set; }
        public bool Minus { get; set; }
        public bool StatGrouping { get; set; }
    }
}
