using System.ComponentModel.DataAnnotations;

namespace _011Global.CustomerApplication.DTO
{
    public class CreditCardDto
    {
        [Required]
        public string CreditCardNumber { get; set; }
        [Required]
        public string SecurityCode { get; set; }
        [Required]
        public DateTime Expiration { get; set; }
    }
}
