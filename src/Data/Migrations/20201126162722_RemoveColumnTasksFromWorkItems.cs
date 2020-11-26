using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class RemoveColumnTasksFromWorkItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_WorkItemTypes_TypeId",
                table: "UserStories");

            migrationBuilder.DropTable(
                name: "SubTask");

            migrationBuilder.DropTable(
                name: "Assignment");

            migrationBuilder.DropIndex(
                name: "IX_UserStories_TypeId",
                table: "UserStories");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "UserStories");

            migrationBuilder.AddColumn<int>(
                name: "WorkItemTypeId",
                table: "UserStories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_WorkItemTypeId",
                table: "UserStories",
                column: "WorkItemTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_WorkItemTypes_WorkItemTypeId",
                table: "UserStories",
                column: "WorkItemTypeId",
                principalTable: "WorkItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_WorkItemTypes_WorkItemTypeId",
                table: "UserStories");

            migrationBuilder.DropIndex(
                name: "IX_UserStories_WorkItemTypeId",
                table: "UserStories");

            migrationBuilder.DropColumn(
                name: "WorkItemTypeId",
                table: "UserStories");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "UserStories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Assignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Effort = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SprintId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    UserStoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignment_Sprints_SprintId",
                        column: x => x.SprintId,
                        principalTable: "Sprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignment_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignment_UserStories_UserStoryId",
                        column: x => x.UserStoryId,
                        principalTable: "UserStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubTask_Assignment_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Assignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_TypeId",
                table: "UserStories",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_SprintId",
                table: "Assignment",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_UserId",
                table: "Assignment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_UserStoryId",
                table: "Assignment",
                column: "UserStoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTask_TaskId",
                table: "SubTask",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_WorkItemTypes_TypeId",
                table: "UserStories",
                column: "TypeId",
                principalTable: "WorkItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
