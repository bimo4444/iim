using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logics
{
    public interface ICore
    {
        List<Item> GetPrimaryItems();
        List<Item> GetMovementItems();

        void Refresh();

        UserConfig SomeUser { get; set; }
    }
}
