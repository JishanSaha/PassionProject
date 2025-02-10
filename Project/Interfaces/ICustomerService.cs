using Project.Models;

namespace Project.Interfaces
{
    public interface ICustomerService
    {
        // definitions for implementations of actions to create, read, update, delete

        // base CRUD
        Task<IEnumerable<CustomerDto>> ListCustomers();


        Task<CustomerDto?> FindCustomer(int id);

        Task<ServiceResponse> UpdateCustomer(CustomerDto customerDto);

        Task<ServiceResponse> AddCustomer(CustomerDto customerDto);

        Task<ServiceResponse> DeleteCustomer(int id);
    }
}
