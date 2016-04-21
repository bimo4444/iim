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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace iim
{
    class Presenter : IPresenter
    {
        ICore core;

        MainWindow mainWindow = new MainWindow();
        MainViewModel mainViewModel = new MainViewModel();

        FirstView firstView = new FirstView();
        FirstViewModel firstViewModel = new FirstViewModel();

        FirstViewMenu firstViewMenu = new FirstViewMenu();
        MenuViewModel menuViewModel = new MenuViewModel();

        MainMenu mainMenu;

        PrimaryView primaryView;
        PrimaryViewModel primaryViewModel;

        MovementView movementView;
        MovementViewModel movementViewModel;

        Dialog connectionErrorDialog;
        ConnectionErrorView connectionErrorView;

        Dialog confirmationDialog;
        ConfirmationView confirmationView;

        private List<UserControl> oldViews;

        public Presenter(ICore core)
        {
            this.core = core;
            SetViewModels();
            Begin();
            Initializing();
            SetOtherViewModels();
            Bindings();
            Subscribes();
            ShowFirstView();
        }

        private void ShowMovement()
        {
            throw new NotImplementedException();
        }

        private void ShowGrid()
        {
            throw new NotImplementedException();
        }

        private void Subscribes()
        {
            mainWindow.Closing += OnShutdown;
            primaryView.tableView.CellValueChanged += OnTableViewCellChanged;
            primaryView.tableView.KeyDown += OnPrimaryTableViewKeyDown;
            primaryView.gridControl.SelectionChanged += OnPrimaryGridControlSelectionChanged;
            movementView.tableView.KeyDown += OnMovementTableViewKeyDown;
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

            primaryViewModel.CopyToClipboard = new DelegateCommand(() => CopyPrimaryGridControlValue());
            movementViewModel.CopyToClipboard = new DelegateCommand(() => CopyMovementGridControlValue());
        }

        string previousStoreCellValue;
        Item primaryViewGridControlSelected;
        private void OnPrimaryGridControlSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            primaryViewGridControlSelected = (Item)primaryView.gridControl.SelectedItem;
            previousStoreCellValue = primaryViewGridControlSelected.StoreCell ?? "";
        }

        ////
        private void OnTableViewCellChanged(object sender, CellValueChangedEventArgs e)
        {
            //Guid guid = ((Item)e.Row).OidUnit;
            //string cell = ((Item)e.Row).StoreCell;
            //if(core.CheckCellAdequacy(cell))
            //{
            //    core.UpdateStoreCell(guid, cell);
            //}
            //else
            //{
            //    string newCell = core.NormalizeStoreCell(cell);
            //    if (ConfirmationView("Добавить новую ячейку:  " + newCell))
            //    {
            //        core.AddNewStoreCell(newCell);
            //        ((Item)e.Row).StoreCell = newCell;
            //    }
            //    else
            //        ((Item)e.Row).StoreCell = previousStoreCellValue;
            //}
        }
        private bool ConfirmationView(string text)
        {
            bool menuVisible = mainViewModel.MenuVisible;
            mainViewModel.MenuVisible = false;
            mainViewModel.SelectedView = confirmationView;
            confirmationDialog.Show(text);
            mainViewModel.SelectedView = oldViews.Last();
            mainViewModel.MenuVisible = menuVisible;
            return confirmationDialog.result ?? false;
        }
        private bool ConnectionErrorView()
        {
            bool menuVisible = mainViewModel.MenuVisible;
            mainViewModel.MenuVisible = false;
            mainViewModel.SelectedView = connectionErrorView;
            connectionErrorDialog.Show();
            if (connectionErrorDialog.result == true)
            {
                mainViewModel.SelectedView = oldViews.Last();
                mainViewModel.MenuVisible = menuVisible;
                return true;
            }
            else
                Application.Current.Shutdown();
            //
            return false;
        }

        private void CopyMovementGridControlValue()
        {
            Clipboard.SetText(movementView.gridControl.GetFocusedValue().ToString());
        }

        private void CopyPrimaryGridControlValue()
        {
            Clipboard.SetText(primaryView.gridControl.GetFocusedValue().ToString());
        }

        private void SelectStoresGroup()
        {
            //firstViewModel.ListBoxItems = core.SelectStoresGroups();
        }

        private void UncheckSelectedStores()
        {
            //firstViewModel.ListBoxItems = core.UncheckSelectedStores();
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
            core.Refresh();
            ResetData();
        }

        private void ResetData()
        {
            primaryViewModel.ListItems = core.GetPrimaryItems();
            movementViewModel.ListItems = core.GetMovementItems();
            primaryViewGridControlSelected = null;
        }

        private void ExcelReportMovement()
        {
            //core.ExportToExcel(movementView.tableView);
        }

        private void ExcelReportGrid()
        {
            //core.ExportToExcel(primaryView.tableView);
        }



        private void OnHideMenuClick(object sender, MouseButtonEventArgs e)
        {
            menuViewModel.MenuVisible = !menuViewModel.MenuVisible;
            menuViewModel.MenuNotVisible = !menuViewModel.MenuNotVisible;
        }

        private void OnMaxDateEditDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //core.ResetMaxDate();
        }

        private void OnMinDateEditDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //core.ResetMinDate();
        }

        private void OnPrimaryTableViewKeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(primaryView.gridControl.GetFocusedValue().ToString());
        }

        private void OnMovementTableViewKeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(movementView.gridControl.GetFocusedValue().ToString());
        }
        private void OnKeyDown(string s, KeyEventArgs e = null)
        {
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                Clipboard.SetText(s);
                if(e != null)
                    e.Handled = true;
            }
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

            menuViewModel.Task = core.SomeUser.TaskGrouping;
            menuViewModel.Zeros = core.SomeUser.Zeros;
            menuViewModel.Stat = core.SomeUser.StatGrouping;
            menuViewModel.Party = core.SomeUser.PartyGrouping;
            menuViewModel.Order = core.SomeUser.OrderRPGrouping;
            menuViewModel.Minus = core.SomeUser.Minus;

            //using(DisabledControls disabledControls = new DisabledControls(
            //    ref mainViewModel, ref menuViewModel, ref firstViewModel))
            //{
            //    firstViewModel.ListBoxItems = await Task.Run(() => core.GetStoresList());
            //}
        }

        private void OnShutdown(object sender, CancelEventArgs e)
        {
            //core.OnShutDown();
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
            mainViewModel.SelectedView = userControl;
            oldViews.Add(userControl);
        }

        private void SetOtherViewModels()
        {
            mainMenu.DataContext = menuViewModel;
            primaryView.DataContext = primaryViewModel;
            movementView.DataContext = movementViewModel;
        }

        private void Initializing()
        {
            mainMenu = new MainMenu();
            primaryView = new PrimaryView();
            primaryViewModel = new PrimaryViewModel();
            movementView = new MovementView();
            movementViewModel = new MovementViewModel();
            connectionErrorView = new ConnectionErrorView();
            connectionErrorDialog = new Dialog(connectionErrorView);
            confirmationView = new ConfirmationView();
            confirmationDialog = new Dialog(confirmationView);
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
