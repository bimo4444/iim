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
    public class GridViewModelBase : VMBase
    {
        public ICommand Refresh { get; set; }
        public ICommand CopyToClipboard { get; set; }

        //GRID MAIN COLLECTION
        private List<Item> _listItems;
        public List<Item> ListItems
        {
            get { return _listItems; }
            set
            {
                _listItems = value;
                OnPropertyChanged("ListItems");
            }
        }

        private bool _taskVisible;
        public bool TaskVisible
        {
            get { return _taskVisible; }
            private set
            {
                _taskVisible = value;
                OnPropertyChanged("TaskVisible");
            }
        }
        private bool _partyVisible;
        public bool PartyVisible
        {
            get { return _partyVisible; }
            private set
            {
                _partyVisible = value;
                OnPropertyChanged("PartyVisible");
            }
        }
        private bool _orderVisible;
        public bool OrderVisible
        {
            get { return _orderVisible; }
            private set
            {
                _orderVisible = value;
                OnPropertyChanged("OrderVisible");
            }
        }
    }
}
