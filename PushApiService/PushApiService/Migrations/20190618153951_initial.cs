using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PushApiService.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastLogin = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    Os = table.Column<int>(nullable: false),
                    OsVersion = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    AppVersion = table.Column<string>(nullable: true),
                    PushToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DeviceId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Token_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Token_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Token_DeviceId",
                table: "Token",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Token_UserId",
                table: "Token",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "AppUser");
        }
    }
}
