using System.Text;
using _011Global.JobsService.Entities;
using _011Global.JobsService.JobInterfaces;
using _011Global.JobsService.Services;
using _011Global.Shared.DbContexts.CreditCardsDbContext;
using _011Global.Shared.DbContexts.CustomerDbContext;
using _011Global.Shared.USAEpay.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace TransasctionsHubUnitTesting
{
    public class USAEpayTest
    {
        private AutorizationService _authorizationService;
        private HttpClient _httpClient;
        private ITransactionService _transactionService;
        private readonly USAEpaySettings settings;

        public USAEpayTest()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<USAEpayTest>()
                .Build();

            settings = new USAEpaySettings();  
            configuration.GetSection("USAEpaySettings").Bind(settings);

            var options = Options.Create(settings);

            _authorizationService = new AutorizationService(options);

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://sandbox.usaepay.com/api/v2/")
            };

            _transactionService = new TransactionService(_httpClient, _authorizationService);
        }

        [Fact]
        public void GenerateAuthorizationHeader_ReturnsBase64StringWithExpectedFormat()
        {
            var generator = new AutorizationService(Options.Create(settings));
            string result = generator.GenerateAuthorizationHeader();

            Assert.False(string.IsNullOrWhiteSpace(result));

            string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(result));

            Assert.StartsWith(settings.ApiKey + ":s2/", decoded);

            var parts = decoded.Split(':');
            Assert.Equal(2, parts.Length);

            var hashParts = parts[1].Split('/');
            Assert.Equal(3, hashParts.Length);

            Assert.Matches("^[a-zA-Z0-9]{10}$", hashParts[1]);
            Assert.Matches("^[0-9a-f]{64}$", hashParts[2]);
        }

        [Fact]
        public async Task Charge_ReturnsPaymentResult_WhenSuccessful()
        {
            var customer = new Customer
            {
                CustomerId = 1,
                MonthlyFee = 10
            };

            var creditCard = new CreditCard
            {
                CreditCardNumber = "4111111111111111",
                ExpirationMonth = "03",
                ExpirationYear = "2027"
            };

            var resultWithSave = await _transactionService.Charge(customer, creditCard, true);
            Assert.NotNull(resultWithSave);
            Assert.False(string.IsNullOrEmpty(resultWithSave.key));
            Assert.False(string.IsNullOrEmpty(resultWithSave.refnum));
            Assert.False(string.IsNullOrEmpty(resultWithSave.authcode));
            Assert.False(string.IsNullOrEmpty(resultWithSave.auth_amount));
            Assert.Equal("Approved", resultWithSave.result);
            Assert.Null(resultWithSave.error);
            Assert.False(string.IsNullOrWhiteSpace(resultWithSave.savedCard.key));
            Assert.False(string.IsNullOrWhiteSpace(resultWithSave.savedCard.type));
            Assert.False(string.IsNullOrWhiteSpace(resultWithSave.savedCard.cardnumber));

            var resultWithoutSave = await _transactionService.Charge(customer, creditCard, false);
            Assert.NotNull(resultWithoutSave);
            Assert.False(string.IsNullOrEmpty(resultWithoutSave.key));
            Assert.False(string.IsNullOrEmpty(resultWithoutSave.refnum));
            Assert.False(string.IsNullOrEmpty(resultWithoutSave.authcode));
            Assert.False(string.IsNullOrEmpty(resultWithoutSave.auth_amount));
            Assert.Equal("Approved", resultWithoutSave.result);
            Assert.Null(resultWithoutSave.error);
            Assert.Null(resultWithoutSave.savedCard);
        }

        [Fact]
        public async Task Charge_ReturnsPaymentsResult_WhenFail()
        {
            var customer = new Customer
            {
                CustomerId = 1,
                MonthlyFee = 10
            };

            var invalidCardNumber = new CreditCard
            {
                CreditCardNumber = "4000000000000002",
                ExpirationMonth = "13",
                ExpirationYear = "2027"
            };

            var invalidCardExpiration = new CreditCard
            {
                CreditCardNumber = "4111111111111111",
                ExpirationMonth = "01",
                ExpirationYear = "2027"
            };

            var result = await _transactionService.Charge(customer, invalidCardNumber,  true);
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.refnum));
            Assert.Null(result.authcode);
            Assert.Null(result.savedCard);
            Assert.Equal("Error", result.result);
            Assert.False(string.IsNullOrEmpty(result.error));

            var result2 = await _transactionService.Charge(customer, invalidCardExpiration, true);
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.refnum));
            Assert.Null(result.authcode);
            Assert.Null(result.savedCard);
            Assert.Equal("Error", result.result);
            Assert.False(string.IsNullOrEmpty(result.error));
        }
    }
}