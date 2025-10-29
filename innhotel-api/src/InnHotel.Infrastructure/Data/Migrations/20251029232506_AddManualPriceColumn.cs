using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnHotel.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddManualPriceColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename PriceOverride column to manual_price
            migrationBuilder.RenameColumn(
                name: "PriceOverride",
                table: "rooms",
                newName: "manual_price");

            // Update column type and make it NOT NULL with default value
            migrationBuilder.AlterColumn<decimal>(
                name: "manual_price",
                table: "rooms",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 100.00m);

            // Update existing NULL values to default price
            migrationBuilder.Sql(@"
                UPDATE rooms 
                SET manual_price = 100.00 
                WHERE manual_price IS NULL;
            ");

            // Add check constraint to ensure ManualPrice > 0
            migrationBuilder.Sql(@"
                ALTER TABLE rooms 
                DROP CONSTRAINT IF EXISTS ""CK_rooms_manual_price"";
                
                ALTER TABLE rooms 
                ADD CONSTRAINT ""CK_rooms_manual_price"" 
                CHECK (manual_price > 0);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the check constraint
            migrationBuilder.Sql(@"
                ALTER TABLE rooms 
                DROP CONSTRAINT IF EXISTS ""CK_rooms_manual_price"";
            ");

            // Rename back to PriceOverride and make it nullable
            migrationBuilder.RenameColumn(
                name: "manual_price",
                table: "rooms",
                newName: "PriceOverride");

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceOverride",
                table: "rooms",
                type: "numeric",
                nullable: true);
        }
    }
}
