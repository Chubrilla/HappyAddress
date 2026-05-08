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

        [Required(ErrorMessage = "Введите номер телефона")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Введите дату рождения")]
        public DateTime BirthDate { get; set; }
    }
}