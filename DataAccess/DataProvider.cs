using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Logging;
using Ninject;
using System.Reflection;

namespace DataAccess
{
    public class DataProvider : IDataProvider
    {
        ILog log { get { return log ?? (log = GetLogger()); } set { log = value; } }

        public ILog GetLogger()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel.Get<ILog>();
        }

        public List<Store> GetStoresList()
        {
            throw new NotImplementedException();
        }

        public List<Item> GetBaseQuery(List<Guid> lg)
        {
            throw new NotImplementedException();
        }

        public List<string> GetStoreCells()
        {
            throw new NotImplementedException();
        }

        public bool UpdateStoreCell(Guid guid, string cell)
        {
            throw new NotImplementedException();
        }
    }
}
