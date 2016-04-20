using DialogBase;
using iim.ViewModels;
using iim.Views;
using Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iim
{
    class Presenter
    {
        Core core = new Core();

        MainWindow mainWindow = new MainWindow();
        MainViewModel mainWiewModel = new MainViewModel();

        FirstView firstView = new FirstView();
        FirstViewModel firstViewModel = new FirstViewModel();

        FirstViewMenu firstViewMenu = new FirstViewMenu();
        MenuViewModel menuViewModel = new MenuViewModel();

        MainMenu mainMenu;

        PrimaryView primaryView;
        PrimaryViewModel primaryViewModel;

        MovementView movementView;
        MovementViewModel movementViewModel;

        ConnectionErrorView connectionErrorView;
        Dialog dialog;

        public Presenter()
        {
            SetViewModels();
            mainWindow.Show();
            Begin();
            InitializeOtherObjects();
            SetOtherViewModels();


        }

        private void Begin()
        {
            throw new NotImplementedException();
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
        }

        private void SetViewModels()
        {
            mainWindow.DataContext = mainWiewModel;
            firstView.DataContext = firstViewModel;
            firstViewMenu.DataContext = menuViewModel;
        }
    }
}
