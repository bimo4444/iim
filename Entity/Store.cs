using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Store
    {
        public Guid OidStore { get; set; }
        public Guid Higher { get; set; }
        public string StoreString { get; set; }
        public bool IsSelected { get; set; }
    }
}
