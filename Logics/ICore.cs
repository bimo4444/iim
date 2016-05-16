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
        IEnumerable<Item> CurrentPrimaryList { get; }
        IEnumerable<string> StoreCells { get; }
        DateTime CurrentMaxDateTime { get; set; }
        DateTime CurrentMinDateTime { get; set; }
        void Begin();
        bool NotEmpty();
        List<Store> GetStoresList();
        List<Store> UncheckSelectedStores();
        List<Store> SelectStoresGroups();
        IEnumerable<Item> GetPrimaryItems();
        IEnumerable<Item> ResetPrimary();
        IEnumerable<Item> GetMovementItems(Guid guidUnit, Guid guidStore);
        IEnumerable<string> GetStoreCellsList();
        bool CheckCellExists(string cell);
        string NormalizeStoreCell(string cell);
        void UpdateStoreCell(Guid guid, string cell, string newCell);
        void AddNewStoreCell(Guid guid, string newCell);
        void ExportToExcel(TableView tableView, string excelFileName);
        DateTime ResetMinDate();
        DateTime ResetMaxDate();
        void OnShutDown();
    }
}
