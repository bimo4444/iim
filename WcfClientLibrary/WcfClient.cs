using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using Trap;
using WcfContract;

namespace WcfClientLibrary
{
    public class WcfClient : IWcfClient
    {
        string url;
        IExceptionTrap trap;
        public WcfClient(IExceptionTrap trap)
        {
            this.trap = trap;
        }
        public void SetUrl(string url)
        {
            this.url = url;
        }
        public List<Store> GetStoresList()
        {
            return trap.Catch(delegate()
            {
                List<Store> result;
                using (ChannelFactory<IService> cf = new ChannelFactory<IService>(
                    new WebHttpBinding(), url))
                {
                    cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                    IService channel = cf.CreateChannel();
                    result = channel.GetStoresList();
                }
                return result;
            });
        }
    }
}
