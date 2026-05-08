using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappyAddress.Migrations
{
    /// <inheritdoc />
    public partial class AddDynamicAdParameters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowChildren",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowPets",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableFrom",
                table: "Ads",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableTo",
                table: "Ads",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BalconyType",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "BathroomType",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Bedrooms",
                table: "Ads",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuildYear",
                table: "Ads",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuildingType",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "Deposit",
                table: "Ads",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Electricity",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Floor",
                table: "Ads",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GarageStatus",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "GarageType",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Gas",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "GuestsCount",
                table: "Ads",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAppliances",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasBathhouse",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasCellar",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasElevator",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasFurniture",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasGarage",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasParking",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPool",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasSecurity",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasTerrace",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Heating",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HouseCondition",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HouseMaterial",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HouseType",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<double>(
                name: "KitchenArea",
                table: "Ads",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LandArea",
                table: "Ads",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LandCategory",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LandStatus",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<double>(
                name: "LivingArea",
                table: "Ads",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MortgageAllowed",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Prepayment",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Renovation",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Rooms",
                table: "Ads",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleType",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Sewerage",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<double>(
                name: "TotalArea",
                table: "Ads",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalFloors",
                table: "Ads",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaterSupply",
                table: "Ads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowChildren",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "AllowPets",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "AvailableFrom",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "AvailableTo",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "BalconyType",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "BathroomType",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Bedrooms",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "BuildYear",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "BuildingType",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Deposit",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Electricity",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "GarageStatus",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "GarageType",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Gas",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "GuestsCount",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HasAppliances",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HasBathhouse",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HasCellar",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HasElevator",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HasFurniture",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HasGarage",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HasParking",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HasPool",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HasSecurity",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HasTerrace",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Heating",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HouseCondition",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HouseMaterial",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HouseType",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "KitchenArea",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "LandArea",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "LandCategory",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "LandStatus",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "LivingArea",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "MortgageAllowed",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Prepayment",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Renovation",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Rooms",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "SaleType",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Sewerage",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "TotalArea",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "TotalFloors",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "WaterSupply",
                table: "Ads");
        }
    }
}
