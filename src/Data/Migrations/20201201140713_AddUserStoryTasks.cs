using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddUserStoryTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoryPoints",
                table: "Bugs");

            migrationBuilder.AddColumn<int>(
                name: "UserStoryTaskId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserStoryTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AcceptanceCriteria = table.Column<string>(maxLength: 3000, nullable: true),
                    UserStoryId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStoryTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStoryTasks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserStoryTasks_UserStories_UserStoryId",
                        column: x => x.UserStoryId,
                        principalTable: "UserStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserStoryTaskId",
                table: "Comments",
                column: "UserStoryTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStoryTasks_UserId",
                table: "UserStoryTasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStoryTasks_UserStoryId",
                table: "UserStoryTasks",
                column: "UserStoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_UserStoryTasks_UserStoryTaskId",
                table: "Comments",
                column: "UserStoryTaskId",
                principalTable: "UserStoryTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_UserStoryTasks_UserStoryTaskId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "UserStoryTasks");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserStoryTaskId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserStoryTaskId",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "StoryPoints",
                table: "Bugs",
                type: "int",
                nullable: true);
        }
    }
}
