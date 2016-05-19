using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelServices
{
    public interface IExcelService
    {
        void OpenWorkbook(string path);
        void Export(TableView tv, string path);
    }
}
