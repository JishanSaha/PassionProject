using Project.Models;

namespace Project.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> ListPayments();

        Task<PaymentDto?> FindPayment(int id);

        Task<ServiceResponse> UpdatePayment(PaymentDto PaymentDto);

        Task<ServiceResponse> AddPayment(PaymentDto PaymentDto);

        Task<ServiceResponse> DeletePayment(int id);

        Task<IEnumerable<PaymentDto>> ListPaymentsForCustomer(int id);
    }
}
