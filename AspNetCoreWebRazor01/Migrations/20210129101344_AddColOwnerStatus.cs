using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreWebRazor01.Migrations
{
    public partial class AddColOwnerStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "Movie",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Movie",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Movie");
        }
    }
}
