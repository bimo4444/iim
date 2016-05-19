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
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            try
            {
                Unique unique = new Unique();
                if (unique.AlreadyRunning("iim"))
                    return;

                SplashScreen ss = new SplashScreen(
                    "Resources/box-icon-251019.png");
                ss.Show(true);

                IPresenter presenter = kernel.Get<IPresenter>();

                App app = new App();
                app.Run();
            }

            catch (Exception x)
            {
                ILog log = kernel.Get<ILog>();
                log.Write(x);
            }
        }
    }
}
