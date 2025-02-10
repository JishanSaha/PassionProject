using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Interfaces;
using Project.Models;
using System;

namespace Project.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        // dependency injection of database context
        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookingDto>> ListBookings()
        {
            List<Booking> Bookings = await _context.Bookings
                .Include(b => b.Customer)
                .ToListAsync();
            // empty list of data transfer object BookingDto
            List<BookingDto> BookingDtos = new List<BookingDto>();
            // foreach Order Item record in database
            foreach (Booking Booking in Bookings)
            {
                // create new instance of BookingDto, add to list
                BookingDtos.Add(new BookingDto()
                {
                    Bid = Booking.Bid,
                    CustomerName = Booking.Customer.Name,
                    CustomerId = Booking.Customer.Cid,
                    BookingDate = Booking.BookingDate,
                    PaymentMethod = Booking.PaymentMethod,
                    Status = Booking.Status
                });
            }
            return BookingDtos;
        }
        public async Task<BookingDto?> FindBooking(int id)
        {
            var Booking = await _context.Bookings
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.Bid == id);

            if (Booking == null)
            {
                return null;
            }
            // create an instance of BookingDto
            BookingDto BookingDto = new BookingDto()
            {
                Bid = Booking.Bid,
                CustomerName = Booking.Customer.Name,
                CustomerId = Booking.Customer.Cid,
                BookingDate = Booking.BookingDate,
                PaymentMethod = Booking.PaymentMethod,
                Status = Booking.Status
            };
            return BookingDto;

        }
        public async Task<ServiceResponse> UpdateBooking(BookingDto BookingDto)
        {
            ServiceResponse serviceResponse = new();


            // Create instance of Booking
            Booking Booking = new Booking()
            {
                Bid = BookingDto.Bid,
                CustomerId = BookingDto.CustomerId,
                BookingDate = BookingDto.BookingDate,
                PaymentMethod = BookingDto.PaymentMethod,
                Status = BookingDto.Status
            };
            // flags that the object has changed
            _context.Entry(Booking).State = EntityState.Modified;

            try
            {
                // SQL Equivalent: Update Booking set ... where Bid={id}
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("An error occurred updating the record");

                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;

        }
        public async Task<ServiceResponse> AddBooking(BookingDto BookingDto)
        {
            ServiceResponse serviceResponse = new();


            // Create instance of Booking
            Customer? customer = await _context.Customers.FindAsync(BookingDto.CustomerId);
            // data must link to a valid entry
            if (customer == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (customer == null)
                {
                    serviceResponse.Messages.Add("Product was not found. ");
                }

                return serviceResponse;
            }
            Booking booking = new Booking()
            {
                Customer = customer,
                CustomerId = BookingDto.CustomerId,
                BookingDate = BookingDto.BookingDate,
                PaymentMethod = BookingDto.PaymentMethod,
                Status = BookingDto.Status
            };
            try
            {
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the Booking.");
                serviceResponse.Messages.Add(ex.Message);
            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = booking.Bid;
            return serviceResponse;
        }
        public async Task<ServiceResponse> DeleteBooking(int id)
        {
            ServiceResponse response = new();

            var Booking = await _context.Bookings.FindAsync(id);
            if (Booking == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Booking cannot be deleted because it does not exist.");
                return response;
            }

            try
            {
                _context.Bookings.Remove(Booking);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error encountered while deleting the Booking");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;

            return response;

        }
        public async Task<IEnumerable<BookingDto>> ListBookingsForCustomer(int id)
        {
            List<Booking> bookings = await _context.Bookings
                .Include(b => b.Customer)
                .Where(b => b.CustomerId == id)
                .ToListAsync();

            // empty list data transfer object BookingDto
            List<BookingDto> bookingDtos = new List<BookingDto>();
            foreach (Booking booking in bookings)
            {
                // create new instance of BookingDto, add to list
                bookingDtos.Add(new BookingDto()
                {
                    Bid = booking.Bid,
                    CustomerName = booking.Customer.Name,
                    CustomerId = booking.Customer.Cid,
                    BookingDate = booking.BookingDate,
                    PaymentMethod = booking.PaymentMethod,
                    Status = booking.Status,
                });
                
            }
            return bookingDtos;
        }
    }

    
}
