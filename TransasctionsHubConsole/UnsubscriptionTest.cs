using _011Global.CustomerApplication.Interfaces;
using _011Global.CustomerApplication.Services;
using _011Global.Shared.DbContexts.CustomerDbContext.Interfaces;
using _011Global.Shared.DbContexts.CustomerDbContext.Repos;
using _011Global.Shared.JobsServiceDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;


namespace TransasctionsHubUnitTesting
{
    public class UnsubscriptionTest
    {
        private IUnsubscritionService _unsubscriptionService;
        private ICustomerRepository _customerRepository;

        public UnsubscriptionTest () 
        {
            var configuration = new ConfigurationBuilder()
           .AddUserSecrets<UnsubscriptionTest>()
           .AddEnvironmentVariables()
           .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<JobsServiceContext>()
             .UseSqlServer(connectionString)
             .Options;

            var context = new JobsServiceContext(options);

            var logger = NullLogger<CustomerRepository>.Instance;
            _customerRepository = new CustomerRepository(context, logger);
            _unsubscriptionService = new UnsubscriptionService(_customerRepository);
        }

        [Fact]
        public async Task Unsubscribe_Customer_WhenSuccessful()
        {
            var email = "Test1@example.com";
            var customer = await _customerRepository.GetByEmail(email);
            Assert.NotNull(customer);

            if (!customer.Subscribed)
            {
                // Subscribe the customer and confirm it worked
                customer.Subscribed = true;
                await _customerRepository.SubscribeCustomer(customer);
                var checkSubscribedCustomer = await _customerRepository.GetByEmail(email);
                Assert.NotNull(checkSubscribedCustomer);
                Assert.True(checkSubscribedCustomer.Subscribed);
            }

            var result = await _unsubscriptionService.UnsubscribeCustomer(email);
            Assert.True(result.Success);

            var updatedCustomer = await _customerRepository.GetByEmail(email);
            Assert.NotNull(updatedCustomer);
            Assert.False(updatedCustomer.Subscribed);
        }

        [Fact]
        public async Task Unsubscribe_Customer_Whenfail()
        {
            var nonExistingEmail = "no-exists@example.com";
            var result = await _unsubscriptionService.UnsubscribeCustomer(nonExistingEmail);
            Assert.NotNull(result);
            Assert.False(result.Success);
        }
    }
}
