using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class RemoveRefFromKanbanBoardColumnsToSprints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanBoardColumns_Sprints_SprintId",
                table: "KanbanBoardColumns");

            migrationBuilder.DropForeignKey(
                name: "FK_Mockup_Bugs_BugId",
                table: "Mockup");

            migrationBuilder.DropForeignKey(
                name: "FK_Mockup_Tests_TestId",
                table: "Mockup");

            migrationBuilder.DropForeignKey(
                name: "FK_Mockup_UserStories_WorkItemId",
                table: "Mockup");

            migrationBuilder.DropForeignKey(
                name: "FK_MockupAttachments_Mockup_MockupId",
                table: "MockupAttachments");

            migrationBuilder.DropIndex(
                name: "IX_KanbanBoardColumns_SprintId",
                table: "KanbanBoardColumns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mockup",
                table: "Mockup");

            migrationBuilder.DropColumn(
                name: "SprintId",
                table: "KanbanBoardColumns");

            migrationBuilder.RenameTable(
                name: "Mockup",
                newName: "Mockups");

            migrationBuilder.RenameIndex(
                name: "IX_Mockup_WorkItemId",
                table: "Mockups",
                newName: "IX_Mockups_WorkItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Mockup_TestId",
                table: "Mockups",
                newName: "IX_Mockups_TestId");

            migrationBuilder.RenameIndex(
                name: "IX_Mockup_BugId",
                table: "Mockups",
                newName: "IX_Mockups_BugId");

            migrationBuilder.AddColumn<int>(
                name: "KanbanBoardId",
                table: "Sprints",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mockups",
                table: "Mockups",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sprints_KanbanBoardId",
                table: "Sprints",
                column: "KanbanBoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_MockupAttachments_Mockups_MockupId",
                table: "MockupAttachments",
                column: "MockupId",
                principalTable: "Mockups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mockups_Bugs_BugId",
                table: "Mockups",
                column: "BugId",
                principalTable: "Bugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mockups_Tests_TestId",
                table: "Mockups",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mockups_UserStories_WorkItemId",
                table: "Mockups",
                column: "WorkItemId",
                principalTable: "UserStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sprints_KanbanBoardColumns_KanbanBoardId",
                table: "Sprints",
                column: "KanbanBoardId",
                principalTable: "KanbanBoardColumns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MockupAttachments_Mockups_MockupId",
                table: "MockupAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Mockups_Bugs_BugId",
                table: "Mockups");

            migrationBuilder.DropForeignKey(
                name: "FK_Mockups_Tests_TestId",
                table: "Mockups");

            migrationBuilder.DropForeignKey(
                name: "FK_Mockups_UserStories_WorkItemId",
                table: "Mockups");

            migrationBuilder.DropForeignKey(
                name: "FK_Sprints_KanbanBoardColumns_KanbanBoardId",
                table: "Sprints");

            migrationBuilder.DropIndex(
                name: "IX_Sprints_KanbanBoardId",
                table: "Sprints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mockups",
                table: "Mockups");

            migrationBuilder.DropColumn(
                name: "KanbanBoardId",
                table: "Sprints");

            migrationBuilder.RenameTable(
                name: "Mockups",
                newName: "Mockup");

            migrationBuilder.RenameIndex(
                name: "IX_Mockups_WorkItemId",
                table: "Mockup",
                newName: "IX_Mockup_WorkItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Mockups_TestId",
                table: "Mockup",
                newName: "IX_Mockup_TestId");

            migrationBuilder.RenameIndex(
                name: "IX_Mockups_BugId",
                table: "Mockup",
                newName: "IX_Mockup_BugId");

            migrationBuilder.AddColumn<int>(
                name: "SprintId",
                table: "KanbanBoardColumns",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mockup",
                table: "Mockup",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanBoardColumns_SprintId",
                table: "KanbanBoardColumns",
                column: "SprintId",
                unique: true,
                filter: "[SprintId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanBoardColumns_Sprints_SprintId",
                table: "KanbanBoardColumns",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mockup_Bugs_BugId",
                table: "Mockup",
                column: "BugId",
                principalTable: "Bugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mockup_Tests_TestId",
                table: "Mockup",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mockup_UserStories_WorkItemId",
                table: "Mockup",
                column: "WorkItemId",
                principalTable: "UserStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MockupAttachments_Mockup_MockupId",
                table: "MockupAttachments",
                column: "MockupId",
                principalTable: "Mockup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
