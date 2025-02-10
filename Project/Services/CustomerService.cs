using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Interfaces;
using Project.Models;

namespace Project.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        // dependency injection of database context
        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }
        // implementations of customer create, read, update, delete go here
        public async Task<IEnumerable<CustomerDto>> ListCustomers()
        {
            // all Customers
            List<Customer> Customers = await _context.Customers
                .ToListAsync();
            // empty list of data transfer object CustomerDto
            List<CustomerDto> CustomerDtos = new List<CustomerDto>();

            foreach (Customer Customer in Customers)
            {
                // create new instance of CustomerDto, add to list
                CustomerDtos.Add(new CustomerDto()
                {
                    Cid = Customer.Cid,
                    Name = Customer.Name,
                    Email = Customer.Email,
                    Phone = Customer.Phone
                });
            }
            // return CustomerDtos
            return CustomerDtos;
        }
        public async Task<CustomerDto?> FindCustomer(int id)
        {

            var Customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Cid == id);

            // no order item found
            if (Customer == null)
            {
                return null;
            }
            // create an instance of CustomerDto
            CustomerDto CustomerDto = new CustomerDto()
            {
                Cid = Customer.Cid,
                Name = Customer.Name,
                Email = Customer.Email,
                Phone = Customer.Phone
            };
            return CustomerDto;

        }
        public async Task<ServiceResponse> UpdateCustomer(CustomerDto CustomerDto)
        {
            ServiceResponse serviceResponse = new();


            // Create instance of Customer
            Customer Customer = new Customer()
            {
                Cid = CustomerDto.Cid,
                Name = CustomerDto.Name,
                Email = CustomerDto.Email,
                Phone = CustomerDto.Phone
            };
            // flags that the object has changed
            _context.Entry(Customer).State = EntityState.Modified;

            try
            {
                // SQL Equivalent: Update Customer set ... where CustomerId={id}
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
        public async Task<ServiceResponse> AddCustomer(CustomerDto CustomerDto)
        {
            ServiceResponse serviceResponse = new();


            // Create instance of Customer
            Customer Customer = new Customer()
            {
                Name = CustomerDto.Name,
                Email = CustomerDto.Email,
                Phone = CustomerDto.Phone
            };
            // SQL Equivalent: Insert into Customers (..) values (..)

            try
            {
                _context.Customers.Add(Customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the Category.");
                serviceResponse.Messages.Add(ex.Message);

            }


            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = Customer.Cid;
            return serviceResponse;
        }


        public async Task<ServiceResponse> DeleteCustomer(int id)
        {
            ServiceResponse response = new();
            // Customer id exist in the first place
            var Customer = await _context.Customers.FindAsync(id);
            if (Customer == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Customer cannot be deleted because it does not exist.");
                return response;
            }

            try
            {
                _context.Customers.Remove(Customer);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error encountered while deleting the customer");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;

            return response;

        }
    }
}
