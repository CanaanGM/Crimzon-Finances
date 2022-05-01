using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class relations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Transfers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Purchases",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_AppUserId",
                table: "Transfers",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_AppUserId",
                table: "Purchases",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_AspNetUsers_AppUserId",
                table: "Purchases",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_AspNetUsers_AppUserId",
                table: "Transfers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_AspNetUsers_AppUserId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_AspNetUsers_AppUserId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_AppUserId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_AppUserId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Purchases");
        }
    }
}
