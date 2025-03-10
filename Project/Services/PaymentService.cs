using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Interfaces;
using Project.Models;
using System;
namespace Project.Services
{
    public class PaymentService :IPaymentService
    {
        private readonly ApplicationDbContext _context;
        // dependency injection of database context
        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PaymentDto>> ListPayments()
        {
            List<Payment> Payments = await _context.Payments
                .Include(p => p.Booking)
                .Include(p => p.Booking.Customer)
                .ToListAsync();
            // empty list of data transfer object PaymentDto
            List<PaymentDto> PaymentDtos = new List<PaymentDto>();
            
            foreach (Payment Payment in Payments)
            {
                // create new instance of PaymentDto, add to list
                PaymentDtos.Add(new PaymentDto()
                {
                    Pid = Payment.Pid,
                    CustomerName = Payment.Booking.Customer.Name,
                    Bid = Payment.Booking.Bid,
                    TotalAmount = Payment.TotalAmount,
                    PaymentDate = Payment.PaymentDate,
                    PaymentMethod = Payment.Booking.PaymentMethod,
                    Status = Payment.Status
                });
            }
            return PaymentDtos;
        }
        public async Task<PaymentDto?> FindPayment(int id)
        {
            var Payment = await _context.Payments
                .Include(p => p.Booking)
                .Include(p=>p.Booking.Customer)
                .FirstOrDefaultAsync(p => p.Pid == id);

            if (Payment == null)
            {
                return null;
            }
            // create an instance of PaymentDto
            PaymentDto PaymentDto = new PaymentDto()
            {
                Pid = Payment.Pid,
                CustomerName = Payment.Booking.Customer.Name,
                Bid = Payment.Booking.Bid,
                TotalAmount = Payment.TotalAmount,
                PaymentDate = Payment.PaymentDate,
                PaymentMethod = Payment.Booking.PaymentMethod,
                Status = Payment.Status
            };
            return PaymentDto;

        }
        public async Task<ServiceResponse> UpdatePayment(PaymentDto PaymentDto)
        {
            ServiceResponse serviceResponse = new();


            // Create instance of Payment
            Payment Payment = new Payment()
            {
                Pid = PaymentDto.Pid,
                BookingId = PaymentDto.Bid,
                TotalAmount = PaymentDto.TotalAmount,
                PaymentDate = PaymentDto.PaymentDate,
                
                Status = PaymentDto.Status
            };
            // flags that the object has changed
            _context.Entry(Payment).State = EntityState.Modified;

            try
            {
                // SQL Equivalent: Update Payment set ... where Pid={id}
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
        public async Task<ServiceResponse> AddPayment(PaymentDto PaymentDto)
        {
            ServiceResponse serviceResponse = new();


            // Create instance of Payment
            Booking? booking = await _context.Bookings.FindAsync(PaymentDto.Bid);
            // data must link to a valid entry
            if (booking == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (booking == null)
                {
                    serviceResponse.Messages.Add("Payment was not found. ");
                }

                return serviceResponse;
            }
            Payment Payment = new Payment()
            {
                Booking = booking,
                BookingId = PaymentDto.Bid,
                TotalAmount= PaymentDto.TotalAmount,
                PaymentDate = PaymentDto.PaymentDate,
                Status = PaymentDto.Status
            };
            try
            {
                _context.Payments.Add(Payment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the Payment.");
                serviceResponse.Messages.Add(ex.Message);
            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = booking.Bid;
            return serviceResponse;
        }
        public async Task<ServiceResponse> DeletePayment(int id)
        {
            ServiceResponse response = new();

            var Payment = await _context.Payments.FindAsync(id);
            if (Payment == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Payment cannot be deleted because it does not exist.");
                return response;
            }

            try
            {
                _context.Payments.Remove(Payment);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error encountered while deleting the Payment");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;

            return response;

        }
        public async Task<IEnumerable<PaymentDto>> ListPaymentsForCustomer(int id)
        {
            List<Payment> payments = await _context.Payments
                .Include(p=>p.Booking)
                .Include(p=>p.Booking.Customer)
                .Where(p => p.Booking.CustomerId == id)
                .ToListAsync();

            // empty list data transfer object PaymentDto
            List<PaymentDto> PaymentDtos = new List<PaymentDto>();
            foreach (Payment Payment in payments)
            {
                // create new instance of BookingDto, add to list
                PaymentDtos.Add(new PaymentDto()
                {
                    Pid = Payment.Pid,
                    CustomerName = Payment.Booking.Customer.Name,
                    Bid = Payment.BookingId,
                    TotalAmount = Payment.TotalAmount,
                    PaymentDate = Payment.PaymentDate,
                    PaymentMethod = Payment.Booking.PaymentMethod,
                    Status = Payment.Status,
                });

            }
            return PaymentDtos;
        }
    }
}
