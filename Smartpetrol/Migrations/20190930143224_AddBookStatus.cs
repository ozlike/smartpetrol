using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Smartpetrol.Migrations
{
    public partial class AddBookStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRented",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "IsReserved",
                table: "Books");

            migrationBuilder.AddColumn<DateTime>(
                name: "RentalTime",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationTime",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentalTime",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ReservationTime",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Books");

            migrationBuilder.AddColumn<bool>(
                name: "IsRented",
                table: "Books",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReserved",
                table: "Books",
                nullable: false,
                defaultValue: false);
        }
    }
}
