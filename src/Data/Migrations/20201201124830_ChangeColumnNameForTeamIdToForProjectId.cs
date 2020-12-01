using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ChangeColumnNameForTeamIdToForProjectId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdForTeam",
                table: "UserStories");

            migrationBuilder.AddColumn<int>(
                name: "IdForProject",
                table: "UserStories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdForProject",
                table: "UserStories");

            migrationBuilder.AddColumn<int>(
                name: "IdForTeam",
                table: "UserStories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
