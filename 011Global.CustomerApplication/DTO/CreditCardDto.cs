using System.ComponentModel.DataAnnotations;

namespace _011Global.CustomerApplication.DTO
{
    public class CreditCardDto
    {
        [Required]
        [CreditCard(ErrorMessage = "Invalid credit card number.")]
        public string CreditCardNumber { get; set; }
        [Required]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "Security code must be 3 or 4 digits.")]
        public string SecurityCode { get; set; }
        [Required]
        public string CardHolder {get; set;}
        [Required]
        [DataType(DataType.Date)]
        public DateTime Expiration { get; set; }
    }
}
