using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Customer
    {
        // a customer id
        [Key]
        public int Cid { get; set; }
        // a customer name
        public required string Name { get; set; }
        // a custmer email
        public required string Email { get; set; }
        // a customer phone nuber
        public int Phone { get; set; }

    }
    public class CustomerDto
    {
        public int Cid { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public int Phone { get; set; }
    }
}

