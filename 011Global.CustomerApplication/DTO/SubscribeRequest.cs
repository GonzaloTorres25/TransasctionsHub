using System.ComponentModel.DataAnnotations;

namespace _011Global.CustomerApplication.DTO
{
    public class SubscribeRequest : IValidatableObject
    {
        public static readonly decimal[] AllowedPackageAmounts = { 10.00m, 25.00m, 99.99m };

        [Required(ErrorMessage = "CustomerEmail is required.")]
        [EmailAddress(ErrorMessage = "CustomerEmail must be a valid email.")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "FirstName is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "PackageAmount is required.")]
        public decimal PackageAmount { get; set; }
        [Required(ErrorMessage = "ShippingAddress is required.")]
        public AddressDto ShippingAddress { get; set; }
        [Required(ErrorMessage = "BillingAddress is required.")]
        public AddressDto BillingAddress { get; set; }
        [Required(ErrorMessage = "CreditCard is required.")]
        public CreditCardDto CreditCard { get; set; }
    
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!AllowedPackageAmounts.Contains(PackageAmount))
            {
                string allowed = string.Join(", ", AllowedPackageAmounts.Select(a => a.ToString("0.##")));
                yield return new ValidationResult(
                    $"PackageAmount must be one of the following values: {allowed}.",
                    new[] { nameof(PackageAmount) });
            }
        }
    }
}

