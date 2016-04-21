using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data.SqlClient;
using System.Data;
using Trap;
using ADOLib;

namespace DataAccess
{
    public class DataProvider : IDataProvider
    {
        int connectionTimeOut = 30;
        string connectionString;

        ADO ado;
        SqlConnectionStringBuilder builder;
        IExceptionTrap exceptionTrap;
        public DataProvider(IExceptionTrap exceptionTrap)
        {
            this.exceptionTrap = exceptionTrap;
        }

        string userAndMachineName = 
            System.Environment.UserName + " | " + System.Environment.MachineName;

        public void Initialize()
        {
            ado = new ADO(connectionString, connectionTimeOut);
            builder = new SqlConnectionStringBuilder(connectionString);
        }
        public void Configure(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Configure(int connectionTimeOut)
        {
            this.connectionTimeOut = connectionTimeOut;
        }
        public List<Store> GetStoresList()
        {
            return exceptionTrap.Catch(delegate()
            {
                using (SomeLinqDataContext lis = new SomeLinqDataContext(builder.ConnectionString))
                {
                    lis.CommandTimeout = connectionTimeOut;
                    return lis
                        .mpzGetMxList()
                        .Select(
                            s => new Store
                            {
                                OidStore = s.OidStore,
                                StoreString = s.StoreString,
                                Higher = s.Higher ?? Guid.Empty
                            })
                        .ToList();
                }
            });
        }

        public List<Item> GetBaseQuery(IEnumerable<Guid> guids)
        {
            return exceptionTrap.Catch(delegate()
            {
                DataTable dataTable =
                ado.StoredProcWithGuidsTableParameter("mpzGetTmc", guids);
                List<Item> items = new List<Item>();
                foreach (DataRow r in dataTable.Rows)
                {
                    items.Add(new Item
                    {
                        //get off string.empty?
                        OidStore = (Guid)r["OidStore"],
                        OidUnit = (Guid)r["OidNP"],
                        Remains = (decimal)r["Wait"],
                        Quantity = (decimal)r["Quantity"],
                        Group = r["Groupp"] != DBNull.Value ? (string)r["Groupp"] : String.Empty,
                        UnitMeasurement = r["EI"] != DBNull.Value ? (string)r["EI"] : String.Empty,
                        OrderRP = r["OrderRP"] != DBNull.Value ? (string)r["OrderRP"] : String.Empty,
                        KeyArticle = r["KeyArt"] != DBNull.Value ? (string)r["KeyArt"] : String.Empty,
                        ComtecNumber = r["ComNum"] != DBNull.Value ? (string)r["ComNum"] : String.Empty,
                        Refill = r["Refill"] != DBNull.Value ? (string)r["Refill"] : String.Empty,
                        StoreKey = r["StoreKey"] != DBNull.Value ? (string)r["StoreKey"] : String.Empty,
                        StoreCell = r["Cell"] != DBNull.Value ? (string)r["Cell"] : String.Empty,
                        Party = r["Party"] != DBNull.Value ? (string)r["Party"] : String.Empty,
                        UnitName = r["NP"] != DBNull.Value ? (string)r["NP"] : String.Empty,
                        User = r["UserName"] != DBNull.Value ? (string)r["UserName"] : String.Empty,
                        Movement = r["Movement"] != DBNull.Value ? (string)r["Movement"] : String.Empty,
                        DocumentType = r["DocumentType"] != DBNull.Value ? (string)r["DocumentType"] : String.Empty,
                        DocumentNumber = r["DocumentNumber"] != DBNull.Value ? (string)r["DocumentNumber"] : String.Empty,
                        NextOperation = r["NextOperation"] != DBNull.Value ? (string)r["NextOperation"] : String.Empty,
                        DocumentName = r["DocumentName"] != DBNull.Value ? (string)r["DocumentName"] : String.Empty,
                        Ready = r["Ready"] != DBNull.Value ? (string)r["Ready"] : String.Empty,
                        Transition = (int?)r["Transition"],
                        Task = r["Task"] != DBNull.Value ? (string)r["Task"] : String.Empty,
                        Stat = r["Stat"] != DBNull.Value ? (string)r["Stat"] : String.Empty,
                        Date = (DateTime)r["Dates"]
                    });
                }
                return items;
            });
        }

        public List<string> GetStoreCells()
        {
            return exceptionTrap.Catch(delegate()
            {
                using (SomeLinqDataContext sl = new SomeLinqDataContext(builder.ConnectionString))
                {
                    sl.CommandTimeout = connectionTimeOut;
                    List<string> listCells = sl.СкладскаяЯчейкаs
                        .Where(
                            w => w.GCRecord == null)
                        .Select(
                            s => s.Наименование)
                        .Distinct()
                        .OrderBy(o => o)
                        .ToList();

                    listCells.Insert(0, "");

                    return listCells;
                }
            });
        }

        public bool UpdateStoreCell(Guid guid, string cell)
        {
            return exceptionTrap.Catch(() =>
            {
                using (SomeLinqDataContext sl = new SomeLinqDataContext(builder.ConnectionString))
                {
                    sl.CommandTimeout = connectionTimeOut;
                    if (cell == "")
                    {
                        var n = sl.НоменклатурнаяПозицияs
                            .Where(
                                w => w.Oid == guid)
                            .Select(
                                s => s)
                            .FirstOrDefault();

                        n.ЯчейкаОтпуска = (Guid?)null;
                        sl.SubmitChanges();
                    }

                    Guid cellGui = sl.СкладскаяЯчейкаs
                        .Where(
                            w => w.Наименование == cell)
                        .Select(
                            s => s.Oid)
                        .FirstOrDefault();

                    var nn = sl.НоменклатурнаяПозицияs
                        .Where(
                            w => w.Oid == guid)
                        .Select(
                            s => s)
                        .FirstOrDefault();

                    nn.ЯчейкаОтпуска = cellGui;
                    sl.SubmitChanges();
                }
            });
        }

        public bool NewCell(string name)
        {
            return exceptionTrap.Catch(() => 
            {
                ado.CommandWithParameter(
                    "insert into СкладскаяЯчейка (Oid, Код, Наименование, СозданПользователем, ДатаСоздания) values (newid(), '" 
                    + name + "', '" + name + "', '" + userAndMachineName + "', @dateTime)", "@dateTime", DateTime.Now);
            });
        }
    }
}
