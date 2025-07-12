using _011Global.CustomerApplication.DTO;
using _011Global.CustomerApplication.Interfaces;
using _011Global.CustomerApplication.Services;
using _011Global.Shared.DbContexts.AddressDbContext.Repos;
using _011Global.Shared.DbContexts.CreditCardsDbContext.Repos;
using _011Global.Shared.DbContexts.CustomerDbContext.Interfaces;
using _011Global.Shared.DbContexts.CustomerDbContext.Repos;
using _011Global.Shared.JobsServiceDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace TransasctionsHubUnitTesting
{
    public class SubscriptionTest
    {
        private ISubscriptionService _subscriptionService;
        private ICustomerRepository _customerRepository;
        public SubscriptionTest() 
        {
            var configuration = new ConfigurationBuilder()
           .AddUserSecrets<SubscriptionTest>()
           .AddEnvironmentVariables()
           .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<JobsServiceContext>()
             .UseSqlServer(connectionString)
             .Options;

            var context = new JobsServiceContext(options);
            var loggerCustomer = NullLogger<CustomerRepository>.Instance;
            var loggerAddress = NullLogger<AddressRepository>.Instance;
            var loggerCreditCard = NullLogger<CreditCardRepository>.Instance;

            _customerRepository = new CustomerRepository(context, loggerCustomer);
            var addressRepository = new AddressRepository(context, loggerAddress);
            var creditCardRepository = new CreditCardRepository(context, loggerCreditCard);
            _subscriptionService = new SubscriptionService(_customerRepository, addressRepository, creditCardRepository);
        }

        [Fact]
        public async Task Subscribe_Customer_WhenSuccessful()
        {
            var request = _validSubscribeRequest;
            var existingCustomer = await _customerRepository.GetByEmail(request.CustomerEmail);

            if (existingCustomer != null)
            {
                Assert.True(false, $"Customer {request.CustomerEmail} already exists. Use another email.");
            }
            var result = await _subscriptionService.SubscribeCustomer(request);
            Assert.NotNull(result);
            Assert.True(result.Success);

            var customer = await _customerRepository.GetByEmail(request.CustomerEmail);
            Assert.NotNull(customer);
        }

        [Fact]
        public async Task Subscribe_Customer_WhenFail()
        {
            var request = _validSubscribeRequest;
            request.CustomerEmail = "SubscribeFailTest";
            request.CreditCard.CreditCardNumber = "1";

            var result = await _subscriptionService.SubscribeCustomer(request);
            Assert.NotNull(result);
            Assert.False(result.Success);

            var customer = await _customerRepository.GetByEmail(request.CustomerEmail);
            Assert.Null(customer);
        }

        private SubscribeRequest _validSubscribeRequest = new SubscribeRequest
        {
            CustomerEmail = "SubscribeTest@example.com",
            FirstName = "Test",
            LastName = "User",
            PackageAmount = 10.00m,
            ShippingAddress = new AddressDto
            {
                CountryIso2 = "UY",
                StateIso2 = "MO",
                City = "Montevideo",
                ZipCode = "11100",
                AddressLine = "123 Main St"
            },
            BillingAddress = new AddressDto
            {
                CountryIso2 = "UY",
                StateIso2 = "MO",
                City = "Montevideo",
                ZipCode = "11100",
                AddressLine = "123 Main St"
            },
            CreditCard = new CreditCardDto
            {
                CreditCardNumber = "4111111111111111",
                CardHolder = "subTestHolder",
                Expiration = new DateTime(2027, 12, 20),
            }
        };
    }
}
