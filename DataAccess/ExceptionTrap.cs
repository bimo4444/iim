using Logging;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ExceptionTrap
    {
        ILog log;

        public ILog GetLogger()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel.Get<ILog>();
        }

        public bool Catch(Action action)
        {
            try
            {
                action.Invoke();
                return true;
            }
            catch (Exception x)
            {
                (log ?? (log = GetLogger())).Write(x);
                return false;
            }
        }

        public T Catch<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception x)
            {
                (log ?? (log = GetLogger())).Write(x);
                return Activator.CreateInstance<T>();
            }
        }
    }
}
