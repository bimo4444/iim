using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metamorphosis
{
    public interface IMetamorphoses
    {
        List<Item> CutMinus(List<Item> t);
        List<Item> GetRemains(List<Item> lm);
        List<Item> CutDates(List<Item> t, DateTime min, DateTime max);
        List<Item> Grouping(
            List<Item> t, bool party, bool order, bool task, bool stat);
    }
}
