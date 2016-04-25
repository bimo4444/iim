using DevExpress.Xpf.Grid;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logics
{
    public interface ICore
    {
        UserConfig SomeUser { get; set; }

        DateTime CurrentMaxDateTime { get; set; }
        DateTime CurrentMinDateTime { get; set; }
        DateTime ResetMinDate();
        DateTime ResetMaxDate();

        IEnumerable<Item> CurrentPrimaryList { get; }
        IEnumerable<string> StoreCells { get; }
        List<Store> GetStoresList();
        List<Store> SelectStoresGroups();
        List<Store> UncheckSelectedStores();

        IEnumerable<Item> GetPrimaryItems();
        IEnumerable<Item> GetMovementItems(Guid guidUnit, Guid guidStore);
        IEnumerable<Item> ResetPrimary();
        IEnumerable<string> GetStoreCellsList();

        void ExportToExcel(TableView tableView, string excelFileName);
        bool CheckCellExists(string cell);
        void UpdateStoreCell(Guid guid, string cell);
        string NormalizeStoreCell(string cell);
        void AddNewStoreCell(string cell, string newCell);
        void OnShutDown();
    }
}
