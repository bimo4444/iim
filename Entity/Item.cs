using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Item
    {
        public Guid OidUnit { get; set; }
        public Guid OidStore { get; set; }
        public Guid OidBaseDimension { get; set; }
        public DateTime? Date { get; set; }

        public string DateString { get; set; }
        public string DocumentNumber { get; set; }
        public string User { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string Movement { get; set; }
        public string QuantityString { get; set; }
        public string StoreString { get; set; }
        public string StroreName { get; set; }
        public string StoreKey { get; set; }
        public string StoreCell { get; set; }
        public string ComtecNumber { get; set; }
        public string KeyArticle { get; set; }
        public string UnitName { get; set; }
        public string UnitMeasurement { get; set; }
        public string Group { get; set; }
        public string Party { get; set; }
        public decimal Quantity { get; set; }
        public decimal Remains { get; set; }
        public decimal Rest { get; set; }
        public string Refill { get; set; }
        public string OrderRP { get; set; }
        public int? Transition { get; set; }
        public string Ready { get; set; }
        public string Task { get; set; }
        public string Stat { get; set; }
        public string NextOperation { get; set; }
    }
}
