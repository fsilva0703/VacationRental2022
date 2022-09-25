using VacationRental.Domain.Models;
using VacationRental.Domain.VacationRental.Models;

namespace VacationRental.Domain.VacationRental.Interfaces
{
    public interface IBookingService
    {
        Task<BookingViewModel> Get(int bookingId);
        Task<List<BookingViewModel>> Get();
        Task<ResourceIdViewModel> Post(BookingBindingModel model);
    }
}
