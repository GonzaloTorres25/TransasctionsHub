using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using _011Global.Shared.Entities;
using _011Global.Shared.USAEpay.Intefaces;

namespace _011Global.Shared.USAEpay.Services
{
    public class TokenizationService : ITokenizationService
    {
        private readonly HttpClient _httpClient;
        private readonly IAutorizationService _autorizationService;
        public TokenizationService(HttpClient httpClient, IAutorizationService autorizationService)
        {
            _httpClient = httpClient;
            _autorizationService = autorizationService;
        }

        public async Task<string> TokenizationCard(string creditCardNumber, DateTime expiration)
        {
            var authHeader = _autorizationService.GenerateAuthorizationHeader();
            var sendRequest = BuildTokenizationRequest(creditCardNumber,expiration);

            var request = new HttpRequestMessage(HttpMethod.Post, "tokens")
            {
                Content = JsonContent.Create(sendRequest)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

            var response = await _httpClient.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Tokenization failed: {response.StatusCode} - {responseContent}");
            }

            using var jsonDoc = JsonDocument.Parse(responseContent);

            if (jsonDoc.RootElement.TryGetProperty("key", out JsonElement tokenElement))
            {
                return tokenElement.GetString();
            }

            if (jsonDoc.RootElement.TryGetProperty("error", out JsonElement errorElement))
            {
                throw new Exception($"Tokenization error: {errorElement.GetString()}");
            }

            throw new Exception("Unexpected response format from tokenization service.");
        }
        private USAEpayTokenizationRequest BuildTokenizationRequest(string creditCardNumber, DateTime expiration)
        {
            return new USAEpayTokenizationRequest
            {
                creditcard = new CreditCardDTO
                {
                    number = creditCardNumber,
                    expiration = expiration.ToString("MMyy")
                }
            };
        }
    }
}
