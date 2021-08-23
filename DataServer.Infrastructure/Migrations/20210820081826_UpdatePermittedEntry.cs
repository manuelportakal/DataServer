using Microsoft.EntityFrameworkCore.Migrations;

namespace DataServer.Infrastructure.Migrations
{
    public partial class UpdatePermittedEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSignatureEnabled",
                table: "PermittedEntries",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSignatureEnabled",
                table: "PermittedEntries");
        }
    }
}
