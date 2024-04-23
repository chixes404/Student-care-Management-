using Graduation_Project.Shared.DTO;
using Graduation_Project.Shared.DTO.AnotherPaymentWay;
using Stripe;
namespace Graduation_Project.API.Services
{
    public interface IPaymentService
    {

        //Task<PaymentResponse> ProcessPayment(PaymentRequest request);
        Task<CustomerResource> CreateCustomer(CreateCustomerResource resource, CancellationToken cancellationToken);
        Task<ChargeResource> CreateCharge(CreateChargeResource resource, CancellationToken cancellationToken);
        Task<string> GetExistingCustomerId(string Email, CancellationToken cancellationToken);
        Task<PaymentResponse> ProcessPaymentRequest(ProcessPaymentRequest request, CancellationToken cancellationToken , int studentId);


    }
}
