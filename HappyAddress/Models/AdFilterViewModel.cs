using System.Collections.Generic;

namespace HappyAddress.Models
{
    public class AdFilterViewModel
    {
        public string SearchText { get; set; }

        public string City { get; set; }
        public string DealType { get; set; }
        public string PropertyType { get; set; }

        public decimal? PriceFrom {  get; set; }
        public decimal? PriceTo { get; set; }

        public string SortBy { get; set; }

        // Параметры квартиры
        public string? RoomsFilter { get; set; }

        public int? FloorFrom { get; set; }
        public int? FloorTo { get; set; }

        public double? LivingAreaFrom { get; set; }
        public double? LivingAreaTo { get; set; }

        public double? KitchenAreaFrom { get; set; }
        public double? KitchenAreaTo { get; set; }

        public string? Renovation { get; set; }
        public string? BuildingType { get; set; }
        public string? BathroomType { get; set; }
        public string? BalconyType { get; set; }

        public bool HasElevator { get; set; }
        public bool HasParking { get; set; }

        // Параметры дома
        public double? TotalAreaFrom { get; set; }
        public double? TotalAreaTo { get; set; }

        public int? BedroomsFrom { get; set; }
        public int? BedroomsTo { get; set; }

        public int? TotalFloorsFrom { get; set; }
        public int? TotalFloorsTo { get; set; }

        public int? BuildYearFrom { get; set; }
        public int? BuildYearTo { get; set; }

        public string HouseMaterial { get; set; }
        public string HouseCondition { get; set; }

        // Коммуникации
        public string Sewerage { get; set; }
        public string WaterSupply { get; set; }
        public string Gas { get; set; }
        public string Heating { get; set; }

        // Удобства
        public bool HasGarage { get; set; }
        public bool HasTerrace { get; set; }
        public bool HasPool { get; set; }
        public bool HasBathhouse { get; set; }
        public bool HasSecurity { get; set; }

        // Параметры участка
        public double? LandAreaFrom { get; set; }
        public double? LandAreaTo { get; set; }

        public string? LandCategory { get; set; }
        public string? LandStatus { get; set; }

        public string? Electricity { get; set; }

        // Параметры гаража
        public string? GarageType { get; set; }
        public string? GarageStatus { get; set; }

        // Фильтры долгосрочной аренды
        public decimal? DepositFrom { get; set; }
        public decimal? DepositTo { get; set; }

        public string? Prepayment { get; set; }

        // Фильтры посуточной аренды
        public int? GuestsCountFrom { get; set; }
        public int? SleepingPlacesFrom { get; set; }

        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }

        public bool HasWifi { get; set; }
        public bool HasAirConditioner { get; set; }
        public bool HasKitchen { get; set; }
        public bool HasTv { get; set; }
        public bool HasWashingMachine { get; set; }
        public bool HasBedLinen { get; set; }

        public bool HasFurniture { get; set; }
        public bool HasAppliances { get; set; }
        public bool AllowChildren { get; set; }
        public bool AllowPets { get; set; }

        public List<Ad> Ads { get; set; } = new List<Ad>();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalAds { get; set; }
    }
}