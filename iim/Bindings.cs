using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Logics;
using DataAccess;
using Metamorphosis;
using ExcelServices;
using Serializer;
using Logging;
using Ninject.Parameters;

namespace iim
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ILog>().To<Log>();
            this.Bind<ICore>().To<Core>();
            this.Bind<IPresenter>().To<Presenter>();
            this.Bind<IDataProvider>().To<DataProvider>();
            this.Bind<IMetamorphoses>().To<Metamorphoses>();
            this.Bind<IXmlSerializer>().To<XmlSerializer>();
            this.Bind<IExcelService>().To<ExcelService>();
        }
    }
}
