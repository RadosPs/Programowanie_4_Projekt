using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;
using Biblioteka.Persistance;
using Biblioteka.Models;

namespace Biblioteka.ViewModels
{
    public class CreateBookViewModel : ViewModelBase
    {
        private string _bookName;
        private string _bookCategory;
        private string _bookAuthor;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly BookService _bookService;
        private readonly Book _editingBook;

        public string BookName
        {
            get => _bookName;
            set => this.RaiseAndSetIfChanged(ref _bookName, value);
        }

        public string BookCategory
        {
            get => _bookCategory;
            set => this.RaiseAndSetIfChanged(ref _bookCategory, value);
        }

        public string BookAuthor
        {
            get => _bookAuthor;
            set => this.RaiseAndSetIfChanged(ref _bookAuthor, value);
        }

        public ReactiveCommand<Unit, Unit> CreateBookCommand { get; }
        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        public CreateBookViewModel(MainWindowViewModel mainWindowViewModel, IAppDbContext appDbContext)
            : this(mainWindowViewModel, appDbContext, null) { }

        public CreateBookViewModel(MainWindowViewModel mainWindowViewModel, IAppDbContext appDbContext, Book book)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _bookService = new BookService(appDbContext);
            _editingBook = book;

            if (book != null)
            {
                BookName = book.Title;
                BookCategory = book.Category;
                BookAuthor = book.Author;
            }

            CreateBookCommand = ReactiveCommand.CreateFromTask(HandleCreateCommand);
            BackCommand = ReactiveCommand.Create(NavigateBack);
        }

        private async Task HandleCreateCommand()
        {
            if (_editingBook == null)
            {
                await _bookService.CreateBook(BookName, BookAuthor, BookCategory);
            }
            else
            {
                await _bookService.UpdateBook(_editingBook.Id, BookName, BookAuthor, BookCategory);
            }
            _mainWindowViewModel.ShowBooks();
        }

        private void NavigateBack()
        {
            _mainWindowViewModel.ShowBooks();
        }
    }
}
