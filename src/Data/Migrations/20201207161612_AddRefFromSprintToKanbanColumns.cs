using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddRefFromSprintToKanbanColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_KanbanBoardColumns_SprintId",
                table: "KanbanBoardColumns");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanBoardColumns_SprintId",
                table: "KanbanBoardColumns",
                column: "SprintId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_KanbanBoardColumns_SprintId",
                table: "KanbanBoardColumns");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanBoardColumns_SprintId",
                table: "KanbanBoardColumns",
                column: "SprintId",
                unique: true,
                filter: "[SprintId] IS NOT NULL");
        }
    }
}
