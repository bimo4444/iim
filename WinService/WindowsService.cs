using Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using WcfServiceLibrary;

namespace WinService
{
    public partial class WindowsService : ServiceBase
    {
        WebServiceHost host;

        public WindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                host = new WebServiceHost(typeof(Service), new Uri("http://localhost:666/test"));
                ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IService), new WebHttpBinding(), "");
                host.Open();
            }
            catch (Exception x)
            {
                Log log = new Log();
                log.Write(x);
                if (host != null)
                {
                    host.Abort();
                    host = null;
                }
            }
        }

        protected override void OnStop()
        {
            if (host != null)
            {
                host.Abort();
                host = null;
            }
        }
    }
}
