using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappyAddress.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyRentComfortFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAirConditioner",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasBedLinen",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasKitchen",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasTv",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasWashingMachine",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasWifi",
                table: "Ads",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SleepingPlaces",
                table: "Ads",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAirConditioner",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HasBedLinen",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HasKitchen",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HasTv",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HasWashingMachine",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "HasWifi",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "SleepingPlaces",
                table: "Ads");
        }
    }
}
