using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HappyAddress.Data;
using HappyAddress.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HappyAddress.Controllers
{
    public class AdsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private const int PageSize = 15;

        public AdsController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        private static string FormatTitleNumber(double value)
        {
            return $"{value:0.#}";
        }

        private static string GetDealVerb(string? dealType)
        {
            return dealType switch
            {
                "Продажа" => "Продаётся",
                "Посуточная аренда" => "Сдаётся посуточно",
                "Долгосрочная аренда" => "Сдаётся",
                _ => "Объявление"
            };
        }

        private static string? GetTitleAreaText(Ad ad)
        {
            if (ad.PropertyType == "Участок")
            {
                return ad.LandArea.HasValue
                    ? $"{FormatTitleNumber(ad.LandArea.Value)} сот."
                    : null;
            }

            var area = ad.TotalArea ?? ad.LivingArea ?? ad.KitchenArea;

            return area.HasValue
                ? $"{FormatTitleNumber(area.Value)} м²"
                : null;
        }

        private static string BuildGeneratedTitle(Ad ad)
        {
            var titleParts = new List<string>
            {
                GetDealVerb(ad.DealType)
            };

            if (!string.IsNullOrWhiteSpace(ad.PropertyType))
            {
                titleParts.Add(ad.PropertyType.ToLowerInvariant());
            }

            var areaText = GetTitleAreaText(ad);

            if (!string.IsNullOrWhiteSpace(areaText))
            {
                titleParts.Add(areaText);
            }

            return string.Join(" ", titleParts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(Ad model, List<IFormFile>? imageFiles)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            model.Title = BuildGeneratedTitle(model);
            ModelState.Remove(nameof(model.Title));

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                ModelState.AddModelError("Description", "Введите описание");
            }

            if (model.Price <= 0)
            {
                ModelState.AddModelError("Price", "Цена должна быть больше 0");
            }

            if (string.IsNullOrWhiteSpace(model.City))
            {
                ModelState.AddModelError("City", "Введите город");
            }

            if (string.IsNullOrWhiteSpace(model.Address))
            {
                ModelState.AddModelError("Address", "Введите адрес");
            }

            if (string.IsNullOrWhiteSpace(model.DealType))
            {
                ModelState.AddModelError("DealType", "Выберите тип сделки");
            }

            if (string.IsNullOrWhiteSpace(model.PropertyType))
            {
                ModelState.AddModelError("PropertyType", "Выберите тип недвижимости");
            }

            if (model.Latitude == null || model.Longitude == null)
            {
                ModelState.AddModelError("", "Выберите точку на карте");
            }

            if (imageFiles != null && imageFiles.Count > 30)
            {
                ModelState.AddModelError("", "Можно загрузить не более 30 фотографий");
            }

            if (imageFiles != null)
            {
                foreach (var imageFile in imageFiles)
                {
                    string extention = Path.GetExtension(imageFile.FileName).ToLower();

                    if (extention != ".jpg" && extention != ".jpeg" && extention != ".png")
                    {
                        ModelState.AddModelError("", "можно загружать фотографии формата JPG или PNG");
                    }

                    if (imageFile.Length > 10 * 1024 * 1024)
                    {
                        ModelState.AddModelError("", "Размер одного фото не должен превышать 10МБ");
                    }
                }
            }

            if (model.DealType == "Посуточная аренда" && (model.PropertyType == "Гараж" || model.PropertyType == "Участок"))
            {
                ModelState.AddModelError("PropertyType", "Для посуточной аренды нельзя выбрать участок или гараж.");
            }

            if (model.PropertyType == "Квартира" &&
                model.Floor.HasValue &&
                model.TotalFloors.HasValue &&
                model.Floor.Value > model.TotalFloors.Value)
            {
                ModelState.AddModelError("Floor", "Этаж квартиры не может быть больше общего количества этажей в доме.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.UserId = userId.Value;
            model.CreatedAt = DateTime.UtcNow;

            _context.Ads.Add(model);
            _context.SaveChanges();

            if (imageFiles != null && imageFiles.Count > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                foreach (var imageFile in imageFiles)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            imageFile.CopyTo(stream);
                        }

                        AdImage adImage = new AdImage
                        {
                            AdId = model.Id,
                            ImagePath = "/uploads/" + uniqueFileName,
                            Status = "Опубликовано"
                        };

                        _context.AdImages.Add(adImage);
                    }
                }

                _context.SaveChanges();
            }
            
            TempData["Success"] = "Объявление успешно создано";
            return RedirectToAction("Profile", "Users", new { id = userId.Value });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var ad = _context.Ads.FirstOrDefault(a => a.Id == id);

            if (ad == null)
            {
                return NotFound();
            }

            ad.Images = _context.AdImages
                .Where(i => i.AdId == ad.Id &&
                    (i.Status == "Опубликовано" ||
                     i.Status == null ||
                     i.Status.Trim() == ""))
                .ToList();

            int? userId = HttpContext.Session.GetInt32("UserId");

            bool isFavorite = false;

            if (userId != null)
            {
                isFavorite = _context.Favorites
                    .Any(f => f.UserId == userId.Value && f.AdId == ad.Id);
            }

            var seller = _context.Users.FirstOrDefault(u => u.Id == ad.UserId);

            int sellerAdsCount = _context.Ads
                .Count(a => a.UserId == ad.UserId && a.Status == "Опубликовано");

            ViewBag.IsFavorite = isFavorite;
            ViewBag.Seller = seller;
            ViewBag.SellerAdsCount = sellerAdsCount;

            return View(ad);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var ad = _context.Ads.FirstOrDefault(a => a.Id == id && a.UserId == userId.Value);

            if (ad == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ad.Images = _context.AdImages
                .Where(i => i.AdId == ad.Id)
                .ToList();

            return View(ad);
        }

        [HttpPost]
        public IActionResult Edit(Ad model, List<IFormFile>? imageFiles)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var ad = _context.Ads.FirstOrDefault(a => a.Id == model.Id && a.UserId == userId.Value);

            if (ad == null)
            {
                return NotFound();
            }

            model.Title = BuildGeneratedTitle(model);
            ModelState.Remove(nameof(model.Title));

            if (model.PropertyType == "Квартира" &&
                model.Floor.HasValue &&
                model.TotalFloors.HasValue &&
                model.Floor.Value > model.TotalFloors.Value)
            {
                ModelState.AddModelError("Floor", "Этаж квартиры не может быть больше общего количества этажей в доме.");
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                ModelState.AddModelError("Description", "Введите описание");
            }

            if (model.Price <= 0)
            {
                ModelState.AddModelError("Price", "Цена должна быть больше 0");
            }

            if (string.IsNullOrWhiteSpace(model.City))
            {
                ModelState.AddModelError("City", "Введите город");
            }

            if (string.IsNullOrWhiteSpace(model.Address))
            {
                ModelState.AddModelError("Address", "Введите адрес");
            }

            if (string.IsNullOrWhiteSpace(model.DealType))
            {
                ModelState.AddModelError("DealType", "Выберите тип сделки");
            }

            if (string.IsNullOrWhiteSpace(model.PropertyType))
            {
                ModelState.AddModelError("PropertyType", "Выберите тип недвижимости");
            }

            if ((model.PropertyType == "Дом" ||
                model.PropertyType == "Коттедж" ||
                model.PropertyType == "Таунхаус") &&
                (!model.LandArea.HasValue || model.LandArea.Value <= 0))
            {
                ModelState.AddModelError("LandArea", "Введите площадь участка в сотках");
            }

            if (imageFiles != null && imageFiles.Count > 30)
            {
                ModelState.AddModelError("", "Можно загрузить не более 30 фотографий");
            }

            if (imageFiles != null)
            {
                foreach (var imageFile in imageFiles)
                {
                    string extension = Path.GetExtension(imageFile.FileName).ToLower();

                    if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                    {
                        ModelState.AddModelError("", "Можно загружать только JPG или PNG");
                    }

                    if (imageFile.Length > 10 * 1024 * 1024)
                    {
                        ModelState.AddModelError("", "Размер одного фото не должен превышать 10 МБ");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                model.Images = _context.AdImages.Where(i => i.AdId == model.Id).ToList();
                return View(model);
            }

            bool mainAdChanged =
                ad.Title != model.Title ||
                ad.Description != model.Description ||
                ad.PhoneNumber != model.PhoneNumber ||
                ad.Price != model.Price ||
                ad.City != model.City ||
                ad.Address != model.Address ||
                ad.DealType != model.DealType ||
                ad.PropertyType != model.PropertyType ||
                ad.TotalArea != model.TotalArea ||
                ad.Rooms != model.Rooms ||
                ad.Bedrooms != model.Bedrooms ||
                ad.Floor != model.Floor ||
                ad.TotalFloors != model.TotalFloors ||
                ad.BuildYear != model.BuildYear ||
                ad.HouseType != model.HouseType ||
                ad.HouseMaterial != model.HouseMaterial ||
                ad.HouseCondition != model.HouseCondition ||
                ad.LandArea != model.LandArea ||
                ad.LandCategory != model.LandCategory ||
                ad.LandStatus != model.LandStatus ||
                ad.LivingArea != model.LivingArea ||
                ad.KitchenArea != model.KitchenArea ||
                ad.Renovation != model.Renovation ||
                ad.BuildingType != model.BuildingType ||
                ad.BathroomType != model.BathroomType ||
                ad.BalconyType != model.BalconyType ||
                ad.Sewerage != model.Sewerage ||
                ad.WaterSupply != model.WaterSupply ||
                ad.Gas != model.Gas ||
                ad.Heating != model.Heating ||
                ad.Electricity != model.Electricity ||
                ad.HasGarage != model.HasGarage ||
                ad.HasTerrace != model.HasTerrace ||
                ad.HasCellar != model.HasCellar ||
                ad.HasPool != model.HasPool ||
                ad.HasBathhouse != model.HasBathhouse ||
                ad.HasSecurity != model.HasSecurity ||
                ad.HasParking != model.HasParking ||
                ad.HasElevator != model.HasElevator ||
                ad.MortgageAllowed != model.MortgageAllowed ||
                ad.Deposit != model.Deposit ||
                ad.Prepayment != model.Prepayment ||
                ad.AllowChildren != model.AllowChildren ||
                ad.AllowPets != model.AllowPets ||
                ad.HasFurniture != model.HasFurniture ||
                ad.HasAppliances != model.HasAppliances ||
                ad.GuestsCount != model.GuestsCount ||
                ad.SleepingPlaces != model.SleepingPlaces ||
                ad.AvailableFrom != model.AvailableFrom ||
                ad.AvailableTo != model.AvailableTo ||
                ad.HasWifi != model.HasWifi ||
                ad.HasAirConditioner != model.HasAirConditioner ||
                ad.HasKitchen != model.HasKitchen ||
                ad.HasTv != model.HasTv ||
                ad.HasWashingMachine != model.HasWashingMachine ||
                ad.HasBedLinen != model.HasBedLinen ||
                ad.GarageType != model.GarageType ||
                ad.GarageStatus != model.GarageStatus;

            ad.Title = model.Title;
            ad.Description = model.Description;
            ad.PhoneNumber = model.PhoneNumber;
            ad.Price = model.Price;
            ad.City = model.City;
            ad.Address = model.Address;
            ad.DealType = model.DealType;
            ad.PropertyType = model.PropertyType;

            // Общие параметры объекта
            ad.TotalArea = model.TotalArea;
            ad.Rooms = model.Rooms;
            ad.Bedrooms = model.Bedrooms;
            ad.Floor = model.Floor;
            ad.TotalFloors = model.TotalFloors;
            ad.BuildYear = model.BuildYear;

            // Дом / дача / коттедж
            ad.HouseType = model.HouseType;
            ad.HouseMaterial = model.HouseMaterial;
            ad.HouseCondition = model.HouseCondition;

            // Участок
            ad.LandArea = model.LandArea;
            ad.LandCategory = model.LandCategory;
            ad.LandStatus = model.LandStatus;

            // Квартира
            ad.LivingArea = model.LivingArea;
            ad.KitchenArea = model.KitchenArea;
            ad.Renovation = model.Renovation;
            ad.BuildingType = model.BuildingType;
            ad.BathroomType = model.BathroomType;
            ad.BalconyType = model.BalconyType;

            // Коммуникации
            ad.Sewerage = model.Sewerage;
            ad.WaterSupply = model.WaterSupply;
            ad.Gas = model.Gas;
            ad.Heating = model.Heating;
            ad.Electricity = model.Electricity;

            // Дополнительные удобства
            ad.HasGarage = model.HasGarage;
            ad.HasTerrace = model.HasTerrace;
            ad.HasCellar = model.HasCellar;
            ad.HasPool = model.HasPool;
            ad.HasBathhouse = model.HasBathhouse;
            ad.HasSecurity = model.HasSecurity;
            ad.HasParking = model.HasParking;
            ad.HasElevator = model.HasElevator;

            // Условия сделки
            ad.MortgageAllowed = model.MortgageAllowed;

            // Аренда
            ad.Deposit = model.Deposit;
            ad.Prepayment = model.Prepayment;
            ad.AllowChildren = model.AllowChildren;
            ad.AllowPets = model.AllowPets;
            ad.HasFurniture = model.HasFurniture;
            ad.HasAppliances = model.HasAppliances;

            // Посуточная аренда
            ad.GuestsCount = model.GuestsCount;
            ad.SleepingPlaces = model.SleepingPlaces;
            ad.AvailableFrom = model.AvailableFrom;
            ad.AvailableTo = model.AvailableTo;

            ad.HasWifi = model.HasWifi;
            ad.HasAirConditioner = model.HasAirConditioner;
            ad.HasKitchen = model.HasKitchen;
            ad.HasTv = model.HasTv;
            ad.HasWashingMachine = model.HasWashingMachine;
            ad.HasBedLinen = model.HasBedLinen;

            // Гараж / машино-место
            ad.GarageType = model.GarageType;
            ad.GarageStatus = model.GarageStatus;

            if (mainAdChanged)
            {
                ad.Status = "На модерации";
                ad.RejectReason = null;
            }

            _context.SaveChanges();

            if (imageFiles != null && imageFiles.Count > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var imageFile in imageFiles)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            imageFile.CopyTo(stream);
                        }

                        AdImage adImage = new AdImage
                        {
                            AdId = ad.Id,
                            ImagePath = "/uploads/" + uniqueFileName,
                            Status = "На модерации"
                        };

                        _context.AdImages.Add(adImage);
                    }
                }

                _context.SaveChanges();
            }
            
            if (mainAdChanged)
            {
                TempData["Success"] = "Объявление обновлено и отправлено на модерацию";
            }
            else if (imageFiles != null && imageFiles.Count > 0)
            {
                TempData["Success"] = "Фото добавлены и отправлены на модерацию";
            }
            else
            {
                TempData["Success"] = "Изменения сохранены";
            }
            
            return RedirectToAction("Profile", "Users", new { id = userId.Value });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var ad = _context.Ads.FirstOrDefault(a => a.Id == id && a.UserId == userId.Value);

            if (ad == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var images = _context.AdImages.Where(i => i.AdId == ad.Id).ToList();

            foreach (var image in images)
            {
                string fullPath = Path.Combine(_environment.WebRootPath, image.ImagePath.TrimStart('/'));

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            _context.AdImages.RemoveRange(images);

            _context.Ads.Remove(ad);
            _context.SaveChanges();

            TempData["Success"] = "Объявление удалено";
            return RedirectToAction("Profile", "Users", new { id = userId.Value });
        }

        [HttpPost]
        public IActionResult AddToFavorites(int adId, string? returnUrl = null)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            bool exists = _context.Favorites
                .Any(f => f.UserId == userId.Value && f.AdId == adId);

            if (!exists)
            {
                Favorite favorite = new Favorite
                {
                    UserId = userId.Value,
                    AdId = adId
                };

                _context.Favorites.Add(favorite);
                _context.SaveChanges();

                TempData["Success"] = "Добавлено в избранное";
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult RemoveFromFavorites(int adId, string? returnUrl = null)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var favorite = _context.Favorites
                .FirstOrDefault(f => f.UserId == userId.Value && f.AdId == adId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();
                TempData["Success"] = "Объявление удалено из избранного";
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Favorites");
        }

        [HttpGet]
        public IActionResult Favorites(int page = 1)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var query = _context.Favorites
                .Where(f => f.UserId == userId.Value)
                .Join(_context.Ads,
                      favorite => favorite.AdId,
                      ad => ad.Id,
                      (favorite, ad) => ad)
                .OrderByDescending(a => a.CreatedAt);

            int totalAds = query.Count();
            int totalPages = (int)System.Math.Ceiling((double)totalAds / PageSize);

            var favoriteAds = query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            foreach (var ad in favoriteAds)
            {
                ad.Images = _context.AdImages.Where(i => i.AdId == ad.Id).ToList();
            }

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(favoriteAds);            
        }

        [HttpPost]
        public IActionResult DeleteImage(int imageId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var image = _context.AdImages
                .FirstOrDefault(i => i.Id == imageId);

            if (image == null)
            {
                TempData["Error"] = "Фото не найдено";
                return RedirectToAction("Profile", "Users", new { id = userId.Value });
            }

            var ad = _context.Ads
                .FirstOrDefault(a => a.Id == image.AdId && a.UserId == userId.Value);

            if (ad == null)
            {
                return NotFound();
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

            TempData["Success"] = "Фото удалено";
            return RedirectToAction("Edit", new { id = ad.Id });
        }
    }
}
