using System.ComponentModel.DataAnnotations;

namespace _011Global.CustomerApplication.DTO
{
    public class SubscribeRequest
    {
        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public SubscriptionPackage PackageAmount { get; set; }
        [Required]
        public AddressDto ShippingAddress { get; set; }
        [Required]
        public AddressDto BillingAddress { get; set; }
        [Required]
        public CreditCardDto CreditCard { get; set; }
    }
}
