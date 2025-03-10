namespace Project.Models.ViewModels
{
    public class CustomerDetails
    {
        // a customer page must have a customer
        public required CustomerDto Customer { get; set; }
        public IEnumerable<BookingDto>? CustomerBookings { get; set; }

        public IEnumerable<PaymentDto>? CustomerPayments { get; set; } 
    }
}
