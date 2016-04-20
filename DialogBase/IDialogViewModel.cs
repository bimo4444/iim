using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DialogBase
{
    public interface IDialogViewModel
    {
        ICommand OK { get; set; }
        ICommand Cancel { get; set; }

        string Text { get; set; }
    }
}
