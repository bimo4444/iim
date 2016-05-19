using Entity;
using MVPLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace iim.ViewModels
{
    public class MovementViewModel : GridViewModelBase
    {
        public ICommand PreviousControl { get; set; }
        public ICommand ExcelReportMov { get; set; }
    }
}
