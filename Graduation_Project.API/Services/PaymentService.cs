using Graduation_Project.Shared.DTO;
using Graduation_Project.Shared.Models;
using Graduation_Project.API.Data;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.FinancialConnections;
using Graduation_Project.API.Configuration;
using Graduation_Project.Shared.DTO.AnotherPaymentWay;

namespace Graduation_Project.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDatabaseContext _dbContext;
        private readonly StripeSettings _stripeSettings;
        private readonly TokenService _tokenService;
        private readonly CustomerService _customerService;
        private readonly ChargeService _chargeService;

        public PaymentService(ApplicationDatabaseContext dbContext, IOptions<StripeSettings> stripeSettings , TokenService tokenService,
        CustomerService customerService,
        ChargeService chargeService)
        {
            _dbContext = dbContext;
            _stripeSettings = stripeSettings.Value;
            _tokenService = tokenService;
            _customerService = customerService;
            _chargeService = chargeService;
        }

        //public async Task<PaymentResponse> ProcessPayment(PaymentRequest request)
        //{

        //    StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

        //    // Step 1: Collect payment method details (e.g., card number, expiration date, CVV)
        //    var paymentMethodDetails = new PaymentMethodCreateOptions
        //    {
        //        Type = "card",
        //        Card = new PaymentMethodCardOptions
        //        {
        //            Number = request.CardNumber,
        //            ExpMonth = request.ExpiryMonth,
        //            ExpYear = request.ExpiryYear,
        //            Cvc = request.cvc
        //        }
        //    };

        //    // Step 2: Create a payment method using the collected details
        //    var paymentMethodService = new PaymentMethodService();
        //    var paymentMethod = await paymentMethodService.CreateAsync(paymentMethodDetails);

        //    // Step 3: Associate the payment method with the PaymentIntent
        //    var options = new PaymentIntentCreateOptions
        //    {
        //        Amount = (long)(request.Amount * 100), // Convert amount to cents
        //        Currency = "usd",
        //        PaymentMethod = paymentMethod.Id // Specify the payment method ID
        //    };

        //    var service = new PaymentIntentService();
        //    var paymentIntent = await service.CreateAsync(options);

        //    if (paymentIntent.Status == "succeeded")
        //    {
        //        var parentTransaction = new ParentTransaction
        //        {
        //            ParentId = 1, // Replace with the actual parent ID
        //            TransactionAmount = request.Amount,
        //            TransactionDate = DateTime.Now
        //        };

        //        _dbContext.ParentTransactions.Add(parentTransaction);
        //        await _dbContext.SaveChangesAsync();

        //        var wallet = await _dbContext.Wallets.FirstOrDefaultAsync(x => x.StudentId == 4); // Replace with appropriate logic to retrieve the student's wallet
        //        if (wallet != null)
        //        {
        //            wallet.Balance += request.Amount;
        //            await _dbContext.SaveChangesAsync();
        //        }

        //        return new PaymentResponse { Success = true, Message = "Payment successful" };
        //    }
        //    else
        //    {
        //        return new PaymentResponse { Success = false, Message = "Payment failed" };
        //    }
        //}

        public async Task<CustomerResource> CreateCustomer(CreateCustomerResource resource, CancellationToken cancellationToken)    
        {
            var options = new CustomerCreateOptions
            {
                Name = resource.Name,
                Email = resource.Email,
                Source= "tok_visa"
            };

            var service = new CustomerService();
            var customer = service.Create(options);

            var customerResource = new CustomerResource(customer.Id, customer.Email, customer.Name);
            return customerResource;
        }


        public async Task<ChargeResource?> CreateCharge(CreateChargeResource resource, CancellationToken cancellationToken)
        {
            var chargeOptions = new ChargeCreateOptions
            {
                Currency = resource.Currency,
                Amount = resource.Amount,
                ReceiptEmail = resource.ReceiptEmail,
                Customer = resource.CustomerId,
                Description = $"Parent with this {resource.ReceiptEmail} pay for School Which you make in Middleware ",
                
                
            };

            var charge = await _chargeService.CreateAsync(chargeOptions, null, cancellationToken);
            if (charge.Status == "succeeded")
            {
                return new ChargeResource(
                    charge.Id,
                    charge.Currency,
                    charge.Amount,
                    charge.CustomerId,
                    charge.ReceiptEmail
                    /*charge.Description*/);
            }
            else
            {
                return null;
            }

        }

        public async Task<string> GetExistingCustomerId(string email, CancellationToken cancellationToken)
        {
            var options = new CustomerListOptions
            {
                Email = email,
                Limit = 1 // Limit the number of customers to 1 since email should be unique
            };

            var customers = await _customerService.ListAsync(options, null, cancellationToken);

            if (customers.Any())
            {
                // Return the ID of the first customer found with the provided email
                return customers.Data.First().Id;
            }
            else
            {
                // If no customer found with the provided email, return null
                return null;
            }
        }

        public async Task<PaymentResponse> ProcessPaymentRequest(ProcessPaymentRequest request, CancellationToken cancellationToken , int studentId)
        {
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            string customerId = await GetExistingCustomerId(request.Email, cancellationToken);

            if (customerId == null)
            {
                // Customer doesn't exist, create a new customer
                var customerResource = await CreateCustomer(new CreateCustomerResource
                {
                    Email = request.Email,
                    Name = request.Name,
                    Card = new CreateCardResource
                    {
                        Name = request.Name,
                        Number = request.CardNumber,
                        ExpiryYear = request.ExpiryYear,
                        ExpiryMonth = request.ExpiryMonth,
                        Cvc = request.Cvc
                    }
                }, cancellationToken);

                customerId = customerResource.CustomerId;
            }

            // Create a charge for the customer
            var chargeResource = await CreateCharge(new CreateChargeResource
            {
                Currency = "usd",
                Amount = request.Amount,
                ReceiptEmail = request.Email,
                CustomerId = customerId,
                //Description = request.Description
            }, cancellationToken);

            if (chargeResource != null )
            {
                
                var parentTransaction = new ParentTransaction
                {

                    ParentId = /*ParentId*/ 5, // Adjust this based on your actual logic
                    StudentId=4,
                    TransactionAmount = request.Amount,
                    TransactionDate = DateTime.Now
                };

                _dbContext.ParentTransactions.Add(parentTransaction);
                await _dbContext.SaveChangesAsync();

                // Update the wallet balance
                var wallet = await _dbContext.Wallets.FirstOrDefaultAsync(x => x.StudentId == /*StudentId , 4*/ studentId);
                if (wallet != null)
                {
                    wallet.Balance += request.Amount;
                    await _dbContext.SaveChangesAsync();
                }

                return new PaymentResponse { Success = true, Message = "Payment successful" };
            }
            else
            {
                // Charge creation failed, return appropriate response
                return new PaymentResponse { Success = false, Message = "Payment failed" };
            }
        }

    }
}



