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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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
        MainMenu mainMenu = new MainMenu();
        MainMenuPrimaryFrame primaryMenuFrame = new MainMenuPrimaryFrame();
        MainMenuMovementFrame movementMenuFrame = new MainMenuMovementFrame();
        PrimaryView primaryView = new PrimaryView();
        PrimaryViewModel primaryViewModel = new PrimaryViewModel();
        MovementView movementView = new MovementView();
        MovementViewModel movementViewModel = new MovementViewModel();
        Dialog connectionErrorDialog;
        ConnectionErrorView connectionErrorView = new ConnectionErrorView();
        Dialog confirmationDialog;
        ConfirmationView confirmationView = new ConfirmationView();
        private List<UserControl> oldViews = new List<UserControl>();
        string previousStoreCellValue;
        Guid movementGuidUnit, movementGuidStore;
        public Presenter(ICore core)
        {
            this.core = core;
            Initializing();
            GetStores();
            Bindings();
            Subscribes();
            SetViewModels();
            SetProperties();
            Show();
        }
        private void Initializing()
        {
            connectionErrorDialog = new Dialog(connectionErrorView);
            confirmationDialog = new Dialog(confirmationView);
        }
        //stores for the first view
        private async void GetStores()
        {
            await Task.Factory.StartNew(() =>
            {
                DisableControls();
                mainViewModel.StatusBarText = "загрузка...";
                Loop(() => DownloadStoresList(), () => firstViewModel.ListBoxItems.Any());
                menuViewModel.MaxDateTime = menuViewModel.CurrentMaxDateTime = core.CurrentMaxDateTime;
                mainViewModel.StatusBarText = "";
                ActivateControls();
            });
        }
        private void DownloadStoresList()
        {
            Task.Factory.StartNew(() => firstViewModel.ListBoxItems = core.GetStoresList()).Wait();
        }
        private void Bindings()
        {
            menuViewModel.ShowPrimaryView = new DelegateCommand(() => ShowPrimaryView());
            menuViewModel.SelectGroup = new DelegateCommand(() => SelectStoresGroup());
            menuViewModel.UncheckAllButton = new DelegateCommand(() => UncheckSelectedStores());
            menuViewModel.ShowMovementView = primaryViewModel.ShowMovement = new DelegateCommand(() => ShowMovement());
            menuViewModel.ExcelReport = primaryViewModel.ExcelReport = new DelegateCommand(() => ExcelReport(primaryView.tableView));
            menuViewModel.ExcelReportMov = movementViewModel.ExcelReportMov = new DelegateCommand(() => ExcelReport(movementView.tableView));
            menuViewModel.PreviousControl = movementViewModel.PreviousControl = new DelegateCommand(() => PreviousControl());
            menuViewModel.Refresh = primaryViewModel.Refresh = movementViewModel.Refresh = new DelegateCommand(() => RefreshData());
            primaryViewModel.CopyToClipboard = new DelegateCommand(() => CopyGridControlValue(primaryView.gridControl));
            movementViewModel.CopyToClipboard = new DelegateCommand(() => CopyGridControlValue(movementView.gridControl));
        }
        private void Subscribes()
        {
            menuViewModel.PropertyChanged += OnMenuPropertyChanged;
            movementView.tableView.KeyDown += OnMovementTableViewKeyDown;
            primaryView.tableView.KeyDown += OnPrimaryTableViewKeyDown;
            primaryView.tableView.CellValueChanged += OnTableViewCellChanged;
            primaryView.gridControl.SelectedItemChanged += OnPrimaryGridControlSelectionChanged;
            mainMenu.minDateEdit.MouseDoubleClick += OnMinDateEditDoubleClick;
            mainMenu.maxDateEdit.MouseDoubleClick += OnMaxDateEditDoubleClick;
            firstViewMenu.maxDateEdit.MouseDoubleClick += OnMaxDateEditDoubleClick;
            mainMenu.menuStack.MouseDown += OnHideMenuClick;
            mainMenu.hiddeenMenuStack.MouseDown += OnHideMenuClick;
            firstView.listBox.SelectionChanged += OnFirstViewSelectionChanged;
            mainWindow.Closing += OnShutdown;
        }
        private void SetViewModels()
        {
            mainWindow.DataContext = mainViewModel;
            firstView.DataContext = firstViewModel;
            firstViewMenu.DataContext = menuViewModel;
            mainMenu.DataContext = menuViewModel;
            primaryView.DataContext = primaryViewModel;
            movementView.DataContext = movementViewModel;
        }
        private void SetProperties()
        {
            menuViewModel.Task = core.SomeUser.TaskGrouping;
            menuViewModel.Zeros = core.SomeUser.Zeros;
            menuViewModel.Stat = core.SomeUser.StatGrouping;
            menuViewModel.Party = core.SomeUser.PartyGrouping;
            menuViewModel.Order = core.SomeUser.OrderRPGrouping;
            menuViewModel.Minus = core.SomeUser.Minus;
        }
        private void Show()
        {
            //to unhide menu first time (show/hide arrows)
            menuViewModel.MenuVisible = true;
            mainViewModel.SelectedMenu = mainMenu;
            firstViewModel.SelectedMenu = firstViewMenu;
            mainViewModel.SelectedView = firstView;
            oldViews.Add(firstView);
            mainWindow.Show();
        }
        private void SelectStoresGroup()
        {
            firstViewModel.ListBoxItems = core.SelectStoresGroups();
        }
        private void UncheckSelectedStores()
        {
            firstViewModel.ListBoxItems = core.UncheckSelectedStores();
        }
        private async void ShowPrimaryView()
        {
            await Task.Factory.StartNew(() =>
                {
                    DisableControls();
                    mainViewModel.StatusBarText = "загрузка...";
                    Loop(() => DownloadPrimaryList(), () => core.NotEmpty());
                    menuViewModel.MinDateTime = menuViewModel.CurrentMinDateTime = core.CurrentMinDateTime;
                    menuViewModel.Time = DateTime.Now.ToShortTimeString();
                    menuViewModel.TotalDays = (core.CurrentMaxDateTime - core.CurrentMinDateTime).Days + 1;
                    ChangeCurrentView(primaryView);
                    mainViewModel.StatusBarText = "";
                    ActivateControls();
                });
        }
        private void DownloadPrimaryList()
        {
            Task.Factory.StartNew(() => 
                {
                    primaryViewModel.ListCells = core.GetStoreCellsList();
                    primaryViewModel.ListItems = core.GetPrimaryItems();
                }).Wait();
        }
        private void Loop(Action action, Func<bool> wayOut)
        {
            action.Invoke();
            while (!wayOut())
            {
                ActivateControls();
                if (ConnectionErrorView())
                {
                    DisableControls();
                    action.Invoke();
                }
            }
        }
        private void ShowMovement()
        {
            DisableControls();
            movementGuidUnit = primaryViewModel.SelectedItem.OidUnit;
            movementGuidStore = primaryViewModel.SelectedItem.OidStore;
            movementViewModel.ListItems = core.GetMovementItems(movementGuidUnit, movementGuidStore);
            ChangeCurrentView(movementView);
            ActivateControls();
        }
        private async void RefreshData()
        {
            await Task.Factory.StartNew(() =>
            {
                DisableControls();
                mainViewModel.StatusBarText = "загрузка...";
                menuViewModel.MovButtonEnabled = false;
                Loop(() => DownloadPrimaryList(), () => primaryViewModel.ListItems.Any());
                if (oldViews.Contains(movementView))
                    movementViewModel.ListItems = core.GetMovementItems(movementGuidUnit, movementGuidStore);
                mainViewModel.StatusBarText = "";
                ActivateControls();
            });
        }
        private void Metamorphosis()
        {
            DisableControls();
            if (oldViews.Contains(movementView))
            {
                primaryViewModel.ListItems = core.ResetPrimary();
                movementViewModel.ListItems = core.GetMovementItems(movementGuidUnit, movementGuidStore);
            }
            else if (oldViews.Contains(primaryView))
                primaryViewModel.ListItems = core.ResetPrimary();
            ActivateControls();
        }
        private void ExcelReport(TableView tableView)
        {
            string excelFileName = ShowSaveFileDialog();
            if (excelFileName != "")
                core.ExportToExcel(tableView, excelFileName);
        }
        private void CopyGridControlValue(GridControl gridControl)
        {
            Clipboard.SetText(gridControl.GetFocusedValue().ToString());
        }
        //views routine
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
            else if (oldViews.Last() == movementView)
            {
                menuViewModel.SelectedFrame = movementMenuFrame;
                menuViewModel.GridButtonsVisibility = false;
            }
        }
        private void ActivateControls()
        {
            DisableControls(false);
        }
        private void DisableControls(bool b = true)
        {
            mainViewModel.CursorState = b ? Cursors.Wait : Cursors.Arrow;
            menuViewModel.ControlsEnabled = !b;
            menuViewModel.ButtonsEnabled = !b;
            firstViewModel.ListEnabled = !b;
        }
        private string ShowSaveFileDialog()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel file (*.xls)|*.xls";
            sfd.FileName = "Наличие ТМЦ " + DateTime.Now.ToString("d.MM.yyyy HH-mm-ss");
            return sfd.ShowDialog() == true ? sfd.FileName : "";
        }
        //subscribes
        private void OnMenuPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bool refresh = false, dates = false;
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
                    core.SomeUser.PartyGrouping = primaryViewModel.PartyVisible = movementViewModel.PartyVisible = menuViewModel.Party;
                    refresh = true;
                    break;
                case ("Order"):
                    core.SomeUser.OrderRPGrouping = primaryViewModel.OrderVisible = movementViewModel.OrderVisible = menuViewModel.Order;
                    refresh = true;
                    break;
                case ("Task"):
                    core.SomeUser.TaskGrouping = primaryViewModel.TaskVisible = movementViewModel.TaskVisible = menuViewModel.Task;
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
            if (!refresh || !oldViews.Any() || oldViews.Last() == firstView)
                return;
            if (dates)
                menuViewModel.TotalDays = (core.CurrentMaxDateTime - core.CurrentMinDateTime).Days + 1;
            Metamorphosis();
        }
        private void OnMinDateEditDoubleClick(object sender, MouseButtonEventArgs e)
        {
            menuViewModel.MinDateTime = core.ResetMinDate();
            menuViewModel.CurrentMinDateTime = core.CurrentMinDateTime;
            menuViewModel.TotalDays = (core.CurrentMaxDateTime - core.CurrentMinDateTime).Days + 1;
            Metamorphosis();
        }
        private void OnMaxDateEditDoubleClick(object sender, MouseButtonEventArgs e)
        {
            menuViewModel.MaxDateTime = core.ResetMaxDate();
            menuViewModel.CurrentMaxDateTime = core.CurrentMaxDateTime;
            if (oldViews.Last() == firstView)
                return;
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
        //changing cells
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
                    primaryViewModel.SelectedItem = null;
                    core.AddNewStoreCell(guid, newCell);
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
        private void OnPrimaryTableViewKeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(primaryView.gridControl, e);
        }
        private void OnMovementTableViewKeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(movementView.gridControl, e);
        }
        private void OnKeyDown(GridControl gridControl, KeyEventArgs e)
        {
            if (gridControl != null && e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                Clipboard.SetText(gridControl.GetFocusedValue().ToString());
                if (e != null)
                    e.Handled = true;
            }
        }
        private void OnShutdown(object sender, CancelEventArgs e)
        {
            core.OnShutDown();
        }
    }
}