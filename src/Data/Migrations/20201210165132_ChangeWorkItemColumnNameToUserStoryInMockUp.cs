using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ChangeWorkItemColumnNameToUserStoryInMockUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mockups_UserStories_UserStoryId",
                table: "Mockups");

            migrationBuilder.DropColumn(
                name: "WorkItemId",
                table: "Mockups");

            migrationBuilder.AlterColumn<int>(
                name: "UserStoryId",
                table: "Mockups",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mockups_UserStories_UserStoryId",
                table: "Mockups",
                column: "UserStoryId",
                principalTable: "UserStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mockups_UserStories_UserStoryId",
                table: "Mockups");

            migrationBuilder.AlterColumn<int>(
                name: "UserStoryId",
                table: "Mockups",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "WorkItemId",
                table: "Mockups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Mockups_UserStories_UserStoryId",
                table: "Mockups",
                column: "UserStoryId",
                principalTable: "UserStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
