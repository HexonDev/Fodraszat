using Microsoft.EntityFrameworkCore.Migrations;

namespace Fodraszat.Data.Migrations
{
    public partial class AddDescriptionToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Leiras",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Leiras",
                table: "AspNetUsers");
        }
    }
}
