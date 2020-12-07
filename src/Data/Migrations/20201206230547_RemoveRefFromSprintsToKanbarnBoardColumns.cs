using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class RemoveRefFromSprintsToKanbarnBoardColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sprints_KanbanBoardColumns_KanbanBoardId",
                table: "Sprints");

            migrationBuilder.DropIndex(
                name: "IX_Sprints_KanbanBoardId",
                table: "Sprints");

            migrationBuilder.DropColumn(
                name: "KanbanBoardId",
                table: "Sprints");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KanbanBoardId",
                table: "Sprints",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sprints_KanbanBoardId",
                table: "Sprints",
                column: "KanbanBoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sprints_KanbanBoardColumns_KanbanBoardId",
                table: "Sprints",
                column: "KanbanBoardId",
                principalTable: "KanbanBoardColumns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
