using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_UserStories_UserStoryId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Mockup_UserStories_UserStoryId",
                table: "Mockup");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_BacklogPriorities_BacklogPriorityId",
                table: "UserStories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_Projects_ProjectId",
                table: "UserStories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_Sprints_SprintId",
                table: "UserStories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_UserStories_WorkItemId",
                table: "UserStories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_WorkItemTypes_WorkItemTypeId",
                table: "UserStories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStories",
                table: "UserStories");

            migrationBuilder.RenameTable(
                name: "UserStories",
                newName: "WorkItems");

            migrationBuilder.RenameIndex(
                name: "IX_UserStories_WorkItemTypeId",
                table: "WorkItems",
                newName: "IX_WorkItems_WorkItemTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_UserStories_WorkItemId",
                table: "WorkItems",
                newName: "IX_WorkItems_WorkItemId");

            migrationBuilder.RenameIndex(
                name: "IX_UserStories_SprintId",
                table: "WorkItems",
                newName: "IX_WorkItems_SprintId");

            migrationBuilder.RenameIndex(
                name: "IX_UserStories_ProjectId",
                table: "WorkItems",
                newName: "IX_WorkItems_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_UserStories_BacklogPriorityId",
                table: "WorkItems",
                newName: "IX_WorkItems_BacklogPriorityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkItems",
                table: "WorkItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_WorkItems_UserStoryId",
                table: "Comments",
                column: "UserStoryId",
                principalTable: "WorkItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mockup_WorkItems_UserStoryId",
                table: "Mockup",
                column: "UserStoryId",
                principalTable: "WorkItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_BacklogPriorities_BacklogPriorityId",
                table: "WorkItems",
                column: "BacklogPriorityId",
                principalTable: "BacklogPriorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Projects_ProjectId",
                table: "WorkItems",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_Sprints_SprintId",
                table: "WorkItems",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_WorkItems_WorkItemId",
                table: "WorkItems",
                column: "WorkItemId",
                principalTable: "WorkItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_WorkItemTypes_WorkItemTypeId",
                table: "WorkItems",
                column: "WorkItemTypeId",
                principalTable: "WorkItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_WorkItems_UserStoryId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Mockup_WorkItems_UserStoryId",
                table: "Mockup");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_BacklogPriorities_BacklogPriorityId",
                table: "WorkItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Projects_ProjectId",
                table: "WorkItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_Sprints_SprintId",
                table: "WorkItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_WorkItems_WorkItemId",
                table: "WorkItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_WorkItemTypes_WorkItemTypeId",
                table: "WorkItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkItems",
                table: "WorkItems");

            migrationBuilder.RenameTable(
                name: "WorkItems",
                newName: "UserStories");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_WorkItemTypeId",
                table: "UserStories",
                newName: "IX_UserStories_WorkItemTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_WorkItemId",
                table: "UserStories",
                newName: "IX_UserStories_WorkItemId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_SprintId",
                table: "UserStories",
                newName: "IX_UserStories_SprintId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_ProjectId",
                table: "UserStories",
                newName: "IX_UserStories_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkItems_BacklogPriorityId",
                table: "UserStories",
                newName: "IX_UserStories_BacklogPriorityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserStories",
                table: "UserStories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_UserStories_UserStoryId",
                table: "Comments",
                column: "UserStoryId",
                principalTable: "UserStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mockup_UserStories_UserStoryId",
                table: "Mockup",
                column: "UserStoryId",
                principalTable: "UserStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_BacklogPriorities_BacklogPriorityId",
                table: "UserStories",
                column: "BacklogPriorityId",
                principalTable: "BacklogPriorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_Projects_ProjectId",
                table: "UserStories",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_Sprints_SprintId",
                table: "UserStories",
                column: "SprintId",
                principalTable: "Sprints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_UserStories_WorkItemId",
                table: "UserStories",
                column: "WorkItemId",
                principalTable: "UserStories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_WorkItemTypes_WorkItemTypeId",
                table: "UserStories",
                column: "WorkItemTypeId",
                principalTable: "WorkItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
