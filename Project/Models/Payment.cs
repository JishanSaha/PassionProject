using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Payment
    {

        // a payment id
        [Key]
        public int Pid { get; set; }
        // a order id
        public virtual Booking Booking { get; set; }
        public int BookingId { get; set; }
        // a total amount
        public decimal TotalAmount { get; set; }
        // a payment date
        public DateTime PaymentDate { get; set; }
        // a payment staus
        public required string Status { get; set; }
    }
    public class PaymentDto
    {
        public int Pid { get; set; }
        public required string CustomerName { get; set; }
        public int Bid { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime PaymentDate { get; set; }
        public required string PaymentMethod { get; set; }

        public required string Status { get; set; }
    }
}
