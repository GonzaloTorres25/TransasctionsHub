using System.Security.Cryptography;
using System.Text;

namespace USAEpayHeaderGenerator
{
    class Program
    {
        static void Main()
        {
            string seed = "abcdefghijklmnop";

            string apiKey = "_V87Qtb513Cd3vabM7RC0TbtJWeSo8p7";
            string apiPin = "123456";

            string prehash = apiKey + seed + apiPin;
            string hash = ComputeSha256Hash(prehash);

            string apiHash = $"s2/{seed}/{hash}";

            string rawAuth = $"{apiKey}:{apiHash}";
            string base64Auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(rawAuth));

            Console.WriteLine("Authorization: Basic " + base64Auth);
        }

        static string ComputeSha256Hash(string rawData)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();
            foreach (byte b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }
}