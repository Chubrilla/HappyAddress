using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HappyAddress.Data;
using HappyAddress.Models;

namespace HappyAddress.Controllers
{
    public class ChatController : Controller
    {
        private readonly AppDbContext _context;

        public ChatController(AppDbContext context)
        {
            _context = context;
        }

        private int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        [HttpGet]
        public IActionResult Index()
        {
            int? userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var chats = _context.Chats
                .Where(c => c.SellerUserId == userId.Value || c.BuyerUserId == userId.Value)
                .OrderByDescending(c => c.UpdatedAt)
                .ToList();

            var model = new List<ChatListItemViewModel>();

            foreach (var chat in chats)
            {
                var ad = _context.Ads.FirstOrDefault(a => a.Id == chat.AdId);

                if (ad == null)
                {
                    continue;
                }

                int otherUserId = chat.SellerUserId == userId.Value
                    ? chat.BuyerUserId
                    : chat.SellerUserId;

                var otherUser = _context.Users.FirstOrDefault(u => u.Id == otherUserId);

                if (otherUser == null)
                {
                    continue;
                }

                var lastMessage = _context.ChatMessages
                    .Where(m => m.ChatId == chat.Id)
                    .OrderByDescending(m => m.CreatedAt)
                    .FirstOrDefault();

                int unreadCount = _context.ChatMessages.Count(m =>
                    m.ChatId == chat.Id &&
                    m.SenderUserId != userId.Value &&
                    !m.IsRead);

                string? adImagePath = _context.AdImages
                    .Where(i => i.AdId == ad.Id &&
                        (i.Status == "Опубликовано" ||
                         i.Status == null ||
                         i.Status.Trim() == ""))
                    .Select(i => i.ImagePath)
                    .FirstOrDefault();

                model.Add(new ChatListItemViewModel
                {
                    Chat = chat,
                    Ad = ad,
                    OtherUser = otherUser,
                    LastMessage = lastMessage,
                    UnreadCount = unreadCount,
                    AdImagePath = adImagePath
                });
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Start(int adId)
        {
            int? userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var ad = _context.Ads.FirstOrDefault(a => a.Id == adId);

            if (ad == null)
            {
                return NotFound();
            }

            if (ad.UserId == userId.Value)
            {
                TempData["Error"] = "Нельзя написать самому себе";
                return RedirectToAction("Details", "Ads", new { id = adId });
            }

            var chat = _context.Chats.FirstOrDefault(c =>
                c.AdId == ad.Id &&
                c.SellerUserId == ad.UserId &&
                c.BuyerUserId == userId.Value);

            if (chat == null)
            {
                chat = new Chat
                {
                    AdId = ad.Id,
                    SellerUserId = ad.UserId,
                    BuyerUserId = userId.Value,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Chats.Add(chat);
                _context.SaveChanges();
            }

            return RedirectToAction("Details", new { id = chat.Id });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var chat = _context.Chats.FirstOrDefault(c =>
                c.Id == id &&
                (c.BuyerUserId == userId.Value || c.SellerUserId == userId.Value));

            if (chat == null)
            {
                return RedirectToAction("Index");
            }

            var messages = _context.ChatMessages
                .Where(m => m.ChatId == chat.Id)
                .OrderBy(m => m.CreatedAt)
                .ToList();

            foreach (var message in messages)
            {
                if (message.SenderUserId != userId.Value)
                {
                    message.IsRead = true;
                }
            }

            _context.SaveChanges();

            var ad = _context.Ads.FirstOrDefault(a => a.Id == chat.AdId);

            if (ad == null)
            {
                TempData["Error"] = "Объявление для этого чата не найдено";
                return RedirectToAction("Index");
            }

            var currentUser = _context.Users.FirstOrDefault(u => u.Id == userId.Value);

            if (currentUser == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account");
            }

            int otherUserId = chat.SellerUserId == userId.Value
                ? chat.BuyerUserId
                : chat.SellerUserId;

            var otherUser = _context.Users.FirstOrDefault(u => u.Id == otherUserId);

            if (otherUser == null)
            {
                TempData["Error"] = "Собеседник не найден";
                return RedirectToAction("Index");
            }

            string? adImagePath = _context.AdImages
                .Where(i => i.AdId == ad.Id &&
                    (i.Status == "Опубликовано" ||
                     i.Status == null ||
                     i.Status.Trim() == ""))
                .Select(i => i.ImagePath)
                .FirstOrDefault();

            var model = new ChatDetailsViewModel
            {
                Chat = chat,
                Ad = ad,
                CurrentUser = currentUser,
                OtherUser = otherUser,
                Messages = messages,
                AdImagePath = adImagePath,
                CurrentUserId = userId.Value
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Send(int chatId, string text)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Вы не авторизованы"
                });
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return Json(new
                {
                    success = false,
                    message = "Сообщение не может быть пустым"
                });
            }

            var chat = _context.Chats.FirstOrDefault(c =>
                c.Id == chatId &&
                (c.BuyerUserId == userId.Value || c.SellerUserId == userId.Value));

            if (chat == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Чат не найден"
                });
            }

            var message = new ChatMessage
            {
                ChatId = chat.Id,
                SenderUserId = userId.Value,
                Text = text.Trim(),
                CreatedAt = DateTime.Now,
                IsRead = false
            };

            _context.ChatMessages.Add(message);

            chat.UpdatedAt = DateTime.Now;

            _context.SaveChanges();

            return Json(new
            {
                success = true,
                text = message.Text,
                createdAt = message.CreatedAt.ToString("dd.MM.yyyy HH:mm")
            });
        }
    }
}
