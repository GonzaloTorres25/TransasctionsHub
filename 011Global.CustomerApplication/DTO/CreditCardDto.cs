using System.ComponentModel.DataAnnotations;
using _011Global.CustomerApplication.Common;

namespace _011Global.CustomerApplication.DTO
{
    public class CreditCardDto
    {
        [Required]
        [CreditCard(ErrorMessage = "Invalid credit card number.")]
        public string CreditCardNumber { get; set; }
        [Required(ErrorMessage = "`Card holder is required")]
        public string CardHolder {get; set;}
        [Required(ErrorMessage = "Expiration date is required.")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "The credit card is expired.")]
        public DateTime Expiration { get; set; }
    }
}