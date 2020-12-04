using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddBoardColumnTableAndRefToUserStories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoardColumnId",
                table: "UserStories",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BoardColumn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ColumnName = table.Column<string>(nullable: true),
                    MaxItems = table.Column<int>(nullable: false),
                    ColumnOrder = table.Column<byte>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardColumn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardColumn_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_BoardColumnId",
                table: "UserStories",
                column: "BoardColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardColumn_ProjectId",
                table: "BoardColumn",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_BoardColumn_BoardColumnId",
                table: "UserStories",
                column: "BoardColumnId",
                principalTable: "BoardColumn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_BoardColumn_BoardColumnId",
                table: "UserStories");

            migrationBuilder.DropTable(
                name: "BoardColumn");

            migrationBuilder.DropIndex(
                name: "IX_UserStories_BoardColumnId",
                table: "UserStories");

            migrationBuilder.DropColumn(
                name: "BoardColumnId",
                table: "UserStories");
        }
    }
}
