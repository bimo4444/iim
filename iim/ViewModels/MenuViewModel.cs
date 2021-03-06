﻿using MVPLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace iim.ViewModels
{
    public class MenuViewModel : VMBase
    {
        public ICommand ShowPrimaryView { get; set; }
        public ICommand ShowMovementView { get; set; }
        public ICommand ExcelReportMov { get; set; }
        public ICommand Refresh { get; set; }
        public ICommand ExcelReport { get; set; }
        public ICommand PreviousControl { get; set; }
        public ICommand ResetMaxDate { get; set; }
        public ICommand ResetMinDate { get; set; }
        public ICommand ShowMenu { get; set; }
        public ICommand UncheckAllButton { get; set; }
        public ICommand SelectGroup { get; set; }

        private UserControl _selectedFrame;
        public UserControl SelectedFrame
        {
            get { return _selectedFrame; }
            set
            {
                _selectedFrame = value;
                OnPropertyChanged("SelectedFrame");
            }
        }

        private int _totalDays;
        public int TotalDays
        {
            get { return _totalDays; }
            set
            {
                _totalDays = value;
                OnPropertyChanged("TotalDays");
            }
        }

        private bool _gridButtonsVisibility;
        public bool GridButtonsVisibility
        {
            get { return _gridButtonsVisibility; }
            set
            {
                _gridButtonsVisibility = value;
                OnPropertyChanged("GridButtonsVisibility");
            }
        }
        private bool _movButtonsVisibility;
        public bool MovButtonsVisibility
        {
            get { return _movButtonsVisibility; }
            set
            {
                _movButtonsVisibility = value;
                OnPropertyChanged("MovButtonsVisibility");
            }
        }
        private bool _movButtonEnabled;
        public bool MovButtonEnabled
        {
            get { return _movButtonEnabled; }
            set
            {
                _movButtonEnabled = value;
                OnPropertyChanged("MovButtonEnabled");
            }
        }
        //MENU VISIBILITY
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

        private bool _menuNotVisible;
        public bool MenuNotVisible
        {
            get { return _menuNotVisible; }
            set
            {
                _menuNotVisible = value;
                OnPropertyChanged("MenuNotVisible");
            }
        }

        //DATES
        private DateTime _minDateTime;
        public DateTime MinDateTime
        {
            get { return _minDateTime; }
            set
            {
                _minDateTime = value;
                OnPropertyChanged("MinDateTime");
            }
        }

        private DateTime _currentMinDateTime;
        public DateTime CurrentMinDateTime
        {
            get { return _currentMinDateTime; }
            set
            {
                _currentMinDateTime = value;
                OnPropertyChanged("CurrentMinDateTime");
            }
        }

        private bool _buttonsEnabled;
        public bool ButtonsEnabled
        {
            get { return _buttonsEnabled; }
            set
            {

                _buttonsEnabled = value;
                OnPropertyChanged("ButtonsEnabled");
            }
        }
        private DateTime _maxDateTime;
        public DateTime MaxDateTime
        {
            get { return _maxDateTime; }
            set
            {
                _maxDateTime = value;
                OnPropertyChanged("MaxDateTime");
            }
        }
        private DateTime _currentMaxDateTime;
        public DateTime CurrentMaxDateTime
        {
            get { return _currentMaxDateTime; }
            set
            {
                _currentMaxDateTime = value;
                OnPropertyChanged("CurrentMaxDateTime");
            }
        }
        private bool _task;
        public bool Task
        {
            get { return _task; }
            set
            {
                _task = value;
                OnPropertyChanged("Task");
            }
        }
        private bool _order;
        public bool Order
        {
            get { return _order; }
            set
            {
                _order = value;
                OnPropertyChanged("Order");
            }
        }
        private bool _party;
        public bool Party
        {
            get { return _party; }
            set
            {
                _party = value;
                OnPropertyChanged("Party");
            }
        }
        
        private int _storesCounter;
        public int StoresCounter
        {
            get { return _storesCounter; }
            set
            {
                _storesCounter = value;
                OnPropertyChanged("StoresCounter");
            }
        }
        private bool _minus;
        public bool Minus
        {
            get { return _minus; }
            set
            {
                _minus = value;
                OnPropertyChanged("Minus");
            }
        }
        private string _time;
        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }
        private string _movementDetails;
        public string MovementDetails
        {
            get { return _movementDetails; }
            set
            {
                _movementDetails = value;
                OnPropertyChanged("MovementDetails");
            }
        }
        private bool _zeros;
        public bool Zeros
        {
            get { return _zeros; }
            set
            {
                _zeros = value;
                OnPropertyChanged("Zeros");
            }
        }
        private bool _stat;
        public bool Stat
        {
            get { return _stat; }
            set
            {
                _stat = value;
                OnPropertyChanged("Stat");
            }
        }
        private bool _controlsEnabled;
        public bool ControlsEnabled
        {
            get { return _controlsEnabled; }
            set
            {
                _controlsEnabled = value;
                OnPropertyChanged("ControlsEnabled");
            }
        }
        private bool _movVisibility;
        public bool MovVisibility
        {
            get { return _movVisibility; }
            set
            {
                _movVisibility = value;
                OnPropertyChanged("MovVisibility");
            }
        }
    }
}
