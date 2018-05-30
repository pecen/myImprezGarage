﻿//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.PerformChecks.ViewModels
{
    using System.Linq;
    using System.Collections.ObjectModel;
    using ImprezGarage.Infrastructure;
    using ImprezGarage.Infrastructure.Model;
    using ImprezGarage.Infrastructure.ViewModels;
    using Prism.Events;
    using Prism.Mvvm;
    using Microsoft.Practices.Unity;

    public class ChecksPerformedViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDataService _dataService;
        private readonly IUnityContainer _container;

        private VehicleViewModel _selectedVehicle;
        public VehicleViewModel SelectedVehicle
        {
            get => _selectedVehicle;
            set => SetProperty(ref _selectedVehicle, value);
        }

        private ObservableCollection<MaintenanceCheckViewModel> _maintainanceChecks;
        public ObservableCollection<MaintenanceCheckViewModel> MaintainanceChecks
        {
            get => _maintainanceChecks;
            set => SetProperty(ref _maintainanceChecks, value);
        }

        public ChecksPerformedViewModel(IEventAggregator eventAggregator, IDataService dataService, IUnityContainer container)
        {
            _eventAggregator = eventAggregator;
            _dataService = dataService;
            _container = container;

            _eventAggregator.GetEvent<Events.SelectVehicleEvent>().Subscribe(OnSelectedVehicleChanged);
        }

        private void OnSelectedVehicleChanged(VehicleViewModel vehicleViewModel)
        {
            SelectedVehicle = vehicleViewModel;
            GetSelectedVehicleMaintainanceChecks();
        }

        private void GetSelectedVehicleMaintainanceChecks()
        {
            if (SelectedVehicle == null)
            {
                MaintainanceChecks = null;
            }
            else
            {
                _dataService.GetMaintenanceChecksForVehicleByVehicleId((performedCheck, error) =>
                {
                    if (error != null)
                    {

                    }

                    if (performedCheck == null)
                    {
                        MaintainanceChecks = null;
                        return;
                    }

                    MaintainanceChecks = new ObservableCollection<MaintenanceCheckViewModel>();

                    foreach (var check in performedCheck.OrderByDescending(o => o.DatePerformed))
                    {
                        var checkVm = _container.Resolve<MaintenanceCheckViewModel>();
                        checkVm.LoadInstance(check, SelectedVehicle);
                        MaintainanceChecks.Add(checkVm);
                    }
                }, SelectedVehicle.Vehicle.Id);
            }
        }
    }
}   //ImprezGarage.Modules.PerformChecks.ViewModels namespace 