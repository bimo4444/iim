using DataAccess;
using DevExpress.Xpf.Grid;
using Entity;
using ExcelServices;
using Metamorphosis;
using Serializer;
using StoreCellsNormalizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfClientLibrary;

namespace Logics
{
    public class Core : ICore
    {
        IDataProvider dataProvider;
        IWcfClient wcfClient;
        IMetamorphoses metamorphosis;
        IXmlSerializer xmlSerializer;
        IExcelService excelService;
        ICellsNormalizer cellsNormalizer;
        Config config;
        public UserConfig SomeUser { get; set; }
        public IEnumerable<Item> CurrentPrimaryList { get; private set; }
        public IEnumerable<string> StoreCells { get; private set; }
        IEnumerable<Item> primaryList;
        IEnumerable<Item> movementList;
        DateTime MaxDateTime;
        DateTime MinDateTime;
        public DateTime CurrentMaxDateTime { get; set; }
        public DateTime CurrentMinDateTime { get; set; }
        //readonly string configFilePath = "config";
        readonly string configFileName = "config\\iimConfig.xml";
        readonly string userConfigFilePath = "users";
        readonly string userConfigFileName = "users\\" + Environment.UserName + ".xml";

        public Core(
            IDataProvider dataProvider,
            IWcfClient wcfClient,
            IMetamorphoses metamorphosis,
            IXmlSerializer xmlSerializer,
            IExcelService excelService,
            ICellsNormalizer cellsNormalizer)
        {
            this.metamorphosis = metamorphosis;
            this.xmlSerializer = xmlSerializer;
            this.excelService = excelService;
            this.dataProvider = dataProvider;
            this.wcfClient = wcfClient;
            this.cellsNormalizer = cellsNormalizer;
            DeserializeConfigs();
            dataProvider.Configure(config.ConnectionString);
            dataProvider.Configure(config.ConnectionTimeOut);
            dataProvider.Init();
            wcfClient.SetUrl(config.WcfServiceAddress);
            Initializing();
        }
        private void DeserializeConfigs()
        {
            //serializer on error returns empty
            config = xmlSerializer.Deserialize<Config>(configFileName);
            SomeUser = xmlSerializer.Deserialize<UserConfig>(userConfigFileName);
        }
        private void Initializing()
        {
            MaxDateTime = CurrentMaxDateTime = DateTime.Now.Date;
            MinDateTime = CurrentMinDateTime = DateTime.MinValue;
        }
        //first view listbox
        public List<Store> GetStoresList()
        {
            List<Store> result = new List<Store>();
            if (config.UsingWcfService)
                result = wcfClient.GetStoresList();
            if (!result.Any())
                result = dataProvider.GetStoresList();
            //nested
            return SomeUser.StoresList.Any() ?
                (SomeUser.StoresList = result
                .Select(s => new Store
                {
                    OidStore = s.OidStore,
                    Higher = s.Higher,
                    StoreString = s.StoreString,
                    IsSelected =
                        (SomeUser.StoresList
                            .Where(w => w.IsSelected == true)
                            .Select(ss => ss.OidStore))
                        .Contains(s.OidStore)
                })
                .ToList()) : (SomeUser.StoresList = result);
        }
        public List<Store> UncheckSelectedStores()
        {
            return (SomeUser.StoresList = SomeUser.StoresList
                   .Select(s => new Store
                   {
                       Higher = s.Higher,
                       IsSelected = false,
                       OidStore = s.OidStore,
                       StoreString = s.StoreString
                   })
                   .ToList());
        }
        public List<Store> SelectStoresGroups()
        {            
            //nested
            var h = SomeUser.StoresList
                .Where(w => w.IsSelected)
                .Select(s => s.Higher)
                .Distinct();
            return (SomeUser.StoresList = SomeUser.StoresList
                .Select(s => new Store
                {
                    Higher = s.Higher,
                    IsSelected = h.Contains(s.Higher),
                    OidStore = s.OidStore,
                    StoreString = s.StoreString
                })
                .ToList());
        }
        //primary view
        public IEnumerable<Item> GetPrimaryItems()
        {
            primaryList = dataProvider.GetBaseQuery(SomeUser.StoresList.Where(w => w.IsSelected).Select(s => s.OidStore).ToList());
            MinDateTime = CurrentMinDateTime = primaryList.Min(m => m.Date) ?? DateTime.MinValue;
            return CurrentPrimaryList = PrimaryMetamorphosis(primaryList);
        }
        public IEnumerable<Item> ResetPrimary()
        {
            return CurrentPrimaryList = PrimaryMetamorphosis(primaryList);
        }
        private IEnumerable<Item> PrimaryMetamorphosis(IEnumerable<Item> primaryList)
        {
            CurrentPrimaryList = primaryList;
            if(CurrentMaxDateTime != MaxDateTime)
                CurrentPrimaryList = metamorphosis.CutDates(CurrentPrimaryList, CurrentMaxDateTime);
            CurrentPrimaryList = metamorphosis.Grouping(CurrentPrimaryList, SomeUser.PartyGrouping, SomeUser.OrderRPGrouping, SomeUser.TaskGrouping, SomeUser.StatGrouping);
            if (SomeUser.Minus)
                CurrentPrimaryList = metamorphosis.CutMinus(CurrentPrimaryList);
            if(!SomeUser.Zeros)
                CurrentPrimaryList = CurrentPrimaryList.Where(w => w.Quantity != (decimal)0 || w.Remains != (decimal)0).ToList();
            return CurrentPrimaryList;
        }
        //movement view
        public IEnumerable<Item> GetMovementItems(Guid guidUnit, Guid guidStore)
        {
            return movementList = MovementMetamorphosis(primaryList, guidUnit, guidStore);
        }
        private IEnumerable<Item> MovementMetamorphosis(IEnumerable<Item> primaryList, Guid guidUnit, Guid guidStore)
        {
            movementList = primaryList.Where(w => w.OidUnit == guidUnit && w.OidStore == guidStore).ToList();
            movementList = metamorphosis.GetRemains(movementList);
            if (SomeUser.Minus)
                movementList = metamorphosis.CutMinus(movementList);
            if (CurrentMaxDateTime != MaxDateTime || CurrentMinDateTime != MinDateTime)
                movementList = metamorphosis.CutDates(movementList, CurrentMinDateTime, CurrentMaxDateTime);
            return movementList;
        }
        //cells
        public IEnumerable<string> GetStoreCellsList()
        {
            return StoreCells = dataProvider.GetStoreCells();
        }
        public bool CheckCellExists(string cell)
        {
            return StoreCells.Contains(cell) ? true : StoreCells.Contains(cellsNormalizer.Normalize(cell));
        }
        public string NormalizeStoreCell(string cell)
        {
            return cellsNormalizer.Normalize(cell);
        }
        public void UpdateStoreCell(Guid guid, string cell, string newCell)
        {
            dataProvider.UpdateStoreCell(guid, newCell);
            primaryList = metamorphosis.RenameCells(primaryList, guid, newCell);
            CurrentPrimaryList = PrimaryMetamorphosis(primaryList);
        }
        public void AddNewStoreCell(Guid guid, string newCell)
        {
            dataProvider.NewCell(newCell);
            var v = StoreCells.ToList();
            v.Add(newCell);
            StoreCells = v.OrderBy(o => o);
            primaryList = metamorphosis.RenameCells(primaryList, guid, newCell);
            CurrentPrimaryList = PrimaryMetamorphosis(primaryList);
        }
        //
        public void ExportToExcel(TableView tableView, string excelFileName)
        {
            excelService.Export(tableView, excelFileName);
        }
        public DateTime ResetMaxDate()
        {
            return CurrentMaxDateTime = MaxDateTime;
        }
        public DateTime ResetMinDate()
        {
            return CurrentMinDateTime = MinDateTime;
        }
        public void OnShutDown()
        {
            xmlSerializer.Serialize(SomeUser, userConfigFilePath, userConfigFileName);
            //for the first date
            //if (!Directory.Exists(configFilePath))
            //    Directory.CreateDirectory(configFilePath);
            //if (!File.Exists(configFileName))
            //    xmlSerializer.Serialize(config, configFilePath, configFileName);
        }
    }
}