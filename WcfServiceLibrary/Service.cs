using DataAccess;
using Entity;
using Ninject;
using Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WcfServiceLibrary
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single)]
    public class Service : IService
    {
        Timer timer;
        List<Store> stores;
        Config config;
        string configPath = "config";
        string configFilePath = "config\\config.xml";
        IXmlSerializer serializer;
        IDataProvider provider;
        public Service()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            serializer = kernel.Get<IXmlSerializer>();
            provider = kernel.Get<IDataProvider>();
            config = serializer.Deserialize<Config>(configFilePath);
            serializer.Serialize(config, configPath, configFilePath);
            provider.Configure(config.ConnectionTimeOut);
            provider.Configure(config.ConnectionString);
            provider.Initialize();
            stores = new List<Store>();
            OnTimerEvent(null, null);
            StartTimer();
        }
        private void OnTimerEvent(object sender, ElapsedEventArgs e)
        {
            GetStoresQueryAsync();
        }
        private async void GetStoresQueryAsync()
        {
            stores = await Task.Run(() => provider.GetStoresList());
        }
        private void StartTimer()
        {
            timer = new Timer(3600000);//1h
            timer.Elapsed += OnTimerEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }
        public List<Store> GetStoresList()
        {
            return stores.Count > 0 ? stores : new List<Store>();
        }
    }
}
