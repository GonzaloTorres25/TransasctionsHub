using System.ComponentModel.DataAnnotations;

namespace _011Global.CustomerApplication.DTO
{
    public class AddressDto
    {
        [Required]
        public string CountryIso2 { get; set; }
        [Required]
        public string StateIso2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string AddressLine { get; set; }
    }
}
