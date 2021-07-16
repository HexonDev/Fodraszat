using Microsoft.EntityFrameworkCore.Migrations;

namespace Fodraszat.Data.Migrations
{
    public partial class AddProfilepictureToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Profilkep",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "default.jpg");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Profilkep",
                table: "AspNetUsers");
        }
    }
}
