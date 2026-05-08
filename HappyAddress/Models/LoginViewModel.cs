using System.ComponentModel.DataAnnotations;

namespace HappyAddress.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите номер телефона")]
        public string PhoneNumber { get; set; }
    }
}