using Microsoft.EntityFrameworkCore.Migrations;

namespace DataServer.Infrastructure.Migrations
{
    public partial class ChangedDataCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataCode",
                table: "Entries",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "AgentCode",
                table: "Agents",
                newName: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Entries",
                newName: "DataCode");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Agents",
                newName: "AgentCode");
        }
    }
}
