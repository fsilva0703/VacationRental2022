using Microsoft.EntityFrameworkCore;
using VacationRental.Domain.Models;

namespace VacationRental.Infra
{
    public class DatabaseInMemoryContext : DbContext
    {
        public DatabaseInMemoryContext(DbContextOptions<DatabaseInMemoryContext> options)
            : base(options)
        {
        }
        public DbSet<BookingViewModel>? Booking { get; set; }
    }
}
