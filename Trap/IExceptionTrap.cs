using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trap
{
    public interface IExceptionTrap
    {
        T Catch<T>(Func<T> func);
        bool Catch(Action action);
    }
}
