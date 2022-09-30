using System.Runtime.Serialization;
using VacationRental.Domain.Extensions.Common;
using VacationRental.Domain.VacationRental.Extensions.Enum;
using VacationRental.Domain.VacationRental.Interfaces;
using VacationRental.Domain.VacationRental.Interfaces.Repositories;
using VacationRental.Domain.VacationRental.Models;
using VacationRental.Domain.VacationRental.Utils;
using static VacationRental.Domain.VacationRental.Models.CalendarDateViewModel;

namespace VacationRental.Domain.VacationRental.Service
{
    public class RentalsService : IRentalsService
    {
        private readonly IRentalsRepository _rentalsRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IDictionary<DateTime, int> _cacheBooking;
        public RentalsService(IRentalsRepository paramRentals, IBookingRepository paramBookingRepository, IDictionary<DateTime, int> paramCacheBooking)
        {
            _rentalsRepository = paramRentals;
            _bookingRepository = paramBookingRepository;
            _cacheBooking = paramCacheBooking;
        }

        public async Task<RentalViewModel> Get(int rentalId)
        {
            try
            {
                return await _rentalsRepository.Get(rentalId);
            }
            catch (Exception)
            {
                throw new NotFoundException(EnumExceptions.RentalNotFound.GetAttributeOfType<EnumMemberAttribute>().Value);
            }
        }

        public async Task<List<RentalViewModel>> Get()
        {
            try
            {
                return await _rentalsRepository.Get();
            }
            catch (Exception)
            {
                throw new NotFoundException(EnumExceptions.RentalNotFound.GetAttributeOfType<EnumMemberAttribute>().Value);
            }
        }

        public async Task<ResourceIdViewModel> Post(RentalBindingModel model)
        {

            var rentals = await _rentalsRepository.Get();

            var NewRental = new RentalViewModel
            {
                Id = rentals.Count + 1,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            };

            return await _rentalsRepository.Post(NewRental);

        }

        public async Task<ResourceIdViewModel> Put(int rentalId, RentalBindingModel model)
        {
            var rentals = await Get(rentalId);

            var bookings = await _bookingRepository.GetByRentalId(rentalId);

            List<int> listUnit = new();

            foreach (var booking in bookings)
            {
                _cacheBooking.Add(booking.Start.AddDays(model.PreparationTimeInDays + booking.Nights), booking.Unit);

                bool exists = _cacheBooking.Any(x => x.Value == booking.Unit && x.Key == booking.Start);
                    
                if(exists)
                    throw new ConflictException(EnumExceptions.AvailableConflict.GetAttributeOfType<EnumMemberAttribute>().Value);

                Units unit = new() { Unit = booking.Unit };

                if(!listUnit.Contains(unit.Unit)) 
                    listUnit.Add(unit.Unit);
            }

            _cacheBooking.Clear();

            if(listUnit.Count > model.Units || (model.Units < listUnit.Count && listUnit.Count > 0))
                throw new ConflictException(EnumExceptions.AvailableConflict.GetAttributeOfType<EnumMemberAttribute>().Value);

            bookings.ToList().ForEach(x => x.Start = x.Start.AddDays(model.PreparationTimeInDays));

            foreach (var booking in bookings)
            {
                await _bookingRepository.Put(booking);
            }

            return await _rentalsRepository.Put(rentalId, model);
        }
    }
}
