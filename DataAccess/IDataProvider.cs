using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IDataProvider
    {
        List<Store> GetStoresList();
        List<Item> GetBaseQuery(List<Guid> lg);
        List<string> GetStoreCells();
        bool UpdateStoreCell(Guid guid, string cell);
    }
}
