using System.Text;
using _011Global.JobsService.Entities;
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
        private TokenizationService _tokenizationService;
        private TransactionService _transactionService;
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

            _tokenizationService = new TokenizationService(_httpClient, _authorizationService);
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
        public async Task TokenizationCard_ReturnsToken_WhenResponseIsSuccessful()
        {
            var creditCardNumber = "4111111111111111";
            var expiration = DateTime.UtcNow.AddYears(1);

            var token = await _tokenizationService.TokenizationCard(creditCardNumber, expiration);

            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public async Task Charge_ReturnsPaymentResult_WhenSuccessful()
        {
            var creditCardNumber = "4111111111111111";
            var expiration = DateTime.UtcNow.AddYears(1);

            var token = await _tokenizationService.TokenizationCard(creditCardNumber, expiration);
            Assert.False(string.IsNullOrWhiteSpace(token));

            var customer = new Customer
            {
                CustomerId = 123,
                MonthlyFee = 10
            };

            var creditCard = new CreditCard
            {
                Token = token
            };

            var result = await _transactionService.Charge(customer, creditCard);
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.result));
        }
    }
}