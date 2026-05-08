using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HappyAddress.Data;
using HappyAddress.Models;

namespace HappyAddress.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        private string NormalizePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return "";

            // Оставляем только цифры
            string digitsOnly = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // Если номер начинается с 8 и длина 11, заменяем 8 на 7
            if (digitsOnly.Length == 11 && digitsOnly.StartsWith("8"))
            {
                digitsOnly = "7" + digitsOnly.Substring(1);
            }

            // Если номер уже начинается с 7 и длина 11 - оставляем
            if (digitsOnly.Length == 11 && digitsOnly.StartsWith("7"))
            {
                return digitsOnly;
            }

            // Иначе возвращаем как есть, чтобы потом валидация показала ошибку
            return digitsOnly;
        }

        private bool IsValidRussianPhone(string phoneNumber)
        {
            return !string.IsNullOrWhiteSpace(phoneNumber)
                   && phoneNumber.Length == 11
                   && phoneNumber.StartsWith("7")
                   && phoneNumber.All(char.IsDigit);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            model.PhoneNumber = NormalizePhoneNumber(model.PhoneNumber);

            if (model.BirthDate > DateTime.Now.AddYears(-18))
            {
                ModelState.AddModelError("BirthDate", "Пользователь должен быть старше 18 лет");
            }

            if (!IsValidRussianPhone(model.PhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "Введите корректный российский номер телефона");
            }

            bool phoneExists = _context.Users.Any(u => u.PhoneNumber == model.PhoneNumber);
            if (phoneExists)
            {
                ModelState.AddModelError("PhoneNumber", "Пользователь с таким номером уже существует");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = new User
            {
                LastName = model.LastName,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                PhoneNumber = model.PhoneNumber,
                BirthDate = model.BirthDate,
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            model.PhoneNumber = NormalizePhoneNumber(model.PhoneNumber);

            if (!IsValidRussianPhone(model.PhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "Введите корректный российский номер телефона");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = _context.Users.FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber);

            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь с таким номером не найден");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}".Trim());

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            User user = _context.Users.FirstOrDefault(u => u.Id == userId.Value);

            if (user == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }
    }
}