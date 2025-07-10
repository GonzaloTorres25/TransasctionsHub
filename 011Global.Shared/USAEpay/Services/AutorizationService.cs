using System.Security.Cryptography;
using System.Text;
using _011Global.JobsService.Entities;
using _011Global.Shared.USAEpay.Intefaces;
using Microsoft.Extensions.Options;

namespace _011Global.Shared.USAEpay.Services
{
    public class AutorizationService : IAutorizationService
    {
        private readonly USAEpaySettings _settings;
        
        public AutorizationService (IOptions<USAEpaySettings> options)
        {
            _settings = options.Value;
        }
        public string GenerateAuthorizationHeader()
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
