using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingHereApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameOfParkingSpotsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spaces_Parkings_ParkingId",
                table: "Spaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Spaces",
                table: "Spaces");

            migrationBuilder.RenameTable(
                name: "Spaces",
                newName: "ParkingSpots");

            migrationBuilder.RenameIndex(
                name: "IX_Spaces_ParkingId",
                table: "ParkingSpots",
                newName: "IX_ParkingSpots_ParkingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingSpots",
                table: "ParkingSpots",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpots_Parkings_ParkingId",
                table: "ParkingSpots",
                column: "ParkingId",
                principalTable: "Parkings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpots_Parkings_ParkingId",
                table: "ParkingSpots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingSpots",
                table: "ParkingSpots");

            migrationBuilder.RenameTable(
                name: "ParkingSpots",
                newName: "Spaces");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSpots_ParkingId",
                table: "Spaces",
                newName: "IX_Spaces_ParkingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Spaces",
                table: "Spaces",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Spaces_Parkings_ParkingId",
                table: "Spaces",
                column: "ParkingId",
                principalTable: "Parkings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
