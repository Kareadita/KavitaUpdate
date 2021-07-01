using Microsoft.EntityFrameworkCore.Migrations;

namespace KavitaUpdate.Migrations
{
    public partial class AddedChangedToChangelog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Changed",
                table: "Updates",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Changed",
                table: "Updates");
        }
    }
}
