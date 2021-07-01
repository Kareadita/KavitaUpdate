using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KavitaUpdate.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Updates",
                columns: table => new
                {
                    UpdateEntityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Version = table.Column<string>(type: "TEXT", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Branch = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    New = table.Column<string>(type: "TEXT", nullable: true),
                    Fixed = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Updates", x => x.UpdateEntityId);
                });

            migrationBuilder.CreateTable(
                name: "UpdateFiles",
                columns: table => new
                {
                    UpdateEntityId = table.Column<int>(type: "INTEGER", nullable: false),
                    OperatingSystem = table.Column<int>(type: "INTEGER", nullable: false),
                    Runtime = table.Column<int>(type: "INTEGER", nullable: false),
                    Architecture = table.Column<int>(type: "INTEGER", nullable: false),
                    Filename = table.Column<string>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    Hash = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateFiles", x => new { x.UpdateEntityId, x.OperatingSystem, x.Architecture, x.Runtime });
                    table.ForeignKey(
                        name: "FK_UpdateFiles_Updates_UpdateEntityId",
                        column: x => x.UpdateEntityId,
                        principalTable: "Updates",
                        principalColumn: "UpdateEntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Updates_Branch_Version",
                table: "Updates",
                columns: new[] { "Branch", "Version" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpdateFiles");

            migrationBuilder.DropTable(
                name: "Updates");
        }
    }
}
