﻿using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metamorphosis
{
    public interface IMetamorphoses
    {
        IEnumerable<Item> CutMinus(IEnumerable<Item> t);
        IEnumerable<Item> GetRemains(IEnumerable<Item> lm);
        IEnumerable<Item> CutDates(IEnumerable<Item> t, DateTime max);
        IEnumerable<Item> CutDates(IEnumerable<Item> t, DateTime min, DateTime max);
        IEnumerable<Item> RenameCells(IEnumerable<Item> items, Guid guid, string newCell);
        IEnumerable<Item> Grouping(IEnumerable<Item> t, bool party, bool order, bool task, bool stat);
    }
}
