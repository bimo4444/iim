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

        //comment
        readonly string xmlFilePath = "xml";
        readonly string xmlFileName = "xml\\xml.xml";

        readonly string configFilePath = "config";
        readonly string configFileName = "config\\iimConfig.xml";

        readonly string userConfigFilePath = "users";
        readonly string userConfigFileName = "users\\" + Environment.UserName + ".xml";

        IDataProvider dataProvider;
        IWcfClient wcfClient;
        IMetamorphoses metamorphosis;
        IXmlSerializer xmlSerializer;
        IExcelService excelService;
        ICellsNormalizer cellsNormalizer;


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
            dataProvider.Initialize();

            wcfClient.SetUrl(config.WcfServiceAddress);

            Initializing();
        }

        public List<Store> GetStoresList()
        {
            //comment this
            //return SomeUser.StoresList.Count() > 0 ? SomeUser.StoresList : (SomeUser.StoresList = dataProvider.GetStoresList());
            List<Store> result = new List<Store>();
            if(config.UsingWcfService)
                result = wcfClient.GetStoresList();
            if (result.Count == 0)
                result = dataProvider.GetStoresList();

            //nested
            return SomeUser.StoresList.Count() > 0 ? 
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

        private void Initializing()
        {
            MaxDateTime = CurrentMaxDateTime = DateTime.Now.Date;
            MinDateTime = CurrentMinDateTime = DateTime.MinValue;
        }

        private void DeserializeConfigs()
        {
            config = xmlSerializer.Deserialize<Config>(configFileName);
            SomeUser = xmlSerializer.Deserialize<UserConfig>(userConfigFileName);
        }

        public bool CheckCellExists(string cell)
        {
            return StoreCells.Contains(cell) ? true : StoreCells.Contains(cellsNormalizer.Normalize(cell));
        }

        public void UpdateStoreCell(Guid guid, string cell, string newCell)
        {
            dataProvider.UpdateStoreCell(guid, newCell);
            primaryList = metamorphosis.RenameCells(primaryList, cell, newCell);
            CurrentPrimaryList = PrimaryMetamorphosis(primaryList);
        }

        public string NormalizeStoreCell(string cell)
        {
            return cellsNormalizer.Normalize(cell);
        }

        public void AddNewStoreCell(string cell, string newCell)
        {
            dataProvider.NewCell(newCell);
            var v = StoreCells.ToList();
            v.Add(newCell);
            StoreCells = v.OrderBy(o => o);
            primaryList = metamorphosis.RenameCells(primaryList, cell, newCell);
            CurrentPrimaryList = PrimaryMetamorphosis(primaryList);
        }

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
            if (!Directory.Exists(configFilePath))
                Directory.CreateDirectory(configFilePath);
            if (!File.Exists(configFileName))
                xmlSerializer.Serialize(config, configFilePath, configFileName);

            //delete
            if (!Directory.Exists(xmlFilePath))
                Directory.CreateDirectory(xmlFilePath);
            if (!File.Exists(xmlFileName))
                xmlSerializer.Serialize(primaryList.ToList(), xmlFilePath, xmlFileName);
        }

        public IEnumerable<Item> GetPrimaryItems()
        {
            //delete
            primaryList = xmlSerializer.Deserialize<List<Item>>(xmlFileName);
            //uncomment
            //primaryList = dataProvider.GetBaseQuery(SomeUser.StoresList.Where(w => w.IsSelected).Select(s => s.OidStore).ToList());
            MinDateTime = CurrentMinDateTime = primaryList.Min(m => m.Date) ?? DateTime.MinValue;
            return CurrentPrimaryList = PrimaryMetamorphosis(primaryList);
        }

        public IEnumerable<string> GetStoreCellsList()
        {
            return StoreCells = dataProvider.GetStoreCells();
        }

        public IEnumerable<Item> GetMovementItems(Guid guidUnit, Guid guidStore)
        {
            return movementList = MovementMetamorphosis(primaryList, guidUnit, guidStore);
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

        public IEnumerable<Item> ResetPrimary()
        {
            return CurrentPrimaryList = PrimaryMetamorphosis(primaryList);
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

        private IEnumerable<Item> PrimaryMetamorphosis(IEnumerable<Item> primaryList)
        {
            CurrentPrimaryList = primaryList;
            if(CurrentMaxDateTime != MaxDateTime || CurrentMinDateTime != MinDateTime)
                CurrentPrimaryList = metamorphosis.CutDates(CurrentPrimaryList, CurrentMinDateTime, CurrentMaxDateTime);
            CurrentPrimaryList = metamorphosis.Grouping(CurrentPrimaryList, SomeUser.PartyGrouping, SomeUser.OrderRPGrouping, SomeUser.TaskGrouping, SomeUser.StatGrouping);
            if (SomeUser.Minus)
                CurrentPrimaryList = metamorphosis.CutMinus(CurrentPrimaryList);
            if(!SomeUser.Zeros)
                CurrentPrimaryList = CurrentPrimaryList.Where(w => w.Quantity != (decimal)0 || w.Remains != (decimal)0).ToList();
            return CurrentPrimaryList;
        }
    }
}
