namespace VacationRental.Domain.VacationRental.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Nights { get; set; }
        public int Unit { get; set; }
    }
}
