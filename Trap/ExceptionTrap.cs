using Logging;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Trap
{
    public class ExceptionTrap : IExceptionTrap
    {
        ILog log;
        public ExceptionTrap(ILog log)
        {
            this.log = log;
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
                log.Write(x);
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
                log.Write(x);
                return Activator.CreateInstance<T>();
            }
        }
    }
}
