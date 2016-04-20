using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using MVPLight;

namespace DialogBase
{
    public class Dialog
    {
        private event EventHandler PropertyChanged;
        DispatcherFrame frame = new DispatcherFrame();

        IDialogViewModel viewModel = new DialogViewModel();

        bool? _result;
        public bool? result 
        {
            get { return _result; }
            private set 
            {
                _result = value;
                EventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            } 
        }

        public Dialog(UserControl userControl, IDialogViewModel viewModel = null)
        {
            viewModel.OK = new DelegateCommand(() => result = true);
            viewModel.Cancel = new DelegateCommand(() => result = false);
            userControl.DataContext = viewModel ?? this.viewModel;
        }

        public void Show(string text = null)
        {
            viewModel.Text = text;
            PropertyChanged += OnPropertyChanged;
            Dispatcher.PushFrame(frame);
        }

        private void OnPropertyChanged(object sender, EventArgs e)
        {
            frame.Continue = false;
            PropertyChanged -= OnPropertyChanged;
        }
    }

    //example:

    //using DialogBase;//using MVPLight;

    //Dialog dialog;

    ////some view with xaml binding Ok and Cancel commands
    ////optional Text binding

    //SomeView sv = new SomeView();
    //private bool ShowDialog(string text)
    //{
    //    if (dialog == null)
    //        dialog = new Dialog(sv);

    //    mainViewModel.CurrentView = sv;////sv.Show();
    //    dialog.Show(text);

    //    return dialog.result ?? false;
    //}
}
