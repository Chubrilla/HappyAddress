using System;
using System.ComponentModel.DataAnnotations;

namespace HappyAddress.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите фамилию")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Введите email")]
        [EmailAddress(ErrorMessage = "Введите корректный email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите номер телефона")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Введите дату рождения")]
        public DateTime BirthDate { get; set; }

    }
}