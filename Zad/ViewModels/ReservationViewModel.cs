using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Biblioteka.Models;
using Biblioteka.Persistance;

namespace Biblioteka.ViewModels
{
    public class ReservationViewModel : ViewModelBase
    {
        private ObservableCollection<Reservation> _reservations;
        private Reservation _selectedReservation;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly ReservationService _reservationService;

        public ObservableCollection<Reservation> Reservations
        {
            get => _reservations;
            set => this.RaiseAndSetIfChanged(ref _reservations, value);
        }

        public Reservation SelectedReservation
        {
            get => _selectedReservation;
            set => this.RaiseAndSetIfChanged(ref _selectedReservation, value);
        }

        public ReactiveCommand<Unit, Unit> CreateReservationCommand { get; }
        public ReactiveCommand<Unit, Unit> RemoveReservationCommand { get; }
        public ReactiveCommand<Unit, Unit> EditReservationCommand { get; }
        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        public ReservationViewModel(MainWindowViewModel mainWindowViewModel, IAppDbContext appDbContext)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _reservationService = new ReservationService(appDbContext);

            CreateReservationCommand = ReactiveCommand.Create(CreateReservation);
            RemoveReservationCommand = ReactiveCommand.Create(RemoveReservation);
            EditReservationCommand = ReactiveCommand.Create(EditReservation);
            BackCommand = ReactiveCommand.Create(Back);

            Reservations = new ObservableCollection<Reservation>();
            RefreshReservations();
        }

        private void CreateReservation()
        {
            _mainWindowViewModel.ShowCreateReservation();
        }

        private async void RemoveReservation()
        {
            if (_selectedReservation != null)
            {
                await _reservationService.DeleteReservation(_selectedReservation.Id);
                RefreshReservations();
            }
        }

        private void EditReservation()
        {
            if (_selectedReservation != null)
            {
                _mainWindowViewModel.ShowCreateReservation(_selectedReservation);
            }
        }

        private void RefreshReservations()
        {
            Reservations.Clear();
            var reservations = _reservationService.GetReservations();
            foreach (var reservation in reservations)
            {
                Reservations.Add(reservation);
            }
        }

        private void Back()
        {
            _mainWindowViewModel.ShowMenu();
        }
    }
}
