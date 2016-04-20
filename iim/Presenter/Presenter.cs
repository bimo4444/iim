using DialogBase;
using Entity;
using iim.ViewModels;
using iim.Views;
using Logics;
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
        }

        private void ShowFirstView()
        {
            firstViewModel.SelectedMenu = firstViewMenu;
            mainViewModel.SelectedView = firstView;
            mainWindow.Show();
        }

        private void Begin()
        {
            mainWindow.Closing += OnShutdown;
            oldViews = new List<UserControl>();
            oldViews.Add(firstView);
            menuViewModel.MenuVisible = true;
            
        }

        private void OnShutdown(object sender, CancelEventArgs e)
        {
 	        //core.SaveProperties(someUser);
        }

        private void DisableControls(bool b)
        {
            mainViewModel.CursorState = 
                b ? Cursors.Wait : Cursors.Arrow;
            menuViewModel.ControlsEnabled = !b;
        }

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
}
