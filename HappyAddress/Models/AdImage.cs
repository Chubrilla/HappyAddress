using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HappyAddress.Models
{
    public class AdImage
    {
        public int Id { get; set; }

        public int AdId { get; set; }

        public string ImagePath { get; set; } = string.Empty;

        public string Status { get; set; } = "Опубликовано";

        public string? RejectReason { get; set; }

        [ValidateNever]
        public Ad? Ad { get; set; }
    }
}