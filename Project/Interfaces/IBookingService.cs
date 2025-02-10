using Project.Models;

namespace Project.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> ListBookings();


        Task<BookingDto?> FindBooking(int id);


        Task<ServiceResponse> UpdateBooking(BookingDto bookingDto);

        Task<ServiceResponse> AddBooking(BookingDto bookingDto);

        Task<ServiceResponse> DeleteBooking(int id);

        Task<IEnumerable<BookingDto>> ListBookingsForCustomer(int id);

        
    }
}
