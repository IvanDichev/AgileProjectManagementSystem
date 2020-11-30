using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddDeleteCascadeOnWorkItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_WorkItems_WorkItemId",
                table: "WorkItems");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_WorkItems_WorkItemId",
                table: "WorkItems",
                column: "WorkItemId",
                principalTable: "WorkItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_WorkItems_WorkItemId",
                table: "WorkItems");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_WorkItems_WorkItemId",
                table: "WorkItems",
                column: "WorkItemId",
                principalTable: "WorkItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
