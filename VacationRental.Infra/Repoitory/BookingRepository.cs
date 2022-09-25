using VacationRental.Domain.Models;
using VacationRental.Domain.VacationRental.Interfaces.Repositories;
using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Infra.Repoitory
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DatabaseInMemoryContext _context;
        public BookingRepository(DatabaseInMemoryContext context)
        {
            _context = context;
        }

        public async Task<BookingViewModel> Get(int bookingId)
        {
            var result = _context.Booking.First(x => x.Id == bookingId);
            return result;
        }

        public async Task<List<BookingViewModel>> Get()
        {
            return _context.Booking.ToList();
        }

        public async Task<ResourceIdViewModel> Post(BookingViewModel bookingModel)
        {
            _context.Booking.Add(bookingModel);
            _context.SaveChanges();

            var key = new ResourceIdViewModel { Id = bookingModel.Id };

            return (key);
        }
    }
}
