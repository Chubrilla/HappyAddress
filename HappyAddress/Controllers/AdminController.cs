using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HappyAddress.Data;
using HappyAddress.Models;

namespace HappyAddress.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return false;
            }

            User? user = _context.Users.FirstOrDefault(u => u.Id == userId.Value);

            return user != null && user.Role == "Admin";
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

            var allowedStatuses = new[] { "На модерации", "Опубликовано", "Отклонено" };

            if (!allowedStatuses.Contains(status))
            {
                return RedirectToAction("Ads");
            }

            ad.Status = status;

            if (status == "Отклонено")
            {
                ad.RejectReason = rejectReason;
            }
            else
            {
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
    }
}