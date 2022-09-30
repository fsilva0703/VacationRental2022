namespace VacationRental.Domain.VacationRental.Models
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }
        public List<Units> PreparationTimes { get; set; }

        public class Units
        {
            public int Unit { get; set; }
        }
    }
}
