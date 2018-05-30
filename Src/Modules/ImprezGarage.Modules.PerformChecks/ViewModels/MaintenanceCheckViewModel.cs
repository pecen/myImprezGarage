﻿//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks.ViewModels
{
    using ImprezGarage.Infrastructure;
    using ImprezGarage.Infrastructure.Dialogs;
    using ImprezGarage.Infrastructure.Model;
    using ImprezGarage.Infrastructure.ViewModels;
    using ImprezGarage.Modules.PerformChecks.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Windows.Input;

    public class MaintenanceCheckViewModel : BindableBase
    {
        private const string DELETE_MAINTENANCE_CHECK = "Are you sure you wish to delete this maintenance check?";
        private const string DELETED_SUCCESSFULLY = "Maintenance check deleted successfuly!";

        private readonly IDataService _dataService;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        
        private VehicleViewModel _selectedVehicle;
        public VehicleViewModel SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                _selectedVehicle = value;
                RaisePropertyChanged("SelectedVehicle");
            }
        }

        private MaintenanceCheckType _maintenanceCheckType;
        public MaintenanceCheckType MaintenanceCheckType
        {
            get => _maintenanceCheckType;
            set { SetProperty(ref _maintenanceCheckType, value); }
        }
        
        private DateTime _datePerformed;
        public DateTime DatePerformed
        {
            get => _datePerformed;
            set { SetProperty(ref _datePerformed, value); }
        }

        public DelegateCommand<MouseButtonEventArgs> OpenMaintenanceCheck { get; set; }
        public DelegateCommand EditMaintenanceCheckCommand { get; set; }
        public DelegateCommand DeleteMaintenanceCheckCommand { get; set; }
        
        public MaintenanceCheckViewModel(IDataService dataService, IRegionManager regionManager, IDialogService dialogService,
            IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _regionManager = regionManager;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            OpenMaintenanceCheck = new DelegateCommand<MouseButtonEventArgs>(OpenMaintenanceCheckExecute);
            EditMaintenanceCheckCommand = new DelegateCommand(EditMaintenanceCheckExecute);
            DeleteMaintenanceCheckCommand = new DelegateCommand(DeleteMaintenanceCheckExecute);
        }

        #region Methods
        #region Command Handlers
        private void DeleteMaintenanceCheckExecute()
        {
            if (!_dialogService.Confirm(DELETE_MAINTENANCE_CHECK))
                return;

            _dataService.DeleteMaintenanceCheck((error) => 
            {
                if(error != null)
                {

                }

                _dialogService.Alert(DELETED_SUCCESSFULLY);
                _eventAggregator.GetEvent<Events.SelectVehicleEvent>().Publish(SelectedVehicle);
            }, Id);
        }

        private void EditMaintenanceCheckExecute()
        {
            EditMaintenceCheck();
        }

        private void OpenMaintenanceCheckExecute(MouseButtonEventArgs obj)
        {
            if (obj.ClickCount == 2)
            {
                EditMaintenceCheck();
            }
        }
        #endregion

        private void EditMaintenceCheck()
        {
            var parameters = new NavigationParameters
            {
                { "MaintenanceCheckId", Id },
                { "SelectedVehicleId", SelectedVehicle.Vehicle.Id},
                { "IsEditMode", true }
            };

            _regionManager.RequestNavigate(RegionNames.ChecksRegion, typeof(PerformNewCheck).FullName + parameters);
        }

        internal void LoadInstance(MaintenanceCheck check, VehicleViewModel selectedVehicle)
        {
            Id = check.Id;

            SelectedVehicle = selectedVehicle;

            DatePerformed = new DateTime(check.DatePerformed.Value.Year, check.DatePerformed.Value.Month, check.DatePerformed.Value.Day);

            _dataService.GetMaintenanceCheckTypeById((type, error) =>
            {
                if (error != null)
                {

                }

                if (type == null)
                    return;

                MaintenanceCheckType = type;
            }, Convert.ToInt32(check.MaintenanceCheckType));
        }
        #endregion
    }
}   //ImprezGarage.Modules.PerformChecks.ViewModels namespace 