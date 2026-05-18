using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HappyAddress.Data;
using HappyAddress.Models;
using System.Security.Cryptography;
using HappyAddress.Services;

namespace HappyAddress.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public AccountController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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

        private string GenerateEmailConfirmationCode()
        {
            return RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
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

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ModelState.AddModelError("Email", "Введите email");
            }

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

            bool emailExists = _context.Users.Any(u => u.Email == model.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "Пользователь с таким email уже существует");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string confirmationCode = GenerateEmailConfirmationCode();

            User user = new User
            {
                LastName = model.LastName,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                Email = model.Email.Trim(),
                PhoneNumber = model.PhoneNumber,
                BirthDate = model.BirthDate,

                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),

                IsEmailConfirmed = false,
                EmailConfirmationCode = confirmationCode,
                EmailConfirmationCodeExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            try
            {
                _emailService.SendEmailConfirmationCode(user.Email, confirmationCode);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Не удалось отправить письмо: " + ex.Message);
                return View(model);
            }

            TempData["EmailForConfirmation"] = user.Email;

            return RedirectToAction("ConfirmEmail", "Account", new { email = user.Email });
        }

        [HttpGet]
        public IActionResult ConfirmEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction("Register");
            }

            ViewBag.Email = email;
            ViewBag.TestEmailCode = TempData["TestEmailCode"];

            TempData.Keep("TestEmailCode");

            return View("~/Views/Account/ConfirmEmail.cshtml");
        }

        [HttpPost]
        public IActionResult ConfirmEmail(string email, string code)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("", "Email не найден");
                ViewBag.Email = email;
                return View("~/Views/Account/ConfirmEmail.cshtml");
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("code", "Введите код подтверждения");
                ViewBag.Email = email;
                return View("~/Views/Account/ConfirmEmail.cshtml");
            }

            User user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь не найден");
                ViewBag.Email = email;
                return View("~/Views/Account/ConfirmEmail.cshtml");
            }

            if (user.IsEmailConfirmed)
            {
                TempData["Success"] = "Email уже подтверждён. Теперь можно войти.";
                return RedirectToAction("Login");
            }

            if (!user.EmailConfirmationCodeExpiresAt.HasValue ||
                user.EmailConfirmationCodeExpiresAt.Value < DateTime.UtcNow)
            {
                ModelState.AddModelError("", "Код подтверждения истёк. Зарегистрируйтесь заново или запросите новый код.");
                ViewBag.Email = email;
                return View("~/Views/Account/ConfirmEmail.cshtml");
            }

            if (user.EmailConfirmationCode != code.Trim())
            {
                ModelState.AddModelError("code", "Неверный код подтверждения");
                ViewBag.Email = email;
                return View("~/Views/Account/ConfirmEmail.cshtml");
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationCode = null;
            user.EmailConfirmationCodeExpiresAt = null;

            _context.SaveChanges();

            TempData["Success"] = "Email успешно подтверждён. Теперь можно войти.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult ResendEmailCode(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction("Register");
            }

            User user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                TempData["Error"] = "Пользователь не найден.";
                return RedirectToAction("Register");
            }

            if (user.IsEmailConfirmed)
            {
                TempData["Success"] = "Email уже подтверждён. Теперь можно войти.";
                return RedirectToAction("Login");
            }

            string newCode = GenerateEmailConfirmationCode();

            user.EmailConfirmationCode = newCode;
            user.EmailConfirmationCodeExpiresAt = DateTime.UtcNow.AddMinutes(15);

            _context.SaveChanges();

            _emailService.SendEmailConfirmationCode(user.Email, newCode);

            TempData["Success"] = "Новый код отправлен на email.";

            return RedirectToAction("ConfirmEmail", "Account", new { email = user.Email });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string email = model.Email.Trim();

            User user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь с таким email не найден");
                return View(model);
            }

            bool passwordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

            if (!passwordValid)
            {
                ModelState.AddModelError("", "Неверный пароль");
                return View(model);
            }

            if (!user.IsEmailConfirmed)
            {
                ModelState.AddModelError("", "Подтвердите email перед входом.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}".Trim());
            HttpContext.Session.SetString("UserRole", user.Role);

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

            ProfileViewModel model = new ProfileViewModel
            {
                Id = user.Id,
                LastName = user.LastName,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                Email = user.Email,
                IsEmailConfirmed = user.IsEmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Profile(ProfileViewModel model)
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

            model.PhoneNumber = NormalizePhoneNumber(model.PhoneNumber);

            if (!IsValidRussianPhone(model.PhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "Введите корректный российский номер телефона");
            }

            bool phoneExists = _context.Users.Any(u =>
                u.PhoneNumber == model.PhoneNumber &&
                u.Id != user.Id);

            if (phoneExists)
            {
                ModelState.AddModelError("PhoneNumber", "Этот номер телефона уже используется другим пользователем");
            }

            if (!ModelState.IsValid)
            {
                model.Email = user.Email;
                model.IsEmailConfirmed = user.IsEmailConfirmed;
                model.BirthDate = user.BirthDate;

                return View(model);
            }

            user.LastName = model.LastName;
            user.FirstName = model.FirstName;
            user.MiddleName = model.MiddleName;
            user.PhoneNumber = model.PhoneNumber;

            _context.SaveChanges();

            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}".Trim());

            TempData["Success"] = "Профиль успешно обновлён";

            return RedirectToAction("Profile");
        }
    }
}