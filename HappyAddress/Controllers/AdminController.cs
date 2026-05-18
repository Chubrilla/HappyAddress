using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HappyAddress.Data;
using HappyAddress.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace HappyAddress.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Users()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var users = _context.Users.OrderBy(u => u.Id).ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult Ads()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var ads = _context.Ads
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            foreach (var ad in ads)
            {
                ad.Images = _context.AdImages
                    .Where(i => i.AdId == ad.Id)
                    .ToList();
            }

            return View(ads);
        }

        [HttpPost]
        public IActionResult ChangeStatus(int id, string status, string? rejectReason)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var ad = _context.Ads.FirstOrDefault(a => a.Id == id);

            if (ad == null)
            {
                return RedirectToAction("Ads");
            }

            var allowedStatuses = new[] { "Опубликовано", "Отклонено" };

            if (!allowedStatuses.Contains(status))
            {
                return RedirectToAction("Ads");
            }

            if (status == "Отклонено")
            {
                if (string.IsNullOrWhiteSpace(rejectReason))
                {
                    TempData["Error"] = "Укажите причину отклонения объявления";
                    return RedirectToAction("Ads");
                }

                ad.Status = "Отклонено";
                ad.RejectReason = rejectReason.Trim();
            }

            if (status == "Опубликовано")
            {
                ad.Status = "Опубликовано";
                ad.RejectReason = null;
            }

            _context.SaveChanges();

            TempData["Success"] = "Статус объявления изменён";
            return RedirectToAction("Ads");
        }

        [HttpPost]
        public IActionResult DeleteAd(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var ad = _context.Ads.FirstOrDefault(a => a.Id == id);

            if (ad == null)
            {
                return RedirectToAction("Ads");
            }

            var images = _context.AdImages.Where(i => i.AdId == ad.Id).ToList();

            foreach (var image in images)
            {
                string FullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImagePath.TrimStart('/'));
                
                if (System.IO.File.Exists(FullPath))
                {
                    System.IO.File.Delete(FullPath);
                }
            }

            _context.AdImages.RemoveRange(images);

            _context.Ads.Remove(ad);
            _context.SaveChanges();

            TempData["Success"] = "Объявление удалено администратором";
            return RedirectToAction("Ads");
        }
       
        [HttpPost]
        public IActionResult ApproveImage(int imageId)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var image = _context.AdImages.FirstOrDefault(i => i.Id == imageId);

            if (image == null)
            {
                TempData["Error"] = "Фото не найдено";
                return RedirectToAction("Ads");
            }

            image.Status = "Опубликовано";
            image.RejectReason = null;

            _context.SaveChanges();

            TempData["Success"] = "Фото опубликовано";
            return RedirectToAction("Ads");
        }

        [HttpPost]
        public IActionResult RejectImage(int imageId, string? rejectReason)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var image = _context.AdImages.FirstOrDefault(i => i.Id == imageId);

            if (image == null)
            {
                TempData["Error"] = "Фото не найдено";
                return RedirectToAction("Ads");
            }

            string filePath = Path.Combine(
                _environment.WebRootPath,
                image.ImagePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.AdImages.Remove(image);
            _context.SaveChanges();

            TempData["Success"] = "Фото отклонено и удалено";
            return RedirectToAction("Ads");
        }
    }
}