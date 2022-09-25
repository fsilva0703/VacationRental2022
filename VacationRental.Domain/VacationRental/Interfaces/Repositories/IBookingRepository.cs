using VacationRental.Domain.Models;
using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Domain.VacationRental.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<BookingViewModel> Get(int bookingId);
        Task<List<BookingViewModel>> Get();
        Task<ResourceIdViewModel> Post(BookingViewModel model);
    }
}
