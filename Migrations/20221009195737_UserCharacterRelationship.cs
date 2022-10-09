using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetrpg.Migrations
{
    public partial class UserCharacterRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userOwnerId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_userOwnerId",
                table: "Characters",
                column: "userOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Users_userOwnerId",
                table: "Characters",
                column: "userOwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Users_userOwnerId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_userOwnerId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "userOwnerId",
                table: "Characters");
        }
    }
}
