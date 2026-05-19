using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HappyAddress.Data;
using HappyAddress.Models;

namespace HappyAddress.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Profile(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            bool isOwner = currentUserId != null && currentUserId.Value == id;

            var adsQuery = _context.Ads.Where(a => a.UserId == id);

            if (!isOwner)
            {
                adsQuery = adsQuery.Where(a => a.Status == "Опубликовано");
            }

            var ads = adsQuery
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            foreach (var ad in ads)
            {
                ad.Images = _context.AdImages
                    .Where(i => i.AdId == ad.Id &&
                        (i.Status == "Опубликовано" ||
                         i.Status == null ||
                         i.Status.Trim() == ""))
                    .ToList();
            }

            var ratings = _context.UserRatings
                .Where(r => r.RatedUserId == id)
                .ToList();

            double averageRating = 0;

            if (ratings.Any())
            {
                averageRating = ratings.Average(r => r.Value);
            }

            bool isSubscribed = false;
            int? currentUserRating = null;

            if (currentUserId != null)
            {
                isSubscribed = _context.UserSubscriptions.Any(s =>
                    s.SubscriberUserId == currentUserId.Value &&
                    s.TargetUserId == id);

                var rating = _context.UserRatings.FirstOrDefault(r =>
                    r.RaterUserId == currentUserId.Value &&
                    r.RatedUserId == id);

                if (rating != null)
                {
                    currentUserRating = rating.Value;
                }
            }

            int subscribersCount = _context.UserSubscriptions
                .Count(s => s.TargetUserId == id);

            var model = new PublicUserProfileViewModel
            {
                User = user,
                Ads = ads,
                PublishedAdsCount = ads.Count,
                AverageRating = averageRating,
                RatingsCount = ratings.Count,
                IsSubscribed = isSubscribed,
                SubscribersCount = subscribersCount,
                CurrentUserRating = currentUserRating,
                IsOwner = isOwner
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Rate(int ratedUserId, int value)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");

            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (currentUserId.Value == ratedUserId)
            {
                TempData["Error"] = "Нельзя оценить самого себя";
                return RedirectToAction("Profile", new { id = ratedUserId });
            }

            if (value < 1 || value > 5)
            {
                TempData["Error"] = "Оценка должна быть от 1 до 5";
                return RedirectToAction("Profile", new { id = ratedUserId });
            }

            var rating = _context.UserRatings.FirstOrDefault(r =>
                r.RaterUserId == currentUserId.Value &&
                r.RatedUserId == ratedUserId);

            if (rating == null)
            {
                rating = new UserRating
                {
                    RaterUserId = currentUserId.Value,
                    RatedUserId = ratedUserId,
                    Value = value
                };

                _context.UserRatings.Add(rating);
            }
            else
            {
                rating.Value = value;
            }

            _context.SaveChanges();

            TempData["Success"] = "Оценка сохранена";
            return RedirectToAction("Profile", new { id = ratedUserId });
        }

        [HttpPost]
        public IActionResult ToggleSubscription(int targetUserId)
        {
            int? currentUserId = HttpContext.Session.GetInt32("UserId");

            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (currentUserId.Value == targetUserId)
            {
                TempData["Error"] = "Нельзя подписаться на самого себя";
                return RedirectToAction("Profile", new { id = targetUserId });
            }

            var subscription = _context.UserSubscriptions.FirstOrDefault(s =>
                s.SubscriberUserId == currentUserId.Value &&
                s.TargetUserId == targetUserId);

            if (subscription == null)
            {
                subscription = new UserSubscription
                {
                    SubscriberUserId = currentUserId.Value,
                    TargetUserId = targetUserId
                };

                _context.UserSubscriptions.Add(subscription);
                TempData["Success"] = "Вы подписались на пользователя";
            }
            else
            {
                _context.UserSubscriptions.Remove(subscription);
                TempData["Success"] = "Вы отписались от пользователя";
            }

            _context.SaveChanges();

            return RedirectToAction("Profile", new { id = targetUserId });
        }
    }
}