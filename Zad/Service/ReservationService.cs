using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Biblioteka.Models;
using Biblioteka.Persistance;
using System.Linq;
using System.Data.Entity.Migrations;

public class ReservationService
{
    private readonly IAppDbContext _appDbContext;

    public ReservationService(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IEnumerable<Reservation> GetReservations()
    {
        return _appDbContext.Reservations;
    }


    public async Task<Reservation> CreateReservation(string title, string author, string borrowerName, string borrowerSurname, string phone, DateTime from, DateTime to)
    {
        var reservation = new Reservation()
        {
            Title = title,
            Author = author,
            BorrowerName = borrowerName,
            BorrowerSurname = borrowerSurname,
            From = from,
            To = to,
            Phone = phone,
            CreatedDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };

        _appDbContext.Reservations.AddOrUpdate(reservation);
        await _appDbContext.SaveChangesAsync();
        return reservation;
    }

    public async Task<Reservation> UpdateReservation(int id, string title, string author, string borrowerName, string borrowerSurname, string phone, DateTime from, DateTime to)
    {
        var reservation = _appDbContext.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation != null)
        {
            reservation.Title = title;
            reservation.Author = author;
            reservation.BorrowerName = borrowerName;
            reservation.BorrowerSurname = borrowerSurname;
            reservation.From = from;
            reservation.To = to;
            reservation.Phone = phone;
            reservation.LastUpdateDate = DateTime.Now;
            _appDbContext.Reservations.AddOrUpdate(reservation);
            await _appDbContext.SaveChangesAsync();
        }
        return reservation;
    }

    public async Task<bool> DeleteReservation(int id)
    {
        var reservationToRemove = _appDbContext.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservationToRemove != null)
        {
            _appDbContext.Reservations.Remove(reservationToRemove);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
