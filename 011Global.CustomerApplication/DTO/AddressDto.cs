using System.ComponentModel.DataAnnotations;

namespace _011Global.CustomerApplication.DTO
{
    public class AddressDto
    {
        [Required]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Country ISO code must be exactly 2 characters.")]
        public string CountryIso2 { get; set; }
        [Required]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Country ISO code must be exactly 2 characters.")]
        public string StateIso2 { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "ZipCode is required")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "AddressLine is required")]
        public string AddressLine { get; set; }
    }
}
