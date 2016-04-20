using MVPLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace iim.ViewModels
{
    public class MainViewModel : VMBase
    {
        private string _statusBarText;
        public string StatusBarText
        {
            get { return _statusBarText; }
            set
            {
                _statusBarText = value;
                OnPropertyChanged("StatusBarText");
            }
        }
        private bool _menuVisible;
        public bool MenuVisible
        {
            get { return _menuVisible; }
            set
            {
                _menuVisible = value;
                OnPropertyChanged("MenuVisible");
            }
        }
        private UserControl selectedView;
        public UserControl SelectedView
        {
            get { return selectedView; }
            set
            {
                selectedView = value;
                OnPropertyChanged("SelectedView");
            }
        }
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
        private Cursor _cursorState;
        public Cursor CursorState
        {
            get { return _cursorState; }
            set
            {
                _cursorState = value;
                OnPropertyChanged("CursorState");
            }
        }
    }
}
