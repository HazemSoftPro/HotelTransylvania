using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnHotel.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class AddSearchIndexes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Room search indexes
        migrationBuilder.CreateIndex(
            name: "IX_rooms_room_number",
            table: "rooms",
            column: "room_number");

        migrationBuilder.CreateIndex(
            name: "IX_rooms_status",
            table: "rooms",
            column: "status");

        migrationBuilder.CreateIndex(
            name: "IX_rooms_floor",
            table: "rooms",
            column: "floor");

        // Guest search indexes
        migrationBuilder.CreateIndex(
            name: "IX_guests_first_name",
            table: "guests",
            column: "first_name");

        migrationBuilder.CreateIndex(
            name: "IX_guests_last_name",
            table: "guests",
            column: "last_name");

        migrationBuilder.CreateIndex(
            name: "IX_guests_email",
            table: "guests",
            column: "email");

        migrationBuilder.CreateIndex(
            name: "IX_guests_phone",
            table: "guests",
            column: "phone");

        migrationBuilder.CreateIndex(
            name: "IX_guests_id_proof_number",
            table: "guests",
            column: "id_proof_number");

        // Employee search indexes
        migrationBuilder.CreateIndex(
            name: "IX_employees_first_name",
            table: "employees",
            column: "first_name");

        migrationBuilder.CreateIndex(
            name: "IX_employees_last_name",
            table: "employees",
            column: "last_name");

        migrationBuilder.CreateIndex(
            name: "IX_employees_position",
            table: "employees",
            column: "position");

        migrationBuilder.CreateIndex(
            name: "IX_employees_hire_date",
            table: "employees",
            column: "hire_date");

        // Reservation search indexes
        migrationBuilder.CreateIndex(
            name: "IX_reservations_check_in_date",
            table: "reservations",
            column: "check_in_date");

        migrationBuilder.CreateIndex(
            name: "IX_reservations_check_out_date",
            table: "reservations",
            column: "check_out_date");

        migrationBuilder.CreateIndex(
            name: "IX_reservations_status",
            table: "reservations",
            column: "status");

        // Room type search indexes
        migrationBuilder.CreateIndex(
            name: "IX_room_types_type_name",
            table: "room_types",
            column: "type_name");

        // Branch search indexes
        migrationBuilder.CreateIndex(
            name: "IX_branches_branch_name",
            table: "branches",
            column: "branch_name");

        migrationBuilder.CreateIndex(
            name: "IX_branches_city",
            table: "branches",
            column: "city");

        // Service search indexes
        migrationBuilder.CreateIndex(
            name: "IX_services_service_name",
            table: "services",
            column: "service_name");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Drop room indexes
        migrationBuilder.DropIndex(
            name: "IX_rooms_room_number",
            table: "rooms");

        migrationBuilder.DropIndex(
            name: "IX_rooms_status",
            table: "rooms");

        migrationBuilder.DropIndex(
            name: "IX_rooms_floor",
            table: "rooms");

        // Drop guest indexes
        migrationBuilder.DropIndex(
            name: "IX_guests_first_name",
            table: "guests");

        migrationBuilder.DropIndex(
            name: "IX_guests_last_name",
            table: "guests");

        migrationBuilder.DropIndex(
            name: "IX_guests_email",
            table: "guests");

        migrationBuilder.DropIndex(
            name: "IX_guests_phone",
            table: "guests");

        migrationBuilder.DropIndex(
            name: "IX_guests_id_proof_number",
            table: "guests");

        // Drop employee indexes
        migrationBuilder.DropIndex(
            name: "IX_employees_first_name",
            table: "employees");

        migrationBuilder.DropIndex(
            name: "IX_employees_last_name",
            table: "employees");

        migrationBuilder.DropIndex(
            name: "IX_employees_position",
            table: "employees");

        migrationBuilder.DropIndex(
            name: "IX_employees_hire_date",
            table: "employees");

        // Drop reservation indexes
        migrationBuilder.DropIndex(
            name: "IX_reservations_check_in_date",
            table: "reservations");

        migrationBuilder.DropIndex(
            name: "IX_reservations_check_out_date",
            table: "reservations");

        migrationBuilder.DropIndex(
            name: "IX_reservations_status",
            table: "reservations");

        // Drop room type indexes
        migrationBuilder.DropIndex(
            name: "IX_room_types_type_name",
            table: "room_types");

        // Drop branch indexes
        migrationBuilder.DropIndex(
            name: "IX_branches_branch_name",
            table: "branches");

        migrationBuilder.DropIndex(
            name: "IX_branches_city",
            table: "branches");

        // Drop service indexes
        migrationBuilder.DropIndex(
            name: "IX_services_service_name",
            table: "services");
    }
}
