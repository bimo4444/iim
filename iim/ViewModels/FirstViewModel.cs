using Entity;
using MVPLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace iim.ViewModels
{
    public class FirstViewModel : VMBase
    {
        private UserControl selectedMenu;
        public UserControl SelectedMenu
        {
            get { return selectedMenu; }
            set
            {
                selectedMenu = value;
                OnPropertyChanged("SelectedMenu");
            }
        }
        private List<Store> _listBoxItems;
        public List<Store> ListBoxItems
        {
            get { return _listBoxItems; }
            set
            {
                _listBoxItems = value;
                OnPropertyChanged("ListBoxItems");
            }
        }
        private bool _listEnabled;
        public bool ListEnabled
        {
            get { return _listEnabled; }
            private set
            {
                _listEnabled = value;
                OnPropertyChanged("ListEnabled");
            }
        }
    }
}
