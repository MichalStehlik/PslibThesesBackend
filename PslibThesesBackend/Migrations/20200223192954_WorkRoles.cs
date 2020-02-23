using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PslibThesesBackend.Migrations
{
    public partial class WorkRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkGoals_Ideas_WorkId",
                table: "WorkGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkGoals_Works_WorkId1",
                table: "WorkGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOutlines_Ideas_WorkId",
                table: "WorkOutlines");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOutlines_Works_WorkId1",
                table: "WorkOutlines");

            migrationBuilder.DropIndex(
                name: "IX_WorkOutlines_WorkId1",
                table: "WorkOutlines");

            migrationBuilder.DropIndex(
                name: "IX_WorkGoals_WorkId1",
                table: "WorkGoals");

            migrationBuilder.DropColumn(
                name: "WorkId1",
                table: "WorkOutlines");

            migrationBuilder.DropColumn(
                name: "WorkId1",
                table: "WorkGoals");

            migrationBuilder.CreateTable(
                name: "WorkRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkId = table.Column<int>(nullable: false),
                    SetRoleId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: true),
                    Mark = table.Column<int>(nullable: true),
                    Finalized = table.Column<bool>(nullable: false),
                    Review = table.Column<string>(nullable: true),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkRoles_SetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkRoles_Works_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkRoleUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkRoleId = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkRoleUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkRoleUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkRoleUsers_WorkRoles_WorkRoleId",
                        column: x => x.WorkRoleId,
                        principalTable: "WorkRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkRoles_RoleId",
                table: "WorkRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkRoles_WorkId",
                table: "WorkRoles",
                column: "WorkId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkRoleUsers_UserId",
                table: "WorkRoleUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkRoleUsers_WorkRoleId",
                table: "WorkRoleUsers",
                column: "WorkRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkGoals_Works_WorkId",
                table: "WorkGoals",
                column: "WorkId",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOutlines_Works_WorkId",
                table: "WorkOutlines",
                column: "WorkId",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkGoals_Works_WorkId",
                table: "WorkGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOutlines_Works_WorkId",
                table: "WorkOutlines");

            migrationBuilder.DropTable(
                name: "WorkRoleUsers");

            migrationBuilder.DropTable(
                name: "WorkRoles");

            migrationBuilder.AddColumn<int>(
                name: "WorkId1",
                table: "WorkOutlines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkId1",
                table: "WorkGoals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkOutlines_WorkId1",
                table: "WorkOutlines",
                column: "WorkId1");

            migrationBuilder.CreateIndex(
                name: "IX_WorkGoals_WorkId1",
                table: "WorkGoals",
                column: "WorkId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkGoals_Ideas_WorkId",
                table: "WorkGoals",
                column: "WorkId",
                principalTable: "Ideas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkGoals_Works_WorkId1",
                table: "WorkGoals",
                column: "WorkId1",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOutlines_Ideas_WorkId",
                table: "WorkOutlines",
                column: "WorkId",
                principalTable: "Ideas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOutlines_Works_WorkId1",
                table: "WorkOutlines",
                column: "WorkId1",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
