using _011Global.CustomerApplication.DTO;
using _011Global.CustomerApplication.Interfaces;
using _011Global.Shared.AddressDbContext;
using _011Global.Shared.AddressDbContext.Intefaces;
using _011Global.Shared.CreditCardsDbContext;
using _011Global.Shared.CreditCardsDbContext.Intefaces;
using _011Global.Shared.CustomerContext.Interfaces;
using _011Global.Shared.CustomerDbContext;
using _011Global.Shared.JobsServiceDBContext;

namespace _011Global.CustomerApplication.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly JobsServiceContext _jobsServiceContext;

        public SubscriptionService(JobsServiceContext jobsServiceContext, ICustomerRepository customerRepository, IAddressRepository addressRepository, ICreditCardRepository creditCardRepository)
        {
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
            _creditCardRepository = creditCardRepository;
            _jobsServiceContext = jobsServiceContext;
        }

        public async Task<ServiceResult> SubscribeCustomer(SubscribeRequest request)
        {
            using var transaction = await _jobsServiceContext.Database.BeginTransactionAsync();

            try
            {
                var shipping = new GeneralAddress
                {
                    CountryIso2 = request.ShippingAddress.CountryIso2,
                    StateIso2 = request.ShippingAddress.StateIso2,
                    City = request.ShippingAddress.City,
                    ZipCode = request.ShippingAddress.ZipCode,
                    Address = request.ShippingAddress.AddressLine,
                    CreationDate = DateTime.UtcNow
                };
                await _addressRepository.Add(shipping);
                await _jobsServiceContext.SaveChangesAsync();

                var billing = new GeneralAddress
                {
                    CountryIso2 = request.BillingAddress.CountryIso2,
                    StateIso2 = request.BillingAddress.StateIso2,
                    City = request.BillingAddress.City,
                    ZipCode = request.BillingAddress.ZipCode,
                    Address = request.BillingAddress.AddressLine,
                    CreationDate = DateTime.UtcNow
                };
                await _addressRepository.Add(billing);
                await _jobsServiceContext.SaveChangesAsync();

                var customer = new Customer
                {
                    CustomerEmail = request.CustomerEmail,
                    CustomerFirstName = request.FirstName,
                    CustomerLastName = request.LastName,
                    ShippingAddressID = shipping.AddressID,
                    BillingAddressID = billing.AddressID,
                    MonthlyFee = (decimal)request.PackageAmount,
                    CreationDate = DateTime.UtcNow
                };
                await _customerRepository.Add(customer);
                await _jobsServiceContext.SaveChangesAsync();

                int LastFourNumbers = int.TryParse(
                    request.CreditCard.CreditCardNumber?.Length >= 4
                        ? request.CreditCard.CreditCardNumber[^4..]
                        : request.CreditCard.CreditCardNumber,
                    out var val) ? val : 0;

                var card = new CreditCard
                {
                    CustomerId = customer.CustomerId,
                    CreditCardNumber = request.CreditCard.CreditCardNumber,
                    LastFourNumbers = LastFourNumbers,
                    CardHolder = request.FirstName + " " + request.LastName,
                    SecurityCode = request.CreditCard.SecurityCode,
                    ExpirationMonth = request.CreditCard.Expiration.Month,
                    ExpirationYear = request.CreditCard.Expiration.Year,
                    CreationDate = DateTime.UtcNow
                };
                await _creditCardRepository.Add(card);
                await _jobsServiceContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ServiceResult
                {
                    Success = true,
                    Message = "Customer subscribed successfully."
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Error: {ex.ToString()}"
                };
            }
        }
    }
}
