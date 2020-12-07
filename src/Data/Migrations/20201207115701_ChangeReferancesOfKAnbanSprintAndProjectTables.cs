using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ChangeReferancesOfKAnbanSprintAndProjectTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SprintId",
                table: "KanbanBoardColumns",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "KanbanBoardColumnOptions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KanbanBoardColumns_SprintId",
                table: "KanbanBoardColumns",
                column: "SprintId",
                unique: true,
                filter: "[SprintId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanBoardColumnOptions_ProjectId",
                table: "KanbanBoardColumnOptions",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanBoardColumnOptions_Projects_ProjectId",
                table: "KanbanBoardColumnOptions",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KanbanBoardColumns_Sprints_SprintId",
                table: "KanbanBoardColumns",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanbanBoardColumnOptions_Projects_ProjectId",
                table: "KanbanBoardColumnOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_KanbanBoardColumns_Sprints_SprintId",
                table: "KanbanBoardColumns");

            migrationBuilder.DropIndex(
                name: "IX_KanbanBoardColumns_SprintId",
                table: "KanbanBoardColumns");

            migrationBuilder.DropIndex(
                name: "IX_KanbanBoardColumnOptions_ProjectId",
                table: "KanbanBoardColumnOptions");

            migrationBuilder.DropColumn(
                name: "SprintId",
                table: "KanbanBoardColumns");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "KanbanBoardColumnOptions");
        }
    }
}
