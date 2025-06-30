using _011Global.CustomerApplication.DTO;
using _011Global.CustomerApplication.Interfaces;
using _011Global.Shared.DbContexts.AddressDbContext;
using _011Global.Shared.DbContexts.AddressDbContext.Interfaces;
using _011Global.Shared.DbContexts.CreditCardsDbContext.Intefaces;
using _011Global.Shared.DbContexts.CustomerDbContext;
using _011Global.Shared.DbContexts.CustomerDbContext.Interfaces;
using _011Global.Shared.Exceptions;
using _011Global.Shared.JobsServiceDBContext;
using Microsoft.EntityFrameworkCore.Storage;

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
                var shipping = await GetOrCreateAddress(request.ShippingAddress);
                var billing = await GetOrCreateAddress(request.BillingAddress);

                var customer = new Customer
                {
                    CustomerEmail = request.CustomerEmail,
                    CustomerFirstName = request.FirstName,
                    CustomerLastName = request.LastName,
                    ShippingAddressID = shipping.AddressID,
                    BillingAddressID = billing.AddressID,
                    MonthlyFee = request.PackageAmount,
                    CreationDate = DateTime.UtcNow,
                    Subscribed = true
                };
                await _customerRepository.Add(customer);

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
                    CardHolder = request.CreditCard.CardHolder,
                    SecurityCode = request.CreditCard.SecurityCode,
                    ExpirationMonth = request.CreditCard.Expiration.Month,
                    ExpirationYear = request.CreditCard.Expiration.Year,
                    CreationDate = DateTime.UtcNow
                };
                await _creditCardRepository.Add(card);
                await transaction.CommitAsync();

                return new ServiceResult
                {
                    Success = true,
                    Message = "Customer subscribed successfully."
                };
            }
            catch (AddDBException ex)
            {
                return await HandleExceptionAsync(transaction, ex.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = "An unexpected error occurred.";
                return await HandleExceptionAsync(transaction, errorMessage);
            }
        }

        private async Task<GeneralAddress> GetOrCreateAddress(AddressDto Address)
        {
            var existing = await _addressRepository.FindAddressMatch(
                    Address.CountryIso2, Address.StateIso2, Address.City, Address.ZipCode, Address.AddressLine);

            if (existing == null)
            {
                existing = new GeneralAddress
                {
                    CountryIso2 = Address.CountryIso2,
                    StateIso2 = Address.StateIso2,
                    City = Address.City,
                    ZipCode = Address.ZipCode,
                    Address = Address.AddressLine,
                    CreationDate = DateTime.UtcNow
                };

                await _addressRepository.Add(existing);
            }
            return existing;
        }
        private async Task<ServiceResult> HandleExceptionAsync(IDbContextTransaction transaction, string message)
        {
            await transaction.RollbackAsync();
            return new ServiceResult
            {
                Success = false,
                Message = message
            };
        }
    }
}
