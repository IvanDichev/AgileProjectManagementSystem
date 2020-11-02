using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ChangeFKUserStoryIdToNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_UserStories_UserStoryId",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "UserStoryId",
                table: "Projects",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_UserStories_UserStoryId",
                table: "Projects",
                column: "UserStoryId",
                principalTable: "UserStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_UserStories_UserStoryId",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "UserStoryId",
                table: "Projects",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_UserStories_UserStoryId",
                table: "Projects",
                column: "UserStoryId",
                principalTable: "UserStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
