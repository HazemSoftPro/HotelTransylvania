using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnHotel.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "reservation_services",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "reservation_services");
        }
    }
}
