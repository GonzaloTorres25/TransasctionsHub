using System.ComponentModel.DataAnnotations;

namespace _011Global.CustomerApplication.DTO
{
    public class SubscribeRequest : IValidatableObject
    {
        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public decimal PackageAmount { get; set; }
        [Required]
        public AddressDto ShippingAddress { get; set; }
        [Required]
        public AddressDto BillingAddress { get; set; }
        [Required]
        public CreditCardDto CreditCard { get; set; }
    
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            decimal[] allowedValues = { 10.00m, 25.00m, 99.99m };

            if (!allowedValues.Contains(PackageAmount))
            {
                yield return new ValidationResult(
                    "PackageAmount must be one of the following values: 10, 25, or 99.99.",
                    new[] { nameof(PackageAmount) });
            }
        }
    }
}

