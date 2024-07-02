using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Biblioteka.Models;
using Biblioteka.Persistance;
using System.Data.Entity.Migrations;
using System.Linq;

public class BookService
{
    private readonly IAppDbContext _appDbContext;

    public BookService(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IEnumerable<Book> GetBooks()
    {
        return _appDbContext.Books;
    }

    public async Task<Book> CreateBook(string title, string author, string category)
    {
        var book = new Book()
        {
            Title = title,
            Author = author,
            Category = category,
            CreatedDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };

        _appDbContext.Books.AddOrUpdate(book);
        await _appDbContext.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateBook(int id, string title, string author, string category)
    {
        var book = _appDbContext.Books.FirstOrDefault(b => b.Id == id);
        if (book != null)
        {
            book.Title = title;
            book.Author = author;
            book.Category = category;
            book.LastUpdateDate = DateTime.Now;
            _appDbContext.Books.AddOrUpdate(book);
            await _appDbContext.SaveChangesAsync();
        }
        return book;
    }

    public async Task<bool> DeleteBook(int id)
    {
        var bookToRemove = _appDbContext.Books.FirstOrDefault(n => n.Id == id);
        if (bookToRemove != null)
        {
            _appDbContext.Books.Remove(bookToRemove);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
