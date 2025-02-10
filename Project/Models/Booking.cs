using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Booking
    {
        // a booking ID
        [Key]
        public int Bid { get; set; }
        public virtual Customer Customer { get; set; }
        public int CustomerId { get; set; }
        // a booking date
        public DateTime BookingDate { get; set; }
        // a payment method
        public required string PaymentMethod { get; set; }
        // a booking status
        public required string Status { get; set; }
        // many to many relationship between booking and package
        public ICollection<Package>? Packages { get; set; }
    }
    public class BookingDto
    {
        public int Bid { get; set; }

        public  string? CustomerName { get; set; }
        public int CustomerId { get; set; }

        public DateTime BookingDate { get; set; }

        public required string PaymentMethod { get; set; }

        public required string Status { get; set; }
    }
}
