using System.Net.Http.Headers;
using System.Net.Http.Json;
using _011Global.JobsService.Entities;
using _011Global.JobsService.JobInterfaces;
using _011Global.Shared.DbContexts.CreditCardsDbContext;
using _011Global.Shared.DbContexts.CustomerDbContext;
using _011Global.Shared.Entities;
using _011Global.Shared.USAEpay.Intefaces;
namespace _011Global.JobsService.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly HttpClient _httpClient;
        private readonly IAutorizationService _autorizationService;

        public TransactionService(HttpClient httpClient, IAutorizationService autorizationService)
        {
            _httpClient = httpClient;
            _autorizationService = autorizationService;
        }

        public async Task<PaymentResult> Charge(Customer customer, CreditCard creditCard)
        {
            var authHeader = _autorizationService.GenerateAuthorizationHeader();
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
        private USAEpayTransactionRequest BuildPaymentRequest(Customer customer, CreditCard creditCard)
        {
            return new USAEpayTransactionRequest
            {
                amount = (decimal)customer.MonthlyFee,
                creditcard = new CreditCardTokenDTO
                {
                    number = creditCard.Token,
                }
            };
        }

    }
}
