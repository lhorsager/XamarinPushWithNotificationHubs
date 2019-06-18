using Microsoft.EntityFrameworkCore.Migrations;

namespace PushApiService.Migrations
{
    public partial class fixdevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PushToken",
                table: "Device");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PushToken",
                table: "Device",
                nullable: true);
        }
    }
}
