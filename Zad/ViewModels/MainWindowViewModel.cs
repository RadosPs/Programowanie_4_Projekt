using ReactiveUI;
using Biblioteka.Persistance;
using Biblioteka.Models;

namespace Biblioteka.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private object _currentView;
        private readonly IAppDbContext _appDbContext;

        public object CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        public MainWindowViewModel()
        {
            _appDbContext = new AppDbContext("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False");
            CurrentView = new MenuViewModel(this, _appDbContext);
        }

        public void ShowMenu()
        {
            CurrentView = new MenuViewModel(this, _appDbContext);
        }

        public void ShowBooks()
        {
            CurrentView = new BooksViewModel(this, _appDbContext);
        }

        public void ShowCreateBook()
        {
            CurrentView = new CreateBookViewModel(this, _appDbContext);
        }

        public void ShowCreateBook(Book book)
        {
            CurrentView = new CreateBookViewModel(this, _appDbContext, book);
        }

        public void ShowReservations()
        {
            CurrentView = new ReservationViewModel(this, _appDbContext);
        }

        public void ShowCreateReservation()
        {
            CurrentView = new CreateReservationViewModel(this, _appDbContext);
        }
        public void ShowCategories()
        {
            CurrentView = new CategoriesViewModel(this, _appDbContext);
        }


        public void ShowCreateReservation(Reservation reservation)
        {
            CurrentView = new CreateReservationViewModel(this, _appDbContext, reservation);
        }
    }
}
