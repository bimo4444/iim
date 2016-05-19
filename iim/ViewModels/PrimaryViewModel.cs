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
    class PrimaryViewModel : GridViewModelBase
    {
        public ICommand ShowMovement { get; set; }
        public ICommand ExcelReport { get; set; }

        private bool _statVisible;
        public bool StatVisible
        {
            get { return _statVisible; }
            set
            {
                _statVisible = value;
                OnPropertyChanged("StatVisible");
            }
        }

        //LIST OF STORE CELLS
        private IEnumerable<string> _listCells;
        public IEnumerable<string> ListCells
        {
            get { return _listCells; }
            set
            {
                _listCells = value;
                OnPropertyChanged("ListCells");
            }
        }

        //SELECTED GRID ROW
        private Item _selectedItem;
        public Item SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                //OnPropertyChanged("SelectedItem");
            }
        }
    }
}
