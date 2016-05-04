using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace WcfContract
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        List<Store> GetStoresList();
    }
}
