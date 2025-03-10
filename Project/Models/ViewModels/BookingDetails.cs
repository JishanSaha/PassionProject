namespace Project.Models.ViewModels
{
    public class BookingDetails
    {
        // A Booking page must have a booking
        // FindBooing(Bid)
        public required BookingDto Booking { get; set; }

        // A Booking may have packages associated to it
        // ListPackagesForBooking(productid)
        public IEnumerable<PackageDto>? BookingPackages { get; set; }

        // All packages
        // ListPackages()
        public IEnumerable<PackageDto>? AllPackages { get; set; }

        
    }
}
