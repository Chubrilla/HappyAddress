using Microsoft.AspNetCore.Mvc;
using HappyAddress.Data;
using HappyAddress.Models;
using System;
using System.Linq;

namespace HappyAddress.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 15;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(
            string searchText,
            string city,
            string dealType,
            string propertyType,
            decimal? priceFrom,
            decimal? priceTo,
            string sortBy,
            string searchMode,

            // Параметры для дома / коттеджа / таунхауса
            double? totalAreaFrom,
            double? totalAreaTo,
            int? bedroomsFrom,
            int? bedroomsTo,
            int? totalFloorsFrom,
            int? totalFloorsTo,
            int? buildYearFrom,
            int? buildYearTo,
            string houseMaterial,
            string houseCondition,
            string sewerage,
            string waterSupply,
            string gas,
            string heating,
            bool hasGarage,
            bool hasTerrace,
            bool hasPool,
            bool hasBathhouse,
            bool hasSecurity,

            // Параметры для квартиры
            string roomsFilter,
            int? floorFrom,
            int? floorTo,
            double? livingAreaFrom,
            double? livingAreaTo,
            double? kitchenAreaFrom,
            double? kitchenAreaTo,
            string renovation,
            string buildingType,
            string bathroomType,
            string balconyType,
            bool hasElevator,
            bool hasParking,

            //Параметры для участка
            double? landAreaFrom,
            double? landAreaTo,
            string landCategory,
            string landStatus,
            string electricity,

            // Гараж
            string garageType,
            string garageStatus,

            decimal? depositFrom,
            decimal? depositTo,
            string prepayment,

            int? guestsCountFrom,
            int? sleepingPlacesFrom,
            DateTime? availableFrom,
            DateTime? availableTo,
            bool hasWifi,
            bool hasAirConditioner,
            bool hasKitchen,
            bool hasTv,
            bool hasWashingMachine,
            bool hasBedLinen,
            bool hasFurniture,
            bool hasAppliances,
            bool allowChildren,
            bool allowPets,

            int page = 1)
        {
            var query = _context.Ads
                .Where(a => a.Status == "Опубликовано")
                .AsQueryable();

            // Убираем лишние пробелы
            searchText = searchText?.Trim();
            city = city?.Trim();
            dealType = dealType?.Trim();
            propertyType = propertyType?.Trim();
            sortBy = sortBy?.Trim();

            // Дом / коттедж / таунхаус
            houseMaterial = houseMaterial?.Trim();
            houseCondition = houseCondition?.Trim();
            sewerage = sewerage?.Trim();
            waterSupply = waterSupply?.Trim();
            gas = gas?.Trim();
            heating = heating?.Trim();

            //Квартира
            roomsFilter = roomsFilter?.Trim();
            renovation = renovation?.Trim();
            buildingType = buildingType?.Trim();
            bathroomType = bathroomType?.Trim();
            balconyType = balconyType?.Trim();

            //Участок
            landCategory = landCategory?.Trim();
            landStatus = landStatus?.Trim();
            electricity = electricity?.Trim();

            prepayment = prepayment?.Trim();

            //Гараж
            garageType = garageType?.Trim();
            garageStatus = garageStatus?.Trim();

            if (searchMode == "text")
            {
                city = null;
                dealType = null;
                propertyType = null;
                priceFrom = null;
                priceTo = null;
                sortBy = null;

                totalAreaFrom = null;
                totalAreaTo = null;
                bedroomsFrom = null;
                bedroomsTo = null;
                totalFloorsFrom = null;
                totalFloorsTo = null;
                buildYearFrom = null;
                buildYearTo = null;
                houseMaterial = null;
                houseCondition = null;
                sewerage = null;
                waterSupply = null;
                gas = null;
                heating = null;
                hasGarage = false;
                hasTerrace = false;
                hasPool = false;
                hasBathhouse = false;
                hasSecurity = false;

                roomsFilter = null;
                floorFrom = null;
                floorTo = null;
                livingAreaFrom = null;
                livingAreaTo = null;
                kitchenAreaFrom = null;
                kitchenAreaTo = null;
                renovation = null;
                buildingType = null;
                bathroomType = null;
                balconyType = null;
                hasElevator = false;
                hasParking = false;

                landAreaFrom = null;
                landAreaTo = null;
                landCategory = null;
                landStatus = null;
                electricity = null;

                garageType = null;
                garageStatus = null;

                depositFrom = null;
                depositTo = null;
                prepayment = null;

                guestsCountFrom = null;
                sleepingPlacesFrom = null;
                availableFrom = null;
                availableTo = null;

                hasWifi = false;
                hasAirConditioner = false;
                hasKitchen = false;
                hasTv = false;
                hasWashingMachine = false;
                hasBedLinen = false;
                hasFurniture = false;
                hasAppliances = false;
                allowChildren = false;
                allowPets = false;
            }
            // Умный поиск:
            // "купить квартиру" -> dealType = "Продажа", propertyType = "Квартира"
            // "снять квартиру" -> dealType = "Долгосрочная аренда", propertyType = "Квартира"
            // "снять посуточно" -> dealType = "Посуточная аренда"
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var normalizedSearch = searchText.ToLower();

                // Определяем тип сделки из текста поиска.
                // Если пользователь ввёл новую фразу, она должна перезаписать старый фильтр.
                if (normalizedSearch.Contains("купить") ||
                    normalizedSearch.Contains("куплю") ||
                    normalizedSearch.Contains("покупка") ||
                    normalizedSearch.Contains("продажа"))
                {
                    dealType = "Продажа";
                }
                else if (normalizedSearch.Contains("посуточно") ||
                         normalizedSearch.Contains("посуточная") ||
                         normalizedSearch.Contains("сутки"))
                {
                    dealType = "Посуточная аренда";
                }
                else if (normalizedSearch.Contains("снять") ||
                         normalizedSearch.Contains("сниму") ||
                         normalizedSearch.Contains("аренда") ||
                         normalizedSearch.Contains("надолго") ||
                         normalizedSearch.Contains("долгосрочная"))
                {
                    dealType = "Долгосрочная аренда";
                }

                // Определяем тип недвижимости из текста поиска
                if (string.IsNullOrWhiteSpace(propertyType))
                {
                    // Определяем тип недвижимости из текста поиска.
                    // Новая поисковая фраза должна перезаписать старый фильтр.
                    if (normalizedSearch.Contains("квартир"))
                    {
                        propertyType = "Квартира";
                    }
                    else if (normalizedSearch.Contains("коттедж"))
                    {
                        propertyType = "Коттедж";
                    }
                    else if (normalizedSearch.Contains("таунхаус"))
                    {
                        propertyType = "Таунхаус";
                    }
                    else if (normalizedSearch.Contains("участ"))
                    {
                        propertyType = "Участок";
                    }
                    else if (normalizedSearch.Contains("гараж"))
                    {
                        propertyType = "Гараж";
                    }
                    else if (normalizedSearch.Contains("дом"))
                    {
                        propertyType = "Дом";
                    }
                }

                // Служебные слова, которые НЕ надо искать в названии/описании.
                // Они используются только для определения фильтров.
                var stopWords = new[]
                {
                    "купить", "куплю", "покупка", "продажа",
                    "снять", "сниму", "аренда", "надолго", "долгосрочная",
                    "посуточно", "посуточная", "сутки",
                    "квартира", "квартиру", "квартиры", "квартире",
                    "дом", "дома", "доме",
                    "коттедж", "коттеджа",
                    "таунхаус", "таунхауса",
                    "участок", "участка",
                    "гараж", "гаража"
                };

                var searchWords = normalizedSearch
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Where(word => !stopWords.Contains(word))
                    .ToList();

                // ВАЖНО:
                // Если пользователь ввёл "купить квартиру",
                // после удаления служебных слов ничего не останется.
                // Значит, текстовый поиск НЕ запускаем,
                // а объявления фильтруются только по dealType/propertyType.
                if (searchWords.Any())
                {
                    foreach (var word in searchWords)
                    {
                        var currentWord = word;

                        query = query.Where(a =>
                            (a.Title ?? "").ToLower().Contains(currentWord) ||
                            (a.Description ?? "").ToLower().Contains(currentWord) ||
                            (a.City ?? "").ToLower().Contains(currentWord) ||
                            (a.Address ?? "").ToLower().Contains(currentWord));
                    }
                }
            }

            // Фильтр по городу
            if (!string.IsNullOrWhiteSpace(city))
            {
                var cityLower = city.ToLower();

                query = query.Where(a =>
                    (a.City ?? "").ToLower().Contains(cityLower));
            }

            // Фильтр по типу сделки
            if (!string.IsNullOrWhiteSpace(dealType))
            {
                query = query.Where(a => a.DealType == dealType);
            }

            // Фильтр по типу недвижимости
            if (!string.IsNullOrWhiteSpace(propertyType))
            {
                query = query.Where(a => a.PropertyType == propertyType);
            }

            // Если пользователь перепутал цену от и до
            if (priceFrom.HasValue && priceTo.HasValue && priceFrom > priceTo)
            {
                var temp = priceFrom;
                priceFrom = priceTo;
                priceTo = temp;
            }

            // Фильтр по цене
            if (priceFrom.HasValue)
            {
                query = query.Where(a => a.Price >= priceFrom.Value);
            }

            if (priceTo.HasValue)
            {
                query = query.Where(a => a.Price <= priceTo.Value);
            }

            // Фильтры для дома / коттеджа / таунхауса

            if (totalAreaFrom.HasValue)
            {
                query = query.Where(a => a.TotalArea >= totalAreaFrom.Value);
            }

            if (totalAreaTo.HasValue)
            {
                query = query.Where(a => a.TotalArea <= totalAreaTo.Value);
            }

            if (bedroomsFrom.HasValue)
            {
                query = query.Where(a => a.Bedrooms >= bedroomsFrom.Value);
            }

            if (bedroomsTo.HasValue)
            {
                query = query.Where(a => a.Bedrooms <= bedroomsTo.Value);
            }

            if (totalFloorsFrom.HasValue)
            {
                query = query.Where(a => a.TotalFloors >= totalFloorsFrom.Value);
            }

            if (totalFloorsTo.HasValue)
            {
                query = query.Where(a => a.TotalFloors <= totalFloorsTo.Value);
            }

            if (buildYearFrom.HasValue)
            {
                query = query.Where(a => a.BuildYear >= buildYearFrom.Value);
            }

            if (buildYearTo.HasValue)
            {
                query = query.Where(a => a.BuildYear <= buildYearTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(houseMaterial))
            {
                query = query.Where(a => a.HouseMaterial == houseMaterial);
            }

            if (!string.IsNullOrWhiteSpace(houseCondition))
            {
                query = query.Where(a => a.HouseCondition == houseCondition);
            }

            if (!string.IsNullOrWhiteSpace(sewerage))
            {
                query = query.Where(a => a.Sewerage == sewerage);
            }

            if (!string.IsNullOrWhiteSpace(waterSupply))
            {
                query = query.Where(a => a.WaterSupply == waterSupply);
            }

            if (!string.IsNullOrWhiteSpace(gas))
            {
                query = query.Where(a => a.Gas == gas);
            }

            if (!string.IsNullOrWhiteSpace(heating))
            {
                query = query.Where(a => a.Heating == heating);
            }

            if (hasGarage)
            {
                query = query.Where(a => a.HasGarage);
            }

            if (hasTerrace)
            {
                query = query.Where(a => a.HasTerrace);
            }

            if (hasPool)
            {
                query = query.Where(a => a.HasPool);
            }

            if (hasBathhouse)
            {
                query = query.Where(a => a.HasBathhouse);
            }

            if (hasSecurity)
            {
                query = query.Where(a => a.HasSecurity);
            }

            // Фильтры для квартиры

            if (!string.IsNullOrWhiteSpace(roomsFilter))
            {
                if (roomsFilter == "studio")
                {
                    query = query.Where(a => a.Rooms == 0);
                }
                else if (roomsFilter == "5plus")
                {
                    query = query.Where(a => a.Rooms >= 5);
                }
                else if (int.TryParse(roomsFilter, out int roomsCount))
                {
                    query = query.Where(a => a.Rooms == roomsCount);
                }
            }

            if (floorFrom.HasValue)
            {
                query = query.Where(a => a.Floor >= floorFrom.Value);
            }

            if (floorTo.HasValue)
            {
                query = query.Where(a => a.Floor <= floorTo.Value);
            }

            if (livingAreaFrom.HasValue)
            {
                query = query.Where(a => a.LivingArea >= livingAreaFrom.Value);
            }

            if (livingAreaTo.HasValue)
            {
                query = query.Where(a => a.LivingArea <= livingAreaTo.Value);
            }

            if (kitchenAreaFrom.HasValue)
            {
                query = query.Where(a => a.KitchenArea >= kitchenAreaFrom.Value);
            }

            if (kitchenAreaTo.HasValue)
            {
                query = query.Where(a => a.KitchenArea <= kitchenAreaTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(renovation))
            {
                query = query.Where(a => a.Renovation == renovation);
            }

            if (!string.IsNullOrWhiteSpace(buildingType))
            {
                query = query.Where(a => a.BuildingType == buildingType);
            }

            if (!string.IsNullOrWhiteSpace(bathroomType))
            {
                query = query.Where(a => a.BathroomType == bathroomType);
            }

            if (!string.IsNullOrWhiteSpace(balconyType))
            {
                query = query.Where(a => a.BalconyType == balconyType);
            }

            if (hasElevator)
            {
                query = query.Where(a => a.HasElevator);
            }

            if (hasParking)
            {
                query = query.Where(a => a.HasParking);
            }

            // Фильтры для участка

            if (landAreaFrom.HasValue)
            {
                query = query.Where(a => a.LandArea >= landAreaFrom.Value);
            }

            if (landAreaTo.HasValue)
            {
                query = query.Where(a => a.LandArea <= landAreaTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(landCategory))
            {
                query = query.Where(a => a.LandCategory == landCategory);
            }

            if (!string.IsNullOrWhiteSpace(landStatus))
            {
                query = query.Where(a => a.LandStatus == landStatus);
            }

            if (!string.IsNullOrWhiteSpace(electricity))
            {
                query = query.Where(a => a.Electricity == electricity);
            }

            // Фильтры для гаража

            if (!string.IsNullOrWhiteSpace(garageType))
            {
                query = query.Where(a => a.GarageType == garageType);
            }

            if (!string.IsNullOrWhiteSpace(garageStatus))
            {
                query = query.Where(a => a.GarageStatus == garageStatus);
            }

            // Фильтры долгосрочной аренды
            if (depositFrom.HasValue)
            {
                query = query.Where(a => a.Deposit >= depositFrom.Value);
            }

            if (depositTo.HasValue)
            {
                query = query.Where(a => a.Deposit <= depositTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(prepayment))
            {
                query = query.Where(a => a.Prepayment == prepayment);
            }

            // Фильтры посуточной аренды
            if (guestsCountFrom.HasValue)
            {
                query = query.Where(a => a.GuestsCount >= guestsCountFrom.Value);
            }

            if (sleepingPlacesFrom.HasValue)
            {
                query = query.Where(a => a.SleepingPlaces >= sleepingPlacesFrom.Value);
            }

            if (availableFrom.HasValue)
            {
                query = query.Where(a => a.AvailableFrom == null || a.AvailableFrom <= availableFrom.Value);
            }

            if (availableTo.HasValue)
            {
                query = query.Where(a => a.AvailableTo == null || a.AvailableTo >= availableTo.Value);
            }

            if (hasWifi)
            {
                query = query.Where(a => a.HasWifi);
            }

            if (hasAirConditioner)
            {
                query = query.Where(a => a.HasAirConditioner);
            }

            if (hasKitchen)
            {
                query = query.Where(a => a.HasKitchen);
            }

            if (hasTv)
            {
                query = query.Where(a => a.HasTv);
            }

            if (hasWashingMachine)
            {
                query = query.Where(a => a.HasWashingMachine);
            }

            if (hasBedLinen)
            {
                query = query.Where(a => a.HasBedLinen);
            }

            if (hasFurniture)
            {
                query = query.Where(a => a.HasFurniture);
            }

            if (hasAppliances)
            {
                query = query.Where(a => a.HasAppliances);
            }

            if (allowChildren)
            {
                query = query.Where(a => a.AllowChildren);
            }

            if (allowPets)
            {
                query = query.Where(a => a.AllowPets);
            }

            // Сортировка
            query = sortBy switch
            {
                "price_asc" => query.OrderBy(a => a.Price),
                "price_desc" => query.OrderByDescending(a => a.Price),
                "date_old" => query.OrderBy(a => a.CreatedAt),
                "date_asc" => query.OrderBy(a => a.CreatedAt),
                _ => query.OrderByDescending(a => a.CreatedAt)
            };

            int totalAds = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalAds / PageSize);

            if (page < 1)
            {
                page = 1;
            }

            if (totalPages > 0 && page > totalPages)
            {
                page = totalPages;
            }

            var ads = query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            foreach (var ad in ads)
            {
                ad.Images = _context.AdImages
                    .Where(i => i.AdId == ad.Id)
                    .ToList();
            }

            var model = new AdFilterViewModel
            {
                SearchText = searchText,
                City = city,
                DealType = dealType,
                PropertyType = propertyType,
                PriceFrom = priceFrom,
                PriceTo = priceTo,
                SortBy = sortBy,

                TotalAreaFrom = totalAreaFrom,
                TotalAreaTo = totalAreaTo,
                BedroomsFrom = bedroomsFrom,
                BedroomsTo = bedroomsTo,
                TotalFloorsFrom = totalFloorsFrom,
                TotalFloorsTo = totalFloorsTo,
                BuildYearFrom = buildYearFrom,
                BuildYearTo = buildYearTo,
                HouseMaterial = houseMaterial,
                HouseCondition = houseCondition,
                Sewerage = sewerage,
                WaterSupply = waterSupply,
                Gas = gas,
                Heating = heating,
                HasGarage = hasGarage,
                HasTerrace = hasTerrace,
                HasPool = hasPool,
                HasBathhouse = hasBathhouse,
                HasSecurity = hasSecurity,

                RoomsFilter = roomsFilter,
                FloorFrom = floorFrom,
                FloorTo = floorTo,
                LivingAreaFrom = livingAreaFrom,
                LivingAreaTo = livingAreaTo,
                KitchenAreaFrom = kitchenAreaFrom,
                KitchenAreaTo = kitchenAreaTo,
                Renovation = renovation,
                BuildingType = buildingType,
                BathroomType = bathroomType,
                BalconyType = balconyType,
                HasElevator = hasElevator,
                HasParking = hasParking,

                LandAreaFrom = landAreaFrom,
                LandAreaTo = landAreaTo,
                LandCategory = landCategory,
                LandStatus = landStatus,
                Electricity = electricity,

                GarageType = garageType,
                GarageStatus = garageStatus,
                
                DepositFrom = depositFrom,
                DepositTo = depositTo,
                Prepayment = prepayment,

                GuestsCountFrom = guestsCountFrom,
                SleepingPlacesFrom = sleepingPlacesFrom,
                AvailableFrom = availableFrom,
                AvailableTo = availableTo,

                HasWifi = hasWifi,
                HasAirConditioner = hasAirConditioner,
                HasKitchen = hasKitchen,
                HasTv = hasTv,
                HasWashingMachine = hasWashingMachine,
                HasBedLinen = hasBedLinen,
                HasFurniture = hasFurniture,
                HasAppliances = hasAppliances,
                AllowChildren = allowChildren,
                AllowPets = allowPets,

                Ads = ads,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalAds = totalAds
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}