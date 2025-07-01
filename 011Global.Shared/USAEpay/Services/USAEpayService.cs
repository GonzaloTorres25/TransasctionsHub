using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using _011Global.JobsService.Entities;
using _011Global.JobsService.JobInterfaces;
using _011Global.Shared.DbContexts.CreditCardsDbContext;
using _011Global.Shared.DbContexts.CustomerDbContext;
using _011Global.Shared.IUSAEpay;
using Microsoft.Extensions.Options;

namespace _011Global.JobsService.Services
{
    public class USAEpayService : IUSAEpayService
    {
        private readonly HttpClient _httpClient;
        private readonly USAEpaySettings _settings;

        public USAEpayService(HttpClient httpClient, IOptions<USAEpaySettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<PaymentResult> Charge(Customer customer, CreditCard creditCard)
        {
            var authHeader = GenerateAuthorizationHeader();
            var sendRequest = BuildPaymentRequest(customer, creditCard);

            var request = new HttpRequestMessage(HttpMethod.Post, "transactions")
            {
                Content = JsonContent.Create(sendRequest)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var paymentResult = await response.Content.ReadFromJsonAsync<PaymentResult>();

            return paymentResult!;
        }
        private USAEpayRequest BuildPaymentRequest(Customer customer, CreditCard creditCard)
        {
            return new USAEpayRequest
            {
                amount = (decimal)customer.MonthlyFee,
                creditcard = new CreditCardDTO
                {
                    number = creditCard.CreditCardNumber,
                    expiration = $"{creditCard.ExpirationMonth.PadLeft(2, '0')}{creditCard.ExpirationYear[^2..]}"
                }
            };
        }

        private string GenerateAuthorizationHeader()
        {
            string seed = GenerateRandomSeed(); 
            string apiKey = _settings.ApiKey;
            string apiPin = _settings.ApiPin;
            string prehash = apiKey + seed + apiPin;
            string hash = ComputeSha256Hash(prehash);
            string apiHash = $"s2/{seed}/{hash}";
            string rawAuth = $"{apiKey}:{apiHash}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(rawAuth));
        }

        private string GenerateRandomSeed()
        {
            var chars = Enumerable.Range('a', 26).Select(c => (char)c)
                .Concat(Enumerable.Range('A', 26).Select(c => (char)c))
                .Concat(Enumerable.Range('0', 10).Select(c => (char)c))
                .ToArray();
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string ComputeSha256Hash(string prehash)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(prehash));
            var builder = new StringBuilder();
            foreach (byte b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }
}
