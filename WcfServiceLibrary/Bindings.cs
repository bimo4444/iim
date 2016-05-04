using DataAccess;
using Logging;
using Ninject.Modules;
using Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trap;

namespace WcfServiceLibrary
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IXmlSerializer>().To<XmlSerializer>();
            Bind<IDataProvider>().To<DataProvider>();
            Bind<IExceptionTrap>().To<ExceptionTrap>();
            Bind<ILog>().To<Log>();
        }
    }
}
