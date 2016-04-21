using DevExpress.Xpf.Grid;
using DialogBase;
using Entity;
using iim.ViewModels;
using iim.Views;
using Logics;
using MVPLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace iim
{
    class Presenter
    {
        Core core = new Core();

        MainWindow mainWindow = new MainWindow();
        MainViewModel mainViewModel = new MainViewModel();

        FirstView firstView = new FirstView();
        FirstViewModel firstViewModel = new FirstViewModel();

        FirstViewMenu firstViewMenu = new FirstViewMenu();
        MenuViewModel menuViewModel = new MenuViewModel();

        SomeUser someUser = new SomeUser();

        MainMenu mainMenu;

        PrimaryView primaryView;
        PrimaryViewModel primaryViewModel;

        MovementView movementView;
        MovementViewModel movementViewModel;

        ConnectionErrorView connectionErrorView;
        Dialog dialog;

        private List<UserControl> oldViews;

        public Presenter()
        {
            SetViewModels();
            ShowFirstView();
            Begin();
            InitializeOtherObjects();
            SetOtherViewModels();
            Bindings();
            Subscribes();
        }

        private void Subscribes()
        {
            mainWindow.Closing += OnShutdown;
            primaryView.tableView.CellValueChanged += OnTableViewCellChanged;
            primaryView.tableView.KeyDown += OnTableViewKeyDown;
            movementView.tableView.KeyDown += OnTableViewKeyDown;
            mainMenu.minDateEdit.MouseDoubleClick += OnMinDateEditDoubleClick;
            mainMenu.maxDateEdit.MouseDoubleClick += OnMaxDateEditDoubleClick;
            mainMenu.menuStack.MouseDown += OnHideMenuClick;
        }

        private void Bindings()
        {
            menuViewModel.ShowGrid = new DelegateCommand(() => ShowGrid());
            menuViewModel.SelectGroup = new DelegateCommand(() => SelectStoresGroup());
            menuViewModel.UncheckAllButton = new DelegateCommand(() => UncheckSelectedStores());
            menuViewModel.ShowMovement = primaryViewModel.ShowMovement = new DelegateCommand(() => ShowMovement());
            menuViewModel.ExcelReport = primaryViewModel.ExcelReport = new DelegateCommand(() => ExcelReportGrid());
            menuViewModel.PreviousControl = movementViewModel.PreviousControl = new DelegateCommand(() => PreviousControl());
            menuViewModel.ExcelReportMov = movementViewModel.ExcelReportMov = new DelegateCommand(() => ExcelReportMovement());
            menuViewModel.Refresh = primaryViewModel.Refresh = movementViewModel.Refresh = new DelegateCommand(() => RefreshData());
        }

        private void SelectStoresGroup()
        {
            firstViewModel.ListBoxItems = core.SelectStoresGroup();
        }

        private void UncheckSelectedStores()
        {
            firstViewModel.ListBoxItems = core.UncheckSelectedStores();
        }

        private void PreviousControl()
        {
            if (oldViews.Count < 2)
                return;
            oldViews.RemoveAt(oldViews.Count - 1);
            mainViewModel.SelectedView = oldViews.Last();
            //firstView
            if (oldViews.Count < 2)
            {
                mainViewModel.MenuVisible = false;
                return;
            }
            MenuVisibility(mainViewModel.SelectedView);
        }

        //changes buttons visibility for mainMenu
        private void MenuVisibility<T>(T t)
        {
            if (primaryView.GetType() == t.GetType())
                menuViewModel.GridButtonsVisibility = true;
            else
                menuViewModel.GridButtonsVisibility = false;
        }

        private void RefreshData()
        {
            throw new NotImplementedException();
        }

        private void ExcelReportMovement()
        {
            throw new NotImplementedException();
        }

        private void ExcelReportGrid()
        {
            throw new NotImplementedException();
        }

        private void ShowMovement()
        {
            throw new NotImplementedException();
        }

        private void ShowGrid()
        {
            throw new NotImplementedException();
        }

        private void OnHideMenuClick(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnMaxDateEditDoubleClick(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnMinDateEditDoubleClick(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnTableViewKeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnTableViewCellChanged(object sender, CellValueChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ShowFirstView()
        {
            firstViewModel.SelectedMenu = firstViewMenu;
            mainViewModel.SelectedView = firstView;
            mainWindow.Show();
        }

        private async void Begin()
        {
            oldViews = new List<UserControl>();
            oldViews.Add(firstView);
            menuViewModel.MenuVisible = true;
            someUser = core.SomeUser;

            using(DisabledControls disabledControls = new DisabledControls(
                ref mainViewModel, ref menuViewModel, ref firstViewModel))
            {
                firstViewModel.ListBoxItems = await Task.Run(() => core.GetStoresList());
            }
        }

        private void OnShutdown(object sender, CancelEventArgs e)
        {
            core.SomeUser = someUser;
            core.SaveConfig();
        }

        //private void DisableControls(bool b)
        //{
        //    mainViewModel.CursorState =
        //        b ? Cursors.Wait : Cursors.Arrow;
        //    menuViewModel.ControlsEnabled = !b;
        //    firstViewModel.ListEnabled = !b;
        //}

        private void ChangeCurrentView(UserControl userControl)
        {
            if (!mainViewModel.MenuVisible)
                mainViewModel.MenuVisible = true;
            oldViews.Add(userControl);
            mainViewModel.SelectedView = userControl;
        }

        private void SetOtherViewModels()
        {
            mainMenu.DataContext = menuViewModel;
            primaryView.DataContext = primaryViewModel;
            movementView.DataContext = movementViewModel;
        }

        private void InitializeOtherObjects()
        {
            mainMenu = new MainMenu();
            primaryView = new PrimaryView();
            primaryViewModel = new PrimaryViewModel();
            movementView = new MovementView();
            movementViewModel = new MovementViewModel();
            connectionErrorView = new ConnectionErrorView();
            dialog = new Dialog(connectionErrorView);
            oldViews = new List<UserControl>();
        }

        private void SetViewModels()
        {
            mainWindow.DataContext = mainViewModel;
            firstView.DataContext = firstViewModel;
            firstViewMenu.DataContext = menuViewModel;
        }

    }

    class DisabledControls : IDisposable
    {
        MainViewModel mainViewModel;
        MenuViewModel menuViewModel;
        FirstViewModel firstViewModel;

        public DisabledControls(
            ref MainViewModel mainViewModel, 
            ref MenuViewModel menuViewModel, 
            ref FirstViewModel firstViewModel)
        {
            this.mainViewModel = mainViewModel;
            this.menuViewModel = menuViewModel;
            this.firstViewModel = firstViewModel;
            this.mainViewModel.CursorState = Cursors.Wait;
            this.menuViewModel.ControlsEnabled = 
                this.firstViewModel.ListEnabled = false;
        }
        public void Dispose()
        {
            this.mainViewModel.CursorState = Cursors.Arrow;
            this.menuViewModel.ControlsEnabled =
                this.firstViewModel.ListEnabled = true;
        }
    }
}
