using Logging;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ILog>().To<Log>();
        }
    }
}
