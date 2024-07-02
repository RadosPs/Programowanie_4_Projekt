using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using Biblioteka.Models;
using Biblioteka.Persistance;

namespace Biblioteka.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainWindowViewModel;
        public ReactiveCommand<Unit, Unit> BooksListCommand { get; }
        public ReactiveCommand<Unit, Unit> ReservationsListCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowCategoriesCommand { get; }

        public MenuViewModel(MainWindowViewModel mainWindowViewModel, IAppDbContext appDbContext)
        {
            _mainWindowViewModel = mainWindowViewModel;
            BooksListCommand = ReactiveCommand.Create(BooksList);
            ReservationsListCommand = ReactiveCommand.Create(ReservationsList);
            ShowCategoriesCommand = ReactiveCommand.Create(ShowCategories);
        }

        private void BooksList()
        {
            _mainWindowViewModel.ShowBooks();
        }

        private void ReservationsList()
        {
            _mainWindowViewModel.ShowReservations();
        }
        private void ShowCategories()
        {
            _mainWindowViewModel.ShowCategories();
        }
    }
}
