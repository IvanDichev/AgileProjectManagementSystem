using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddIdForTeamInUSerStories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tasks_UserStoryTaskId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Mockup_Tasks_UserStoryTaskId",
                table: "Mockup");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Mockup_UserStoryTaskId",
                table: "Mockup");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserStoryTaskId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserStoryTaskId",
                table: "Mockup");

            migrationBuilder.DropColumn(
                name: "UserStoryTaskId",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "IdForTeam",
                table: "UserStories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WorkItemTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItemTypes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkItemTypes");

            migrationBuilder.DropColumn(
                name: "IdForTeam",
                table: "UserStories");

            migrationBuilder.AddColumn<int>(
                name: "UserStoryTaskId",
                table: "Mockup",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserStoryTaskId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcceptanceCriteria = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    UserStoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_UserStories_UserStoryId",
                        column: x => x.UserStoryId,
                        principalTable: "UserStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mockup_UserStoryTaskId",
                table: "Mockup",
                column: "UserStoryTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserStoryTaskId",
                table: "Comments",
                column: "UserStoryTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserStoryId",
                table: "Tasks",
                column: "UserStoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tasks_UserStoryTaskId",
                table: "Comments",
                column: "UserStoryTaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mockup_Tasks_UserStoryTaskId",
                table: "Mockup",
                column: "UserStoryTaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
