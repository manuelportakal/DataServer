using Microsoft.EntityFrameworkCore.Migrations;

namespace DataServer.Infrastructure.Migrations
{
    public partial class AddedSecurityProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecurityToken",
                table: "Agents",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecurityToken",
                table: "Agents");
        }
    }
}
