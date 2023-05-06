using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingHereApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDatesToParkingSpotEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Parkings");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Spaces",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Spaces",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Spaces");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Spaces");

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Parkings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
