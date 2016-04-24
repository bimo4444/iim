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

        IEnumerable<Item> CurrentPrimaryList { get; private set; }
        List<string> StoreCells { get; private set; }
        List<Store> GetStoresList();
        List<Store> SelectStoresGroups();
        List<Store> UncheckSelectedStores();

        IEnumerable<Item> GetPrimaryItems();
        IEnumerable<Item> GetMovementItems(Guid guid);
        IEnumerable<Item> ResetPrimary();
        IEnumerable<Item> ResetMovement(Guid guid);
        IEnumerable<string> GetStoreCellsList();
        IEnumerable<Item> RenameCells(string cell, string newCell);

        void Refresh();
        void ExportToExcel(TableView tableView);
        bool CheckCellExists(string cell);
        void UpdateStoreCell(Guid guid, string cell);
        string NormalizeStoreCell(string cell);
        void AddNewStoreCell(string newCell);
        void OnShutDown();

    }
}
