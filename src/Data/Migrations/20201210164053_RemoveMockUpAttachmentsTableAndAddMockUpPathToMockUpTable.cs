using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class RemoveMockUpAttachmentsTableAndAddMockUpPathToMockUpTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mockup_Bugs_BugId",
                table: "Mockup");

            migrationBuilder.DropForeignKey(
                name: "FK_Mockup_Tests_TestId",
                table: "Mockup");

            migrationBuilder.DropForeignKey(
                name: "FK_Mockup_UserStories_WorkItemId",
                table: "Mockup");

            migrationBuilder.DropTable(
                name: "MockupAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mockup",
                table: "Mockup");

            migrationBuilder.DropIndex(
                name: "IX_Mockup_WorkItemId",
                table: "Mockup");

            migrationBuilder.RenameTable(
                name: "Mockup",
                newName: "Mockups");

            migrationBuilder.RenameIndex(
                name: "IX_Mockup_TestId",
                table: "Mockups",
                newName: "IX_Mockups_TestId");

            migrationBuilder.RenameIndex(
                name: "IX_Mockup_BugId",
                table: "Mockups",
                newName: "IX_Mockups_BugId");

            migrationBuilder.AddColumn<string>(
                name: "MockUpPath",
                table: "Mockups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserStoryId",
                table: "Mockups",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mockups",
                table: "Mockups",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Mockups_UserStoryId",
                table: "Mockups",
                column: "UserStoryId");

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
                name: "FK_Mockups_UserStories_UserStoryId",
                table: "Mockups",
                column: "UserStoryId",
                principalTable: "UserStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mockups_Bugs_BugId",
                table: "Mockups");

            migrationBuilder.DropForeignKey(
                name: "FK_Mockups_Tests_TestId",
                table: "Mockups");

            migrationBuilder.DropForeignKey(
                name: "FK_Mockups_UserStories_UserStoryId",
                table: "Mockups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mockups",
                table: "Mockups");

            migrationBuilder.DropIndex(
                name: "IX_Mockups_UserStoryId",
                table: "Mockups");

            migrationBuilder.DropColumn(
                name: "MockUpPath",
                table: "Mockups");

            migrationBuilder.DropColumn(
                name: "UserStoryId",
                table: "Mockups");

            migrationBuilder.RenameTable(
                name: "Mockups",
                newName: "Mockup");

            migrationBuilder.RenameIndex(
                name: "IX_Mockups_TestId",
                table: "Mockup",
                newName: "IX_Mockup_TestId");

            migrationBuilder.RenameIndex(
                name: "IX_Mockups_BugId",
                table: "Mockup",
                newName: "IX_Mockup_BugId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mockup",
                table: "Mockup",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MockupAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MockupId = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MockupAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MockupAttachments_Mockup_MockupId",
                        column: x => x.MockupId,
                        principalTable: "Mockup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mockup_WorkItemId",
                table: "Mockup",
                column: "WorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MockupAttachments_MockupId",
                table: "MockupAttachments",
                column: "MockupId");

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
        }
    }
}
