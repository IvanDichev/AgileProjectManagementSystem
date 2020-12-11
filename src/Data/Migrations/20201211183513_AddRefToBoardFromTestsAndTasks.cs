using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddRefToBoardFromTestsAndTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KanbanBoardColumnId",
                table: "UserStoryTasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KanbanBoardColumnId",
                table: "Tests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserStoryTasks_KanbanBoardColumnId",
                table: "UserStoryTasks",
                column: "KanbanBoardColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_KanbanBoardColumnId",
                table: "Tests",
                column: "KanbanBoardColumnId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_KanbanBoardColumns_KanbanBoardColumnId",
                table: "Tests",
                column: "KanbanBoardColumnId",
                principalTable: "KanbanBoardColumns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStoryTasks_KanbanBoardColumns_KanbanBoardColumnId",
                table: "UserStoryTasks",
                column: "KanbanBoardColumnId",
                principalTable: "KanbanBoardColumns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_KanbanBoardColumns_KanbanBoardColumnId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStoryTasks_KanbanBoardColumns_KanbanBoardColumnId",
                table: "UserStoryTasks");

            migrationBuilder.DropIndex(
                name: "IX_UserStoryTasks_KanbanBoardColumnId",
                table: "UserStoryTasks");

            migrationBuilder.DropIndex(
                name: "IX_Tests_KanbanBoardColumnId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "KanbanBoardColumnId",
                table: "UserStoryTasks");

            migrationBuilder.DropColumn(
                name: "KanbanBoardColumnId",
                table: "Tests");
        }
    }
}
