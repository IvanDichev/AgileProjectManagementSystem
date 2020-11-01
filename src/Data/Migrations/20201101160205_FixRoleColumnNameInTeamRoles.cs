using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class FixRoleColumnNameInTeamRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rolse",
                table: "TeamRoles");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "TeamRoles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "TeamRoles");

            migrationBuilder.AddColumn<string>(
                name: "Rolse",
                table: "TeamRoles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
