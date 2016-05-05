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
using Trap;
using StoreCellsNormalizer;
using WcfClientLibrary;

namespace iim
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IPresenter>().To<Presenter>();
            this.Bind<ICore>().To<Core>();
            this.Bind<IXmlSerializer>().To<XmlSerializer>();
            this.Bind<IMetamorphoses>().To<Metamorphoses>();
            this.Bind<IExcelService>().To<ExcelService>();
            this.Bind<ICellsNormalizer>().To<CellsNormalizer>();
            this.Bind<IDataProvider>().To<DataProvider>();
            this.Bind<IWcfClient>().To<WcfClient>();
            this.Bind<IExceptionTrap>().To<ExceptionTrap>();
            this.Bind<ILog>().To<Log>().InSingletonScope();
        }
    }
}
