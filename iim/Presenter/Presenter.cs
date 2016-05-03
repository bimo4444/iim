using DevExpress.Xpf.Grid;
using DialogBase;
using Entity;
using iim.ViewModels;
using iim.Views;
using Logics;
using Microsoft.Win32;
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
        MainMenuPrimaryFrame primaryMenuFrame;
        MainMenuMovementFrame movementMenuFrame;

        PrimaryView primaryView;
        PrimaryViewModel primaryViewModel;

        MovementView movementView;
        MovementViewModel movementViewModel;

        Dialog connectionErrorDialog;
        ConnectionErrorView connectionErrorView;

        Dialog confirmationDialog;
        ConfirmationView confirmationView;

        private List<UserControl> oldViews;

        string previousStoreCellValue;

        public Presenter(ICore core)
        {
            this.core = core;
            SetViewModels();
            ShowFirstView();
            Begin();
            DownloadStoresListAsync();
            Initializing();
            SetOtherViewModels();
            Bindings();
            Subscribes();
        }

        private void Begin()
        {
            oldViews = new List<UserControl>();
            oldViews.Add(firstView);

            menuViewModel.MenuVisible = true;
            mainViewModel.SelectedMenu = mainMenu;

            menuViewModel.Task = core.SomeUser.TaskGrouping;
            menuViewModel.Zeros = core.SomeUser.Zeros;
            menuViewModel.Stat = core.SomeUser.StatGrouping;
            menuViewModel.Party = core.SomeUser.PartyGrouping;
            menuViewModel.Order = core.SomeUser.OrderRPGrouping;
            menuViewModel.Minus = core.SomeUser.Minus;
        }
        private void DownloadStoresListAsync()// async
        {
            DisableControls(true);
            //firstViewModel.ListBoxItems = await Task.Run(() => core.GetStoresList());
            menuViewModel.MaxDateTime = menuViewModel.CurrentMaxDateTime = core.CurrentMaxDateTime;
            DisableControls(false);
        }

        private void ShowFirstView()
        {
            firstViewModel.SelectedMenu = firstViewMenu;
            mainViewModel.SelectedView = firstView;
            mainWindow.Show();
        }

        private void ShowPrimaryView()// async
        {
            DisableControls(true);
            //primaryViewModel.ListCells = await Task.Run(() => core.GetStoreCellsList());
            //primaryViewModel.ListItems = await Task.Run(() => core.GetPrimaryItems());
            menuViewModel.MinDateTime = menuViewModel.CurrentMinDateTime = core.CurrentMinDateTime;
            ChangeCurrentView(primaryView);
            menuViewModel.Time = DateTime.Now.ToShortTimeString();
            menuViewModel.TotalDays = (core.CurrentMaxDateTime - core.CurrentMinDateTime).Days + 1;
            DisableControls(false);
        }

        Guid movementGuidUnit, movementGuidStore;
        private void ShowMovement()
        {
            DisableControls(true);
            movementGuidUnit = primaryViewModel.SelectedItem.OidUnit;
            movementGuidStore = primaryViewModel.SelectedItem.OidStore;
            movementViewModel.ListItems = core.GetMovementItems(movementGuidUnit, movementGuidStore);
            ChangeCurrentView(movementView);
            DisableControls(false);
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
        private void ChangeCurrentView(UserControl userControl)
        {
            if (!mainViewModel.MenuVisible)
                mainViewModel.MenuVisible = true;
            mainViewModel.SelectedView = userControl;
            oldViews.Add(userControl);
            SetMenuMode();
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
                mainViewModel.StatusBarText = "";
                return;
            }
            else
                SetMenuMode();
        }
        private void SetMenuMode()
        {
            if (oldViews.Last() == primaryView)
            {
                menuViewModel.SelectedFrame = primaryMenuFrame;
                menuViewModel.GridButtonsVisibility = true;
            }
                
            if (oldViews.Last() == movementView)
            {
                menuViewModel.SelectedFrame = movementMenuFrame;
                menuViewModel.GridButtonsVisibility = false;
            }
        }
        private void RefreshData()
        {
            primaryViewModel.ListItems = core.GetPrimaryItems();
            if(oldViews.Contains(movementView))
                movementViewModel.ListItems = core.GetMovementItems(movementGuidUnit, movementGuidStore);
        }
        private void Metamorphosis()
        {
            DisableControls(true);
            if (oldViews.Contains(movementView))
            {
                primaryViewModel.ListItems = core.ResetPrimary();
                movementViewModel.ListItems = core.GetMovementItems(movementGuidUnit, movementGuidStore);
                return;
            }
            if (oldViews.Contains(primaryView))
                primaryViewModel.ListItems = core.ResetPrimary();
            DisableControls(false);
        }

        private void DisableControls(bool b)
        {
            mainViewModel.CursorState =
                b ? Cursors.Wait : Cursors.Arrow;
            menuViewModel.ControlsEnabled = !b;
            menuViewModel.ButtonsEnabled = !b;
            firstViewModel.ListEnabled = !b;
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
            primaryMenuFrame = new MainMenuPrimaryFrame();
            movementMenuFrame = new MainMenuMovementFrame();
        }
        private void SetViewModels()
        {
            mainWindow.DataContext = mainViewModel;
            firstView.DataContext = firstViewModel;
            firstViewMenu.DataContext = menuViewModel;
        }

        private void Bindings()
        {
            menuViewModel.ShowPrimaryView = new DelegateCommand(() => ShowPrimaryView());
            menuViewModel.SelectGroup = new DelegateCommand(() => SelectStoresGroup());
            menuViewModel.UncheckAllButton = new DelegateCommand(() => UncheckSelectedStores());
            menuViewModel.ShowMovementView = primaryViewModel.ShowMovement = new DelegateCommand(() => ShowMovement());
            menuViewModel.ExcelReport = primaryViewModel.ExcelReport = new DelegateCommand(() => ExcelReportGrid());
            menuViewModel.PreviousControl = movementViewModel.PreviousControl = new DelegateCommand(() => PreviousControl());
            menuViewModel.ExcelReportMov = movementViewModel.ExcelReportMov = new DelegateCommand(() => ExcelReportMovement());
            menuViewModel.Refresh = primaryViewModel.Refresh = movementViewModel.Refresh = new DelegateCommand(() => RefreshData());
            primaryViewModel.CopyToClipboard = new DelegateCommand(() => CopyPrimaryGridControlValue());
            movementViewModel.CopyToClipboard = new DelegateCommand(() => CopyMovementGridControlValue());
        }
        private void ExcelReportGrid()
        {
            string excelFileName = ShowSaveFileDialog();
            if (excelFileName == "")
                return;
            core.ExportToExcel(primaryView.tableView, excelFileName);
        }
        private void ExcelReportMovement()
        {
            string excelFileName = ShowSaveFileDialog();
            if (excelFileName == "")
                return;
            core.ExportToExcel(movementView.tableView, excelFileName);
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
            firstViewModel.ListBoxItems = core.SelectStoresGroups();
        }
        private void UncheckSelectedStores()
        {
            firstViewModel.ListBoxItems = core.UncheckSelectedStores();
        }

        private void Subscribes()
        {
            mainWindow.Closing += OnShutdown;
            primaryView.tableView.CellValueChanged += OnTableViewCellChanged;
            primaryView.tableView.KeyDown += OnPrimaryTableViewKeyDown;
            primaryView.gridControl.SelectedItemChanged += OnPrimaryGridControlSelectionChanged;
            movementView.tableView.KeyDown += OnMovementTableViewKeyDown;
            mainMenu.minDateEdit.MouseDoubleClick += OnMinDateEditDoubleClick;
            mainMenu.maxDateEdit.MouseDoubleClick += OnMaxDateEditDoubleClick;
            mainMenu.menuStack.MouseDown += OnHideMenuClick;
            firstView.listBox.SelectionChanged += OnFirstViewSelectionChanged;
            menuViewModel.PropertyChanged += OnMenuPropertyChanged;
        }
        private void SetOtherViewModels()
        {
            mainMenu.DataContext = menuViewModel;
            primaryView.DataContext = primaryViewModel;
            movementView.DataContext = movementViewModel;
        }
        private void OnMenuPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bool refresh = false;
            bool dates = false;
            switch (e.PropertyName)
            {
                case ("Stat"):
                    core.SomeUser.StatGrouping = primaryViewModel.StatVisible = menuViewModel.Stat;
                    refresh = true;
                    break;
                case ("Zeros"):
                    core.SomeUser.Zeros = menuViewModel.Zeros;
                    refresh = true;
                    break;
                case ("Minus"):
                    core.SomeUser.Minus = menuViewModel.Minus;
                    refresh = true;
                    break;
                case ("Party"):
                    core.SomeUser.PartyGrouping = primaryViewModel.PartyVisible = menuViewModel.Party;
                    refresh = true;
                    break;
                case ("Order"):
                    core.SomeUser.OrderRPGrouping = primaryViewModel.OrderVisible = menuViewModel.Order;
                    refresh = true;
                    break;
                case ("Task"):
                    core.SomeUser.TaskGrouping = primaryViewModel.TaskVisible = menuViewModel.Task;
                    refresh = true;
                    break;
                case ("CurrentMinDateTime"):
                    core.CurrentMinDateTime = menuViewModel.CurrentMinDateTime;
                    refresh = dates = true;
                    break;
                case ("CurrentMaxDateTime"):
                    core.CurrentMaxDateTime = menuViewModel.CurrentMaxDateTime;
                    refresh = dates = true;
                    break;
            }
            if (!refresh || oldViews.Last() == firstView)
                return;
            if (dates)
                menuViewModel.TotalDays = (core.CurrentMaxDateTime - core.CurrentMinDateTime).Days + 1;
            Metamorphosis();
        }
        private void OnFirstViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            menuViewModel.ButtonsEnabled = firstView.listBox.SelectedItems.Count > 0 ? true : false;
            menuViewModel.StoresCounter = firstView.listBox.SelectedItems.Count;
            if (e != null)
                e.Handled = true;
        }
        private void OnPrimaryGridControlSelectionChanged(object sender, SelectedItemChangedEventArgs e)
        {
            if (primaryViewModel.SelectedItem != null)
            {
                previousStoreCellValue = primaryViewModel.SelectedItem.StoreCell;
                mainViewModel.StatusBarText = menuViewModel.MovementDetails = String.Format(
                    "{0} | {1} | {2} | Есть: {3}",
                    primaryViewModel.SelectedItem.UnitName,
                    primaryViewModel.SelectedItem.KeyArticle,
                    primaryViewModel.SelectedItem.ComtecNumber,
                    primaryViewModel.SelectedItem.Quantity);
                if (!menuViewModel.MovButtonEnabled)
                    menuViewModel.MovButtonEnabled = true;
            }
            else
                menuViewModel.MovButtonEnabled = false;
        }
        private void OnTableViewCellChanged(object sender, CellValueChangedEventArgs e)
        {
            Guid guid = ((Item)e.Row).OidUnit;
            string cell = ((Item)e.Row).StoreCell;
            if (core.CheckCellExists(cell))
            {
                core.UpdateStoreCell(guid, previousStoreCellValue, cell);
                primaryViewModel.ListItems = core.CurrentPrimaryList;
            }
            else
            {
                string newCell = core.NormalizeStoreCell(cell);

                if (ConfirmationView("Добавить новую ячейку:  " + newCell))
                {
                    core.AddNewStoreCell(cell, newCell);
                    primaryViewModel.ListCells = core.StoreCells;
                    primaryViewModel.ListItems = core.CurrentPrimaryList;
                }
                else
                    ((Item)e.Row).StoreCell = previousStoreCellValue;
            }
        }
        private void OnHideMenuClick(object sender, MouseButtonEventArgs e)
        {
            menuViewModel.MenuVisible = !menuViewModel.MenuVisible;
            menuViewModel.MenuNotVisible = !menuViewModel.MenuNotVisible;
        }
        private void OnMaxDateEditDoubleClick(object sender, MouseButtonEventArgs e)
        {
            menuViewModel.MaxDateTime = core.ResetMaxDate();
        }
        private void OnMinDateEditDoubleClick(object sender, MouseButtonEventArgs e)
        {
            menuViewModel.MinDateTime = core.ResetMinDate();
        }
        private void OnPrimaryTableViewKeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(primaryView.gridControl.GetFocusedValue().ToString(), e);
        }
        private void OnMovementTableViewKeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(movementView.gridControl.GetFocusedValue().ToString(), e);
        }
        private void OnKeyDown(string s, KeyEventArgs e)
        {
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                Clipboard.SetText(s);
                if (e != null)
                    e.Handled = true;
            }
        }
        private void OnShutdown(object sender, CancelEventArgs e)
        {
            core.OnShutDown();
        }
        private string ShowSaveFileDialog()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel file (*.xls)|*.xls";
            sfd.FileName = "Наличие ТМЦ " + DateTime.Now.ToString("d.MM.yyyy HH-mm-ss");
            return sfd.ShowDialog() == true ? sfd.FileName : "";
        }
    }
}
