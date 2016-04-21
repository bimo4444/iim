using DataAccess;
using DevExpress.Xpf.Grid;
using Entity;
using ExcelServices;
using Metamorphosis;
using Serializer;
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
        public List<string> StoreCells { get; set; }

        public UserConfig SomeUser { get; set; }
        Config config;


        readonly string configFilePath = "config";
        readonly string configFileName = "config\\iimConfig.xml";

        readonly string userConfigFilePath = "users";
        readonly string userConfigFileName = "users\\" + Environment.UserName + ".xml";

        IDataProvider dataProvider;
        IMetamorphoses metamorphosis;
        IXmlSerializer xmlSerializer;
        IExcelService excelService;
        //ICellsNormalizer cellsNormalizer;

        public Core(
            IDataProvider dataProvider,
            IMetamorphoses metamorphosis,
            IXmlSerializer xmlSerializer,
            IExcelService excelService)

        {
            this.metamorphosis = metamorphosis;
            this.xmlSerializer = xmlSerializer;
            this.excelService = excelService;
            this.dataProvider = dataProvider;
            //dataProvider.Configure(config.ConnectionString);
            //dataProvider.Configure(config.ConnectionTimeOut);

            Initializing();
            DeserializeConfigs();

        }

        private void Initializing()
        {

        }

        private void DeserializeConfigs()
        {
            config = xmlSerializer.Deserialize<Config>(configFileName);
            SomeUser = xmlSerializer.Deserialize<UserConfig>(userConfigFileName);
        }

        public List<Store> GetStoresList()
        {
            //nested
            var storesList = dataProvider.GetStoresList();
            return SomeUser.ls.Count > 0 ? (SomeUser.ls = storesList
                .Select(s => new Store
                {
                    OidStore = s.OidStore,
                    Higher = s.Higher,
                    StoreString = s.StoreString,
                    IsSelected = 
                        (SomeUser.ls 
                            .Where(w => w.IsSelected == true)
                            .Select(ss => ss.OidStore))
                        .Contains(s.OidStore)
                })
                .ToList()) : (SomeUser.ls = storesList);
        }

        public List<Store> SelectStoresGroups()
        {
            //nested
            var h = SomeUser.ls
                .Where(w => w.IsSelected)
                .Select(s => s.Higher)
                .Distinct();
            return (SomeUser.ls = SomeUser.ls
                .Where(w => h.Contains(w.Higher))
                .Select(s => new Store 
                { 
                    Higher = s.Higher, 
                    IsSelected = true, 
                    OidStore = s.OidStore, 
                    StoreString = s.StoreString 
                })
                .ToList());
        }

        public List<Store> UncheckSelectedStores()
        {
            return (SomeUser.ls = SomeUser.ls
                .Select(s => new Store
                {
                    Higher = s.Higher,
                    IsSelected = false,
                    OidStore = s.OidStore,
                    StoreString = s.StoreString
                })
                .ToList());
        }

        public bool CheckCellAdequacy(string cell)
        {
            throw new NotImplementedException();

            //cell = cellsNormalizer.Normalize(cell);

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
            throw new NotImplementedException();
        }

        public void ExportToExcel(TableView globalDevExpressXpfGridTableView)
        {
            throw new NotImplementedException();
        }


        public void ResetMaxDate()
        {
            throw new NotImplementedException();
        }

        public void ResetMinDate()
        {
            throw new NotImplementedException();
        }

        public void OnShutDown()
        {
            xmlSerializer.Serialize(SomeUser, userConfigFilePath, userConfigFileName);
            //xmlSerializer.Serialize(config, configFilePath, configFileName);
        }

        public List<Item> GetPrimaryItems()
        {
            throw new NotImplementedException();
        }

        public List<Item> GetMovementItems()
        {
            throw new NotImplementedException();
        }
    }
}
