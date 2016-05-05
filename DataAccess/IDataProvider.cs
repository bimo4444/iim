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
        void Init();
        bool NewCell(string name);
        List<Store> GetStoresList();
        IEnumerable<string> GetStoreCells();
        bool UpdateStoreCell(Guid guid, string cell);
        IEnumerable<Item> GetBaseQuery(IEnumerable<Guid> guids);

    }
}
