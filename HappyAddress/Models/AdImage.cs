using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HappyAddress.Models
{
    public class AdImage
    {
        public int Id { get; set; }
        public int AdId { get; set; }
        public string ImagePath { get; set; }

        [ValidateNever]
        public Ad? Ad { get; set; }
    }
}
