using Microsoft.EntityFrameworkCore.Migrations;
using MySpot.Core.Entities;

#nullable disable

namespace MySpot.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Introducing_Capacity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParkingSpotCapacity",
                table: "WeeklyParkingSpots",
                type: "integer",
                nullable: false,
                defaultValue: WeeklyParkingSpot.MaxParkingSpotCapacity);

            migrationBuilder.AddColumn<int>(
                name: "ParkingSpotCapacity",
                table: "Reservations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParkingSpotCapacity",
                table: "WeeklyParkingSpots");

            migrationBuilder.DropColumn(
                name: "ParkingSpotCapacity",
                table: "Reservations");
        }
    }
}
