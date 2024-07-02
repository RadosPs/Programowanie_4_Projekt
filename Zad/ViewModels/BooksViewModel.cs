using DynamicData;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Biblioteka.Models;
using Biblioteka.Persistance;

namespace Biblioteka.ViewModels
{
    public class BooksViewModel : ViewModelBase
    {
        private ObservableCollection<Book> _books;
        private Book _selectedBook;
        private BookService _bookService;
        private MainWindowViewModel _mainWindowViewModel;
        private bool _sortByCategory;

        public ObservableCollection<Book> Books
        {
            get => _books;
            set => this.RaiseAndSetIfChanged(ref _books, value);
        }

        public Book SelectedBook
        {
            get => _selectedBook;
            set => this.RaiseAndSetIfChanged(ref _selectedBook, value);
        }

        public bool SortByCategory
        {
            get => _sortByCategory;
            set
            {
                if (_sortByCategory != value)
                {
                    _sortByCategory = value;
                    RefreshBooks();
                }
            }
        }

        public ReactiveCommand<Unit, Unit> CreateBookCommand { get; }
        public ReactiveCommand<Unit, Unit> RemoveBookCommand { get; }
        public ReactiveCommand<Unit, Unit> EditBookCommand { get; }
        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        public BooksViewModel(MainWindowViewModel mainWindowViewModel, IAppDbContext appDbContext)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _bookService = new BookService(appDbContext);

            CreateBookCommand = ReactiveCommand.Create(CreateBook);
            RemoveBookCommand = ReactiveCommand.Create(RemoveBook);
            EditBookCommand = ReactiveCommand.Create(EditBook);
            BackCommand = ReactiveCommand.Create(Back);

            Books = new ObservableCollection<Book>();
            RefreshBooks();
        }

        private void CreateBook()
        {
            _mainWindowViewModel.ShowCreateBook();
        }

        private async void RemoveBook()
        {
            if (_selectedBook != null)
            {
                await _bookService.DeleteBook(_selectedBook.Id);
                RefreshBooks();
            }
        }

        private void EditBook()
        {
            if (_selectedBook != null)
            {
                _mainWindowViewModel.ShowCreateBook(_selectedBook);
            }
        }

        private void Back()
        {
            _mainWindowViewModel.ShowMenu();
        }

        public void RefreshBooks()
        {
            var books = _bookService.GetBooks();

            if (SortByCategory)
            {
                Books = new ObservableCollection<Book>(books.OrderBy(b => b.Category));
            }
            else
            {
                Books = new ObservableCollection<Book>(books);
            }
        }
    }
}
