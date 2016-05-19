using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfClientLibrary
{
    public interface IWcfClient
    {
        void SetUrl(string url);
        List<Store> GetStoresList();
    }
}
