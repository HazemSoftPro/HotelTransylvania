using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnHotel.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceBasePriceWithManualPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add ManualPrice column to rooms table
            migrationBuilder.AddColumn<decimal>(
                name: "manual_price",
                table: "rooms",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 0m);

            // Step 2: Populate ManualPrice with basePrice from related RoomType
            // If PriceOverride exists, use it; otherwise use basePrice from room_types
            migrationBuilder.Sql(@"
                UPDATE rooms 
                SET manual_price = COALESCE(price_override, rt.base_price)
                FROM room_types rt 
                WHERE rooms.room_type_id = rt.id;
            ");

            // Step 3: Add check constraint to ensure ManualPrice > 0
            migrationBuilder.AddCheckConstraint(
                name: "CK_rooms_manual_price",
                table: "rooms",
                sql: "manual_price > 0");

            // Step 4: Drop PriceOverride column from rooms table
            migrationBuilder.DropColumn(
                name: "price_override",
                table: "rooms");

            // Step 5: Drop BasePrice column and its constraint from room_types table
            migrationBuilder.DropCheckConstraint(
                name: "CK_room_types_base_price",
                table: "room_types");

            migrationBuilder.DropColumn(
                name: "base_price",
                table: "room_types");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback Step 5: Re-add BasePrice column to room_types
            migrationBuilder.AddColumn<decimal>(
                name: "base_price",
                table: "room_types",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 100.00m); // Default fallback price

            migrationBuilder.AddCheckConstraint(
                name: "CK_room_types_base_price",
                table: "room_types",
                sql: "base_price > 0");

            // Rollback Step 4: Re-add PriceOverride column to rooms
            migrationBuilder.AddColumn<decimal>(
                name: "price_override",
                table: "rooms",
                type: "numeric",
                nullable: true);

            // Rollback Step 3: Drop ManualPrice constraint
            migrationBuilder.DropCheckConstraint(
                name: "CK_rooms_manual_price",
                table: "rooms");

            // Rollback Step 2: Try to restore original data structure
            // This is a best-effort rollback - some data relationships may be lost
            migrationBuilder.Sql(@"
                UPDATE room_types 
                SET base_price = (
                    SELECT MIN(manual_price) 
                    FROM rooms 
                    WHERE rooms.room_type_id = room_types.id
                )
                WHERE EXISTS (
                    SELECT 1 FROM rooms WHERE rooms.room_type_id = room_types.id
                );
            ");

            // Rollback Step 1: Drop ManualPrice column
            migrationBuilder.DropColumn(
                name: "manual_price",
                table: "rooms");
        }
    }
}
