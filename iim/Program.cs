using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ninject;
using System.Reflection;
using System.Threading;
using System.Runtime.InteropServices;
using Uniqueness;

namespace iim
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Unique unique = new Unique();
                if (unique.AlreadyRunning("iim"))
                    return;

                SplashScreen ss = new SplashScreen(
                    "Resources/box-icon-251019.png");
                ss.Show(true);

                Presenter p = new Presenter();
                App app = new App();
                app.Run();
            }
            catch (Exception x)
            {
                var kernel = new StandardKernel();
                kernel.Load(Assembly.GetExecutingAssembly());
                ILog log = kernel.Get<ILog>();
                log.Write(x);

                Main();
            }
        }
    }
}
