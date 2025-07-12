using _011Global.CustomerApplication.Common;
using _011Global.CustomerApplication.DTO;
using _011Global.CustomerApplication.Interfaces;
using _011Global.Shared.DbContexts.AddressDbContext;
using _011Global.Shared.DbContexts.AddressDbContext.Interfaces;
using _011Global.Shared.DbContexts.CreditCardsDbContext;
using _011Global.Shared.DbContexts.CreditCardsDbContext.Intefaces;
using _011Global.Shared.DbContexts.CustomerDbContext;
using _011Global.Shared.DbContexts.CustomerDbContext.Interfaces;
using _011Global.Shared.Exceptions;
using Microsoft.EntityFrameworkCore.Storage;

namespace _011Global.CustomerApplication.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ICreditCardRepository _creditCardRepository;

        public SubscriptionService(ICustomerRepository customerRepository, IAddressRepository addressRepository, ICreditCardRepository creditCardRepository)
        {
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
            _creditCardRepository = creditCardRepository;
        }

        public async Task<ServiceResult> SubscribeCustomer(SubscribeRequest request)
        { 
            //Starts a database transaction to ensure all operations succeed or all are rolled back in case of failure
            using var transaction = await _customerRepository.BeginTransactionDbAsync();
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

                var card = new CreditCard
                {
                    CustomerId = customer.CustomerId,
                    CreditCardNumber = request.CreditCard.CreditCardNumber,
                    LastFourNumbers = request.CreditCard.CreditCardNumber[^4..],
                    CardHolder = request.CreditCard.CardHolder,
                    ExpirationMonth = request.CreditCard.Expiration.Month.ToString("D2"),
                    ExpirationYear = request.CreditCard.Expiration.Year.ToString(),
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
                return await HandleExceptionAsync(transaction, errorMessage + "error:" + ex);
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
