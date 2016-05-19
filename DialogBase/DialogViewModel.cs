using MVPLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DialogBase
{
    public class DialogViewModel : VMBase, IDialogViewModel
    {
        public ICommand OK { get; set; }
        public ICommand Cancel { get; set; }

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }
    }
}
