using System.ComponentModel.DataAnnotations;

namespace HappyAddress.Models
{
    public class ProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required(ErrorMessage = "Введите номер телефона")]
        public string PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public string? AvatarPath { get; set; }
    }
}