using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PslibThesesBackend.Migrations
{
    public partial class UserIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkRoles_SetRoles_RoleId",
                table: "WorkRoles");

            migrationBuilder.DropIndex(
                name: "IX_WorkRoles_RoleId",
                table: "WorkRoles");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "WorkRoles");

            migrationBuilder.AddColumn<byte[]>(
                name: "IconImage",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconImageType",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LockedChange",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkRoles_SetRoleId",
                table: "WorkRoles",
                column: "SetRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkRoles_SetRoles_SetRoleId",
                table: "WorkRoles",
                column: "SetRoleId",
                principalTable: "SetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkRoles_SetRoles_SetRoleId",
                table: "WorkRoles");

            migrationBuilder.DropIndex(
                name: "IX_WorkRoles_SetRoleId",
                table: "WorkRoles");

            migrationBuilder.DropColumn(
                name: "IconImage",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IconImageType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockedChange",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "WorkRoles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkRoles_RoleId",
                table: "WorkRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkRoles_SetRoles_RoleId",
                table: "WorkRoles",
                column: "RoleId",
                principalTable: "SetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
