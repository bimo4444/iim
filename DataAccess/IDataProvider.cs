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
        void Configure(string connectionString);
        void Configure(int connectionTimeOut);

        bool NewCell(string name);
        List<Store> GetStoresList();
        List<string> GetStoreCells();
        bool UpdateStoreCell(Guid guid, string cell);
        List<Item> GetBaseQuery(IEnumerable<Guid> guids);

    }
}
