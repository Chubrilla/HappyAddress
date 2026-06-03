using HappyAddress.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappyAddress.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260603153000_EnsureAdPhoneNumberColumn")]
    public partial class EnsureAdPhoneNumberColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                SET @column_exists = (
                    SELECT COUNT(*)
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_SCHEMA = DATABASE()
                      AND TABLE_NAME = 'Ads'
                      AND COLUMN_NAME = 'PhoneNumber'
                );
                """);

            migrationBuilder.Sql("""
                SET @sql = IF(
                    @column_exists = 0,
                    'ALTER TABLE `Ads` ADD COLUMN `PhoneNumber` longtext NULL',
                    'SELECT 1'
                );
                """);

            migrationBuilder.Sql("PREPARE stmt FROM @sql;");
            migrationBuilder.Sql("EXECUTE stmt;");
            migrationBuilder.Sql("DEALLOCATE PREPARE stmt;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Ads");
        }
    }
}
