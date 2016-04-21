using DataAccess;
using DevExpress.Xpf.Grid;
using Entity;
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
        Config config;
        public SomeUser SomeUser { get; set; }
        XmlSerializer xmlSerializer = new XmlSerializer();

        readonly string configFilePath = "config";
        readonly string configFileName = "config\\iimConfig.xml";

        readonly string userConfigFilePath = "users";
        readonly string userConfigFileName = "users" + "\\" + Environment.UserName + ".xml";

        DataProvider provider;

        public Core()
        {
            config = xmlSerializer.Deserialize<Config>(configFileName);
            SomeUser = xmlSerializer.Deserialize<SomeUser>(userConfigFileName);
            provider = new DataProvider(config.ConnectionString, config.ConnectionTimeOut);
        }

        public void SaveConfig()
        {
            xmlSerializer.Serialize(SomeUser, userConfigFilePath, userConfigFileName);
        }


        public List<Store> GetStoresList()
        {
            var storesList = provider.GetStoresList();
            var checkedStores = SomeUser.ls
                .Where(w => w.IsSelected == true)
                .Select(s => s.OidStore)
                .ToList();
            return storesList
                .Select(s => new Store 
                { 
                    OidStore = s.OidStore, 
                    Higher = s.Higher, 
                    StoreString = s.StoreString, 
                    IsSelected = checkedStores.Contains(s.OidStore) 
                })
                .ToList();
        }

        public List<Store> SelectStoresGroup()
        {
            throw new NotImplementedException();
        }

        public List<Store> UncheckSelectedStores()
        {
            throw new NotImplementedException();
        }

        public bool CheckCellAdequacy(string cell)
        {
            throw new NotImplementedException();
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

        public void RefreshData()
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
    }
}
