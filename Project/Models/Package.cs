using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Package
    {
        
        // a package id
        [Key]
        public int Pid { get; set; }
        // a package name
        public required string Name { get; set; }
        // a package description
        public required string Description { get; set; }
        // a package price
        public int Price { get; set; }
        // many to many relationship between booking and package
        public ICollection<Booking>? Bookings { get; set; }

    }
    public class PackageDto
    {
        public int Pid { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int Price { get; set; }
        ////wanted to embedded a video but.....
        //public string Video { get; set; }  // Stores YouTube/Vimeo embed URL
    }
}
