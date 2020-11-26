using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class RemoveTaskAndSubTaskTablesAddWorkItemTypesTableChangeNameUserStoriesToWorkItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTasks_Tasks_TaskId",
                table: "SubTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Sprints_SprintId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_UserStories_UserStoryId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubTasks",
                table: "SubTasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Assignment");

            migrationBuilder.RenameTable(
                name: "SubTasks",
                newName: "SubTask");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_UserStoryId",
                table: "Assignment",
                newName: "IX_Assignment_UserStoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_UserId",
                table: "Assignment",
                newName: "IX_Assignment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_SprintId",
                table: "Assignment",
                newName: "IX_Assignment_SprintId");

            migrationBuilder.RenameIndex(
                name: "IX_SubTasks_TaskId",
                table: "SubTask",
                newName: "IX_SubTask_TaskId");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "UserStories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkItemId",
                table: "UserStories",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assignment",
                table: "Assignment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubTask",
                table: "SubTask",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_TypeId",
                table: "UserStories",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_WorkItemId",
                table: "UserStories",
                column: "WorkItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Sprints_SprintId",
                table: "Assignment",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_AspNetUsers_UserId",
                table: "Assignment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_UserStories_UserStoryId",
                table: "Assignment",
                column: "UserStoryId",
                principalTable: "UserStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTask_Assignment_TaskId",
                table: "SubTask",
                column: "TaskId",
                principalTable: "Assignment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_WorkItemTypes_TypeId",
                table: "UserStories",
                column: "TypeId",
                principalTable: "WorkItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_UserStories_WorkItemId",
                table: "UserStories",
                column: "WorkItemId",
                principalTable: "UserStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Sprints_SprintId",
                table: "Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_AspNetUsers_UserId",
                table: "Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_UserStories_UserStoryId",
                table: "Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTask_Assignment_TaskId",
                table: "SubTask");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_WorkItemTypes_TypeId",
                table: "UserStories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_UserStories_WorkItemId",
                table: "UserStories");

            migrationBuilder.DropTable(
                name: "WorkItemTypes");

            migrationBuilder.DropIndex(
                name: "IX_UserStories_TypeId",
                table: "UserStories");

            migrationBuilder.DropIndex(
                name: "IX_UserStories_WorkItemId",
                table: "UserStories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubTask",
                table: "SubTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assignment",
                table: "Assignment");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "UserStories");

            migrationBuilder.DropColumn(
                name: "WorkItemId",
                table: "UserStories");

            migrationBuilder.RenameTable(
                name: "SubTask",
                newName: "SubTasks");

            migrationBuilder.RenameTable(
                name: "Assignment",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_SubTask_TaskId",
                table: "SubTasks",
                newName: "IX_SubTasks_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignment_UserStoryId",
                table: "Tasks",
                newName: "IX_Tasks_UserStoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignment_UserId",
                table: "Tasks",
                newName: "IX_Tasks_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignment_SprintId",
                table: "Tasks",
                newName: "IX_Tasks_SprintId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubTasks",
                table: "SubTasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTasks_Tasks_TaskId",
                table: "SubTasks",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Sprints_SprintId",
                table: "Tasks",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_UserStories_UserStoryId",
                table: "Tasks",
                column: "UserStoryId",
                principalTable: "UserStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
