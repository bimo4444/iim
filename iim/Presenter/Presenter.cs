using iim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iim
{
    class Presenter
    {
        MainWindow mainWindow = new MainWindow();
        MainViewModel mainWiewModel = new MainViewModel();

        public Presenter()
        {
            SetViewModels();
            mainWindow.Show();
        }

        private void SetViewModels()
        {
            mainWindow.DataContext = mainWiewModel;
        }
    }
}
