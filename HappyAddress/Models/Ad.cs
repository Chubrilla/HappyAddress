using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HappyAddress.Models
{
    public class Ad
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        public string City { get; set; }
        public string Address { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string DealType { get; set; }
        public string PropertyType { get; set; }

        // Общие параметры объекта
        public double? TotalArea { get; set; }          // Площадь объекта, м²
        public int? Rooms { get; set; }                 // Количество комнат
        public int? Bedrooms { get; set; }              // Количество спален
        public int? Floor { get; set; }                 // Этаж
        public int? TotalFloors { get; set; }           // Всего этажей
        public int? BuildYear { get; set; }             // Год постройки

        // Дом / дача / коттедж
        public string? HouseType { get; set; }          // Для постоянного проживания / Дача
        public string? HouseMaterial { get; set; }      // Кирпичный, монолитный, деревянный и т.д.
        public string? HouseCondition { get; set; }     // Можно жить, нужен ремонт и т.д.

        // Участок
        public double? LandArea { get; set; }           // Площадь участка
        public string? LandCategory { get; set; }       // Категория земли
        public string? LandStatus { get; set; }         // ИЖС, СНТ, ЛПХ и т.д.

        // Квартира
        public double? LivingArea { get; set; }         // Жилая площадь
        public double? KitchenArea { get; set; }        // Площадь кухни
        public string? Renovation { get; set; }         // Ремонт
        public string? BuildingType { get; set; }       // Тип дома
        public string? BathroomType { get; set; }       // Санузел
        public string? BalconyType { get; set; }        // Балкон / лоджия

        // Коммуникации
        public string? Sewerage { get; set; }           // Канализация
        public string? WaterSupply { get; set; }        // Водоснабжение
        public string? Gas { get; set; }                // Газ
        public string? Heating { get; set; }            // Отопление
        public string? Electricity { get; set; }        // Электричество

        // Дополнительные удобства
        public bool HasGarage { get; set; }             // Гараж
        public bool HasTerrace { get; set; }            // Терраса
        public bool HasCellar { get; set; }             // Погреб
        public bool HasPool { get; set; }               // Бассейн
        public bool HasBathhouse { get; set; }          // Баня
        public bool HasSecurity { get; set; }           // Охрана
        public bool HasParking { get; set; }            // Парковка
        public bool HasElevator { get; set; }           // Лифт

        // Условия сделки
        public bool MortgageAllowed { get; set; }       // Ипотека возможна
        public string? SaleType { get; set; }           // Только продаю / одновременно покупаю другую

        // Аренда
        public decimal? Deposit { get; set; }           // Залог
        public string? Prepayment { get; set; }         // Предоплата
        public bool AllowChildren { get; set; }         // Можно с детьми
        public bool AllowPets { get; set; }             // Можно с животными
        public bool HasFurniture { get; set; }          // Есть мебель
        public bool HasAppliances { get; set; }         // Есть техника

        // Посуточная аренда
        public int? GuestsCount { get; set; }           // Количество гостей
        public DateTime? AvailableFrom { get; set; }    // Доступно с
        public DateTime? AvailableTo { get; set; }      // Доступно до
        public int? SleepingPlaces { get; set; }        // Спальных мест
        public bool HasWifi { get; set; }               // Wi-Fi
        public bool HasAirConditioner { get; set; }     // Кондиционер
        public bool HasKitchen { get; set; }            // Кухня
        public bool HasTv { get; set; }                 // Телевизор
        public bool HasWashingMachine { get; set; }     // Стиральная машина
        public bool HasBedLinen { get; set; }           // Постельное бельё

        // Гараж / машино-место
        public string? GarageType { get; set; }         // Тип гаража
        public string? GarageStatus { get; set; }       // Собственность / аренда / ГСК

        public string Status { get; set; } = "На модерации";
        public string? RejectReason { get; set; }

        public string? ImagePath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ValidateNever]
        public User? User { get; set; }

        public string? PhoneNumber { get; set; }

        [ValidateNever]
        public List<AdImage> Images { get; set; } = new List<AdImage>();
    }
}