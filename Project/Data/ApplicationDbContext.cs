using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //create customer table from the model
        public DbSet<Customer> Customers { get; set; }

        // create package table from the model
        public DbSet<Package> Packages { get; set; }

        // create booking table from the model
        public DbSet<Booking> Bookings { get; set; }

        // create payment table from the model
        public DbSet<Payment> Payments { get; set; }
    }
}
