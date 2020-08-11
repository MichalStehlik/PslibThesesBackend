using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PslibThesesBackend.Migrations
{
    public partial class IdeaOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ideas_Users_UserId",
                table: "Ideas");

            migrationBuilder.DropColumn(
                name: "Offered",
                table: "Ideas");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Ideas",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IdeaOffers",
                columns: table => new
                {
                    IdeaId = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdeaOffers", x => new { x.IdeaId, x.UserId });
                    table.ForeignKey(
                        name: "FK_IdeaOffers_Ideas_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "Ideas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdeaOffers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ideas_UserId1",
                table: "Ideas",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_IdeaOffers_UserId",
                table: "IdeaOffers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ideas_Users_UserId",
                table: "Ideas",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ideas_Users_UserId1",
                table: "Ideas",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ideas_Users_UserId",
                table: "Ideas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ideas_Users_UserId1",
                table: "Ideas");

            migrationBuilder.DropTable(
                name: "IdeaOffers");

            migrationBuilder.DropIndex(
                name: "IX_Ideas_UserId1",
                table: "Ideas");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Ideas");

            migrationBuilder.AddColumn<bool>(
                name: "Offered",
                table: "Ideas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Ideas_Users_UserId",
                table: "Ideas",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
