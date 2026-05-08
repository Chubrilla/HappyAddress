using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HappyAddress.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AdId { get; set; }
        
        [ValidateNever]
        public User? User { get; set; }
        
        [ValidateNever]
        public Ad? Ad { get; set; }
    }
}
