using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteka.Models;

namespace Biblioteka.Persistance
{
    public interface IAppDbContext
    {
        DbSet<Book> Books { get; set; }
        DbSet<Reservation> Reservations { get; set; }

        Task<int> SaveChangesAsync();

    }
}
