using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _011Global.Shared.DbContexts.AddressDbContext
{
    public class GeneralAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressID { get; set; }
        public string CountryIso2 { get; set; }
        public string StateIso2 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
