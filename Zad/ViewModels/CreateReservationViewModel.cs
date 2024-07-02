using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;
using Biblioteka.Models;
using Biblioteka.Persistance;

namespace Biblioteka.ViewModels
{
    public class CreateReservationViewModel : ViewModelBase
    {
        private string _title;
        private string _author;
        private string _borrowerName;
        private string _borrowerSurname;
        private string _from;
        private string _to;
        private string _phone;
        private string _fromErrorMessage;
        private string _toErrorMessage;
        private string _phoneErrorMessage;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly ReservationService _reservationService;
        private readonly Reservation _editingReservation;

        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        public string Author
        {
            get => _author;
            set => this.RaiseAndSetIfChanged(ref _author, value);
        }

        public string BorrowerName
        {
            get => _borrowerName;
            set => this.RaiseAndSetIfChanged(ref _borrowerName, value);
        }

        public string BorrowerSurname
        {
            get => _borrowerSurname;
            set => this.RaiseAndSetIfChanged(ref _borrowerSurname, value);
        }

        public string From
        {
            get => _from;
            set
            {
                this.RaiseAndSetIfChanged(ref _from, value);
                ValidateFrom();
            }
        }

        public string To
        {
            get => _to;
            set
            {
                this.RaiseAndSetIfChanged(ref _to, value);
                ValidateTo();
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                this.RaiseAndSetIfChanged(ref _phone, value);
                ValidatePhone();
            }
        }

        public string FromErrorMessage
        {
            get => _fromErrorMessage;
            set => this.RaiseAndSetIfChanged(ref _fromErrorMessage, value);
        }

        public string ToErrorMessage
        {
            get => _toErrorMessage;
            set => this.RaiseAndSetIfChanged(ref _toErrorMessage, value);
        }

        public string PhoneErrorMessage
        {
            get => _phoneErrorMessage;
            set => this.RaiseAndSetIfChanged(ref _phoneErrorMessage, value);
        }

        public ReactiveCommand<Unit, Unit> CreateReservationCommand { get; }
        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        public CreateReservationViewModel(MainWindowViewModel mainWindowViewModel, IAppDbContext appDbContext, Reservation reservation = null)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _reservationService = new ReservationService(appDbContext);
            _editingReservation = reservation;

            if (_editingReservation != null)
            {
                Title = _editingReservation.Title;
                Author = _editingReservation.Author;
                BorrowerName = _editingReservation.BorrowerName;
                BorrowerSurname = _editingReservation.BorrowerSurname;
                From = _editingReservation.From?.ToString("yyyy-MM-dd");
                To = _editingReservation.To?.ToString("yyyy-MM-dd");
                Phone = _editingReservation.Phone;
            }

            CreateReservationCommand = ReactiveCommand.CreateFromTask(HandleCreateCommand);
            BackCommand = ReactiveCommand.Create(NavigateBack);
        }

        private async Task HandleCreateCommand()
        {
            if (!ValidateInput())
                return;

            if (!DateTime.TryParse(From, out DateTime fromDate) || !DateTime.TryParse(To, out DateTime toDate))
            {
                FromErrorMessage = "Invalid date format";
                ToErrorMessage = "Invalid date format";
                return;
            }

            if (fromDate >= toDate)
            {
                FromErrorMessage = "From date must be before To date";
                ToErrorMessage = "To date must be after From date";
                return;
            }

            if (_editingReservation == null)
            {
                await _reservationService.CreateReservation(Title, Author, BorrowerName, BorrowerSurname, Phone, fromDate, toDate);
            }
            else
            {
                await _reservationService.UpdateReservation(_editingReservation.Id, Title, Author, BorrowerName, BorrowerSurname, Phone, fromDate, toDate);
            }

            _mainWindowViewModel.ShowReservations();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Author) ||
                string.IsNullOrWhiteSpace(BorrowerName) || string.IsNullOrWhiteSpace(BorrowerSurname) ||
                string.IsNullOrWhiteSpace(From) || string.IsNullOrWhiteSpace(To) || string.IsNullOrWhiteSpace(Phone))
            {
                return false;
            }

            return true;
        }

        private void ValidateFrom()
        {
            if (!DateTime.TryParse(From, out DateTime fromDate))
            {
                FromErrorMessage = "Invalid date format";
            }
            else
            {
                FromErrorMessage = null;
            }
        }

        private void ValidateTo()
        {
            if (!DateTime.TryParse(To, out DateTime toDate))
            {
                ToErrorMessage = "Invalid date format";
            }
            else
            {
                ToErrorMessage = null;
            }
        }

        private void ValidatePhone()
        {
            if (string.IsNullOrWhiteSpace(Phone))
            {
                PhoneErrorMessage = "Phone number is required";
            }
            else
            {
                PhoneErrorMessage = null;
            }
        }

        private void NavigateBack()
        {
            _mainWindowViewModel.ShowReservations();
        }
    }
}
