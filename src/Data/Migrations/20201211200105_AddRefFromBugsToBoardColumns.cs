using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddRefFromBugsToBoardColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KanbanBoardColumnId",
                table: "Bugs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bugs_KanbanBoardColumnId",
                table: "Bugs",
                column: "KanbanBoardColumnId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bugs_KanbanBoardColumns_KanbanBoardColumnId",
                table: "Bugs",
                column: "KanbanBoardColumnId",
                principalTable: "KanbanBoardColumns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bugs_KanbanBoardColumns_KanbanBoardColumnId",
                table: "Bugs");

            migrationBuilder.DropIndex(
                name: "IX_Bugs_KanbanBoardColumnId",
                table: "Bugs");

            migrationBuilder.DropColumn(
                name: "KanbanBoardColumnId",
                table: "Bugs");
        }
    }
}
