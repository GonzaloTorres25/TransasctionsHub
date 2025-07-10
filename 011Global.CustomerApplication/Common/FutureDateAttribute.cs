using System.ComponentModel.DataAnnotations;

namespace _011Global.CustomerApplication.Common
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value) =>        
            value is DateTime date && date >= DateTime.Today;
    }
}
