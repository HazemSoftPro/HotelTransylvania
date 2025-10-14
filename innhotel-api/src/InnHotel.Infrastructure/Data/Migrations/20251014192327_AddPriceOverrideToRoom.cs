using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnHotel.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceOverrideToRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceOverride",
                table: "rooms",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceOverride",
                table: "rooms");
        }
    }
}
