using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Authority.Migrations
{
    public partial class AddIconColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "IconImage",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ADMIN",
                column: "ConcurrencyStamp",
                value: "44bc14ab-4a40-41f5-b319-8009afca291f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "EXT",
                column: "ConcurrencyStamp",
                value: "a0aab0f7-54b8-4877-bfe3-a0bf17344cbc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "MANAGER",
                column: "ConcurrencyStamp",
                value: "e9c67070-7890-43c7-aa00-47df13e71d2e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "STUDENT",
                column: "ConcurrencyStamp",
                value: "ca556f7f-30ed-4bb0-badc-983655a8518f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "TEACHER",
                column: "ConcurrencyStamp",
                value: "5069d59d-039d-4c79-bde9-c0ffc65f6a29");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMINUSER",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "c2f9594a-f703-4283-9cde-59d64d039588", "ST@PSLIB.CLOUD", "ST@PSLIB.CLOUD", "AQAAAAEAACcQAAAAEEWrdOPfW5dBQ16EXKSPhZ28eLNTWa9czz1l/erzooUNioBWKZuTR3ykNJzBCfxkCQ==", "st@pslib.cloud" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconImage",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ADMIN",
                column: "ConcurrencyStamp",
                value: "1faf9e91-14be-4cbd-a018-5df394db1109");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "EXT",
                column: "ConcurrencyStamp",
                value: "1c8bf5ae-398a-4848-a3c3-7407174a0e26");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "MANAGER",
                column: "ConcurrencyStamp",
                value: "c5befb28-a3c4-46fe-a839-e6df3cf7d0a6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "STUDENT",
                column: "ConcurrencyStamp",
                value: "8f2c36ca-b187-4cd4-96c6-44b1f1105e21");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "TEACHER",
                column: "ConcurrencyStamp",
                value: "8f3b1a16-32cb-4293-b341-61860b3871ce");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMINUSER",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "a5311857-faf5-458d-962a-d1a9cde8c711", null, null, "AQAAAAEAACcQAAAAEFuOJWNbL9TT69lpz7nMIr9Ew+PCAtEOyiH0qXsqdv4UsT5yVUja/Ximzgr6RAHfSQ==", "Admin" });
        }
    }
}
