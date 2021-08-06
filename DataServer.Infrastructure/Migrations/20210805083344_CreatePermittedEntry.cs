using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataServer.Infrastructure.Migrations
{
    public partial class CreatePermittedEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PermittedEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermittedEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermittedEntries_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermittedEntries_AgentId",
                table: "PermittedEntries",
                column: "AgentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermittedEntries");
        }
    }
}
