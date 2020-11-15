using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ChangeUserStoryNameToTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserStories");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "UserStories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "UserStories");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserStories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
