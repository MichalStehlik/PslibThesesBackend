using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Authority.Migrations
{
    public partial class ImageStoringExtension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "OriginalImage",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalImageType",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PictureImage",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureImageType",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ADMIN",
                column: "ConcurrencyStamp",
                value: "2c944047-b05c-4a03-b0cf-a12745fdeab8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "EXT",
                column: "ConcurrencyStamp",
                value: "d9af54a4-3dd7-4c99-97a1-85114cdb2e14");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "MANAGER",
                column: "ConcurrencyStamp",
                value: "2aacadb1-d5ef-49ab-8579-ef904c507fa2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "STUDENT",
                column: "ConcurrencyStamp",
                value: "1a890692-09ce-4d1c-858f-c4d6163e8f3c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "TEACHER",
                column: "ConcurrencyStamp",
                value: "ca20fe84-9731-4182-b90f-72537580a7ba");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMINUSER",
                columns: new[] { "ConcurrencyStamp", "Gender", "PasswordHash" },
                values: new object[] { "86ca259d-f876-4d1e-aac5-f7d004395553", 0, "AQAAAAEAACcQAAAAEED/yKhn0tNF003c6va5hEP7W7JvzWqiPF7X3yek0iNbVIGmCjk5w2x3gIwnT+Owaw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalImage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OriginalImageType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PictureImage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PictureImageType",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ADMIN",
                column: "ConcurrencyStamp",
                value: "561fd2d2-7b13-4e25-8313-8125343da02c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "EXT",
                column: "ConcurrencyStamp",
                value: "5fcbbca6-1d89-4817-9da4-84c5dc1d58e1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "MANAGER",
                column: "ConcurrencyStamp",
                value: "7b971b56-c3d5-4990-bd24-f9191c47418c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "STUDENT",
                column: "ConcurrencyStamp",
                value: "bde6280b-f178-40d1-bf08-8380cc45b108");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "TEACHER",
                column: "ConcurrencyStamp",
                value: "ab78e561-0b5a-4102-9e7c-688cbbde8c67");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ADMINUSER",
                columns: new[] { "ConcurrencyStamp", "Gender", "PasswordHash" },
                values: new object[] { "350450ab-047d-4aac-8bc6-28fe17bea05a", 3, "AQAAAAEAACcQAAAAEPpxEh2S83xGGQfSaHD15KSRAHcwuCxsaW4Q8LpWKY+7wL6cgtR+C+pSli6yDUcwDw==" });
        }
    }
}
