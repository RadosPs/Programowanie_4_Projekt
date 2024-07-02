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
    public class CategoriesViewModel : ViewModelBase
    {
        private ObservableCollection<CategoryCount> _categories;
        private readonly IAppDbContext _appDbContext;
        private readonly BookService _bookService;
        private MainWindowViewModel _mainWindowViewModel;

        public ObservableCollection<CategoryCount> Categories
        {
            get => _categories;
            set => this.RaiseAndSetIfChanged(ref _categories, value);
        }

        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        public CategoriesViewModel(MainWindowViewModel mainWindowViewModel, IAppDbContext appDbContext)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _appDbContext = appDbContext;
            _bookService = new BookService(appDbContext);

            Categories = new ObservableCollection<CategoryCount>();
            RefreshCategories();

            BackCommand = ReactiveCommand.Create(Back);
        }

        private void RefreshCategories()
        {
            var books = _bookService.GetBooks();

            var categoryCounts = books.GroupBy(b => b.Category)
                                      .Select(g => new CategoryCount { Category = g.Key, Count = g.Count() })
                                      .OrderBy(c => c.Category);

            Categories.Clear();
            foreach (var categoryCount in categoryCounts)
            {
                Categories.Add(categoryCount);
            }
        }

        private void Back()
        {
            _mainWindowViewModel.ShowMenu();
        }
    }

    public class CategoryCount
    {
        public string Category { get; set; }
        public int Count { get; set; }
    }
}
