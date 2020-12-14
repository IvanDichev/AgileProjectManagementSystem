using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ChangeColumnDataTypeOfDayNoInBurndownData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayNo",
                table: "BurndownDatas");

            migrationBuilder.AddColumn<DateTime>(
                name: "DayOfSprint",
                table: "BurndownDatas",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfSprint",
                table: "BurndownDatas");

            migrationBuilder.AddColumn<int>(
                name: "DayNo",
                table: "BurndownDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
