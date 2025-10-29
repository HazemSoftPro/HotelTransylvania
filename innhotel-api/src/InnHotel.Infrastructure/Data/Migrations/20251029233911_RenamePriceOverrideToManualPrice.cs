using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnHotel.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenamePriceOverrideToManualPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename PriceOverride column to manual_price
            migrationBuilder.RenameColumn(
                name: "PriceOverride",
                table: "rooms",
                newName: "manual_price");

            // Update existing NULL values to default price before making it NOT NULL
            migrationBuilder.Sql(@"
                UPDATE rooms 
                SET manual_price = 100.00 
                WHERE manual_price IS NULL;
            ");

            // Alter column to be NOT NULL with proper type
            migrationBuilder.AlterColumn<decimal>(
                name: "manual_price",
                table: "rooms",
                type: "numeric(10,2)",
                nullable: false,
                defaultValue: 100.00m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

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

            // Alter column back to nullable
            migrationBuilder.AlterColumn<decimal>(
                name: "manual_price",
                table: "rooms",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldNullable: false);

            // Rename back to PriceOverride
            migrationBuilder.RenameColumn(
                name: "manual_price",
                table: "rooms",
                newName: "PriceOverride");
        }
    }
}
