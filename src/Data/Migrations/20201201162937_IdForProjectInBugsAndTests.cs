using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class IdForProjectInBugsAndTests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdForProject",
                table: "Tests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdForProject",
                table: "Bugs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdForProject",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "IdForProject",
                table: "Bugs");
        }
    }
}
