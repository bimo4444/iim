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

namespace Logics
{
    public class Core : ICore
    {
        Config config;
        public UserConfig SomeUser { get; set; }

        IEnumerable<string> storeCells;
        IEnumerable<Item> primaryList;
        IEnumerable<Item> currentPrimaryList;
        IEnumerable<Item> movementList;

        DateTime MaxDateTime;
        DateTime MinDateTime;
        public DateTime CurrentMaxDateTime { get; set; }
        public DateTime CurrentMinDateTime { get; set; }
        
        readonly string configFilePath = "config";
        readonly string configFileName = "config\\iimConfig.xml";

        readonly string userConfigFilePath = "users";
        readonly string userConfigFileName = "users\\" + Environment.UserName + ".xml";

        IDataProvider dataProvider;
        IMetamorphoses metamorphosis;
        IXmlSerializer xmlSerializer;
        IExcelService excelService;
        ICellsNormalizer cellsNormalizer;

        public Core(
            IDataProvider dataProvider,
            IMetamorphoses metamorphosis,
            IXmlSerializer xmlSerializer,
            IExcelService excelService,
            ICellsNormalizer cellsNormalizer)

        {
            this.metamorphosis = metamorphosis;
            this.xmlSerializer = xmlSerializer;
            this.excelService = excelService;
            this.dataProvider = dataProvider;
            DeserializeConfigs();

            dataProvider.Configure(config.ConnectionString);
            dataProvider.Configure(config.ConnectionTimeOut);
            dataProvider.Initialize();

            Initializing();
        }

        public List<Store> GetStoresList()
        {
            //comment this
            return SomeUser.StoresList.Count() > 0 ? SomeUser.StoresList : (SomeUser.StoresList = dataProvider.GetStoresList());

            //nested
            var storesList = dataProvider.GetStoresList();
            return SomeUser.StoresList.Count() > 0 ? (SomeUser.StoresList = storesList
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
                .ToList()) : (SomeUser.StoresList = storesList);
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
            return storeCells.Contains(cell) ? true : storeCells.Contains(cellsNormalizer.Normalize(cell));
        }

        public void UpdateStoreCell(Guid guid, string cell)
        {
            throw new NotImplementedException();
        }

        public string NormalizeStoreCell(string cell)
        {
            throw new NotImplementedException();
        }

        public void AddNewStoreCell(string newCell)
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            primaryList = dataProvider.GetBaseQuery(SomeUser.StoresList.Where(w => w.IsSelected).Select(s => s.OidStore).ToList());
        }

        public void ExportToExcel(TableView tableView)
        {
            throw new NotImplementedException();
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
        }

        public IEnumerable<Item> GetPrimaryItems()
        {
            primaryList = dataProvider.GetBaseQuery(SomeUser.StoresList.Where(w => w.IsSelected).Select(s => s.OidStore).ToList());
            MinDateTime = CurrentMinDateTime = primaryList.Min(m => m.Date) ?? DateTime.MinValue;
            return currentPrimaryList = PrimaryMetamorphosis(primaryList);
        }

        public IEnumerable<string> GetStoreCellsList()
        {
            return dataProvider.GetStoreCells();
        }

        public IEnumerable<Item> GetMovementItems(Guid guid)
        {
            movementList = SelectMovement(guid);
            return movementList = MovementMetamorphosis(movementList);
        }

        private IEnumerable<Item> SelectMovement(Guid guid)
        {
            throw new NotImplementedException();
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
            return currentPrimaryList = PrimaryMetamorphosis(primaryList);
        }

        public IEnumerable<Item> ResetMovement()
        {
            return currentPrimaryList = MovementMetamorphosis(primaryList);
        }

        private IEnumerable<Item> MovementMetamorphosis(IEnumerable<Item> primaryList)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Item> PrimaryMetamorphosis(IEnumerable<Item> primaryList)
        {
            if(CurrentMaxDateTime != MaxDateTime || CurrentMinDateTime != MinDateTime)
                primaryList = metamorphosis.CutDates(primaryList, CurrentMinDateTime, CurrentMaxDateTime);
            if (!SomeUser.Minus)
                primaryList = metamorphosis.CutMinus(primaryList);
            primaryList = metamorphosis.Grouping(primaryList, SomeUser.PartyGrouping, SomeUser.OrderRPGrouping, SomeUser.TaskGrouping, SomeUser.StatGrouping)
            return primaryList;
        }
    }
}
