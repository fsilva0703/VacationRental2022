using VacationRental.Domain.Extensions.Common;
using VacationRental.Domain.Models;
using VacationRental.Domain.VacationRental.Interfaces;
using VacationRental.Domain.VacationRental.Interfaces.Repositories;
using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Domain.VacationRental.Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        public BookingService(IBookingRepository paramBooking)
        {
            _bookingRepository = paramBooking;
        }

        public async Task<BookingViewModel> Get(int bookingId)
        {
            try
            {
                return await _bookingRepository.Get(bookingId);
            }
            catch(Exception)
            {
                throw new NotFoundException("Booking not found");
            }
        }

        public async Task<List<BookingViewModel>> Get()
        {
            try
            {
                return await _bookingRepository.Get();
            }
            catch (Exception)
            {
                throw new NotFoundException("No booking was found");
            }
        }

        public async Task<ResourceIdViewModel> Post(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ConflictException("Nights must be in minimum one.");
            //if (!_rentals.ContainsKey(model.RentalId))
            //    throw new NotFoundException("Rental not found");

            var bookings = await _bookingRepository.Get();

            for (var i = 0; i < model.Nights; i++)
            {
                var count = 0;
                
                foreach (var booking in bookings)
                {
                    if (booking.RentalId == model.RentalId
                        && (booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights) > model.Start.Date)
                        || (booking.Start < model.Start.AddDays(model.Nights) && booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights))
                        || (booking.Start > model.Start && booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
                    {
                        count++;
                    }
                }
                //if (count >= _rentals[model.RentalId].Units)
                //    throw new ConflictException("Not available");
            }

            var NewBooking =  new BookingViewModel
            {
                Id = bookings.Count + 1,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date
            };

            return await _bookingRepository.Post(NewBooking);
        }
    }
}
